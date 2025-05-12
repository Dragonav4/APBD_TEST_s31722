using APBD_s31722_TEST_TEMPLATE.DataLayer;
using APBD_s31722_TEST_TEMPLATE.DataLayer.Models;
using APBD_s31722_TEST_TEMPLATE.Exceptions;
using APBD_s31722_TEST_TEMPLATE.Interfaces;
using APBD_s31722_TEST_TEMPLATE.Utils;

namespace APBD_s31722_TEST_TEMPLATE.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IDbClient _dbClient;

    public AppointmentService(IDbClient dbClient) => _dbClient = dbClient;

    private const string GetAllApointmentsById = @"
            select date,p.first_name,p.last_name,p.date_of_birth,D.doctor_id,D.pwz,s.name,aps.service_fee
FROM Appointment ap
JOIN dbo.Patient P on P.patient_id = ap.patient_id
JOIN dbo.Doctor D on D.doctor_id = ap.doctor_id
JOIN Appointment_Service aps ON ap.appointment_id = aps.appointment_id
JOIN Service s ON aps.service_id = s.service_id
WHERE ap.appointment_id = @appointmentId";

    public async Task<AppointmentResponseDto> GetAppointmentAsync(int appointmentId)
    {
        var appointmentById = _dbClient.ReadDataAsync(GetAllApointmentsById,
            reader => new
            {
                Date = reader.GetDateTime(reader.GetOrdinal("date")),
                FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                LastName = reader.GetString(reader.GetOrdinal("last_name")),
                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                DoctorId = reader.GetInt32(reader.GetOrdinal("doctor_id")),
                Pwz = reader.GetString(reader.GetOrdinal("pwz")),
                ServiceName = reader.GetString(reader.GetOrdinal("name")),
                ServiceFee = reader.GetDecimal(reader.GetOrdinal("service_fee"))
            },
            new Dictionary<string, object> { ["@appointmentId"] = appointmentId }
        );

        var rows = await appointmentById.ToListAsync();
        if (!rows.Any()) throw new ArgumentException("No appointment found");

        var first = rows.First();
        return new AppointmentResponseDto //todo rebuild and make a VIEW-MODEL
        {
            Date = first.Date,
            Patient = new PatientDto
            {
                FirstName = first.FirstName,
                LastName = first.LastName,
                DateOfBirth = first.DateOfBirth
            },
            Doctor = new DoctorDto
            {
                DoctorId = first.DoctorId,
                Pwz = first.Pwz
            },
            AppointmentServices = rows
                .Select(r => new AppointmentServiceDto
                {
                    Name = r.ServiceName,
                    ServiceFee = r.ServiceFee
                })
                .ToList()
        };
    }

    public async Task<CreateAppointmentResponseDto> CreateAppointmentAsync(CreateAppointmentDto dto)
    {
        var duplicateAppointment = await _dbClient.ReadScalarAsync<int?>(
            "SELECT COUNT(*) FROM Appointment WHERE appointment_id = @appointmentId",
            new Dictionary<string, object> { ["@appointmentId"] = dto.AppointmentId }
        );
        if (duplicateAppointment.GetValueOrDefault() > 0)
            throw new BadRequestException($"Appointment {dto.AppointmentId} already exists :<");

        var patientExists = await _dbClient.ReadScalarAsync<int?>(
            "SELECT COUNT(*) FROM Patient WHERE patient_id = @patientId",
            new Dictionary<string, object> { ["@patientId"] = dto.PatientId }
        );
        if (patientExists.GetValueOrDefault() == 0)
            throw new BadRequestException($"Patient {dto.PatientId} not found :/");

        var doctorId = await _dbClient.ReadScalarAsync<int?>(
            "SELECT doctor_id FROM Doctor WHERE pwz = @pwz",
            new Dictionary<string, object> { ["@pwz"] = dto.Pwz }
        );
        if (!doctorId.HasValue)
            throw new BadRequestException($"Doctor with PWZ '{dto.Pwz}' not found");

        var serviceExists = new List<(int ServiceId, decimal Fee)>();
        foreach (var serviceDto in dto.Services)
        {
            var serviceId = await _dbClient.ReadScalarAsync<int?>(
                "SELECT service_id FROM Service WHERE name = @name",
                new Dictionary<string, object> { ["@name"] = serviceDto.ServiceName }
            );
            if (!serviceId.HasValue) throw new BadRequestException($"Service '{serviceDto.ServiceName}' not found");

            serviceExists.Add((serviceId.Value, serviceDto.ServiceFee));
        }

        var commands = new List<CommandConfig>
        {
            new CommandConfig
            {
                Query = @"
                    INSERT INTO Appointment
                       (appointment_id, patient_id, doctor_id, date)
                    VALUES
                       (@AppointmentId,@PatientId,@DoctorId,@Date)",
                Parameters = new
                {
                    dto.AppointmentId,
                    dto.PatientId,
                    DoctorId = doctorId.Value,
                    Date = DateTime.Now
                }
            }
        };

        commands.AddRange(serviceExists.Select(s =>
            new CommandConfig
            {
                Query = @"
                    INSERT INTO Appointment_Service
                       (appointment_id, service_id, service_fee)
                    VALUES
                       (@AppointmentId,@ServiceId,@ServiceFee)",
                Parameters = new
                {
                    dto.AppointmentId,
                    s.ServiceId,
                    ServiceFee = s.Fee
                }
            }));

        await _dbClient.ExecuteNonQueriesAsTransactionAsync(commands);

        return new CreateAppointmentResponseDto
        {
            AppointmentId = dto.AppointmentId,
            PatientId = dto.PatientId,
            Pwz = dto.Pwz,
            Services = dto.Services
        };
    }
}
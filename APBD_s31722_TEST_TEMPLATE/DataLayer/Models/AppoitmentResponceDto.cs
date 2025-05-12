namespace APBD_s31722_TEST_TEMPLATE.DataLayer.Models;

public class AppointmentResponseDto
{
    public DateTime Date { get; set; }
    public PatientDto Patient { get; set; }
    public DoctorDto Doctor { get; set; }
    public List<AppointmentServiceDto> AppointmentServices { get; set; }
    

    //Not eplemented with DTO class yet sorry for that :|
    
    // public AppointmentResponseDto(IEnumerable<AppointmentResponseDto> requests)
    // {
    //     var data = requests.ToList();
    //     var first = data.FirstOrDefault();
    //     if (first == null) return;
    //     Date = first.Date;
    //     Patient = new PatientDto
    //     {
    //         FirstName = first.Patient.FirstName,
    //         LastName = first.Patient.LastName,
    //         DateOfBirth = first.Patient.DateOfBirth,
    //     };
    //     Doctor = new DoctorDto
    //     {
    //         DoctorId = first.Doctor.DoctorId,
    //         Pwz = first.Doctor.Pwz,
    //     };
    //     AppointmentServices = requests
    //         .Select(r => new AppointmentServiceDto
    //         {
    //             Name = r.AppointmentServices.First().Name,
    //             ServiceFee = r.AppointmentServices.First().ServiceFee,
    //         })
    //         .ToList();
    //
    // }
}




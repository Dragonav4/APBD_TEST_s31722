
using APBD_s31722_TEST_TEMPLATE.DataLayer.Models;

namespace APBD_s31722_TEST_TEMPLATE.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentResponseDto> GetAppointmentAsync(int appointmentId);

    Task<CreateAppointmentResponseDto> CreateAppointmentAsync(CreateAppointmentDto dto);
}
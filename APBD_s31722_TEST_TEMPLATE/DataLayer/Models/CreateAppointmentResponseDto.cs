namespace APBD_s31722_TEST_TEMPLATE.DataLayer.Models;

public class CreateAppointmentResponseDto
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public string Pwz { get; set; }
    public List<CreateAppointmentServiceDto> Services { get; set; }
}
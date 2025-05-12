using System.ComponentModel.DataAnnotations;

namespace APBD_s31722_TEST_TEMPLATE.DataLayer.Models;

public class CreateAppointmentDto
{
    [Required] 
    public int AppointmentId { get; set; }

    [Required] 
    public int PatientId { get; set; }

    [Required]
    public string Pwz { get; set; }

    [Required, MinLength(1)] 
    public List<CreateAppointmentServiceDto> Services { get; set; }
}
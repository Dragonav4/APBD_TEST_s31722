using System.ComponentModel.DataAnnotations;

namespace APBD_s31722_TEST_TEMPLATE.DataLayer.Models;

public class CreateAppointmentServiceDto
{
    [Required]
    public string ServiceName { get; set; }

    [Required]
    public decimal ServiceFee { get; set; }
}
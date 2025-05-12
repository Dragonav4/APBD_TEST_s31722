using APBD_s31722_TEST_TEMPLATE.DataLayer.Models;
using APBD_s31722_TEST_TEMPLATE.Exceptions;
using APBD_s31722_TEST_TEMPLATE.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APBD_s31722_TEST_TEMPLATE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointmentsById(int id)
    {
        var dto = await _appointmentService.GetAppointmentAsync(id);
        if (dto is null) throw new InternalServerErrorException("Appointment not found :/");
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> AddAppointment([FromBody] CreateAppointmentDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _appointmentService.CreateAppointmentAsync(dto);
        return CreatedAtAction(
            nameof(GetAppointmentsById),
            new { id = created.AppointmentId },
            created
        );
    }
}
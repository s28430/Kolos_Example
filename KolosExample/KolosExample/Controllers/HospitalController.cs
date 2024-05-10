using KolosExample.Exceptions;
using KolosExample.Models.DTO;
using KolosExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace KolosExample.Controllers;

[ApiController]
[Route("api/prescriptions")]
public class HospitalController(IHospitalService service) : ControllerBase
{
    private readonly IHospitalService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetPrescriptionsAsync([FromQuery] string? doctorLastName)
    {
        try
        {
            return Ok(await _service.GetPrescriptionsAsync(doctorLastName));
        }
        catch (InvalidDatabaseEntryException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        catch (DoctorNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescriptionAsync(PostPrescriptionRequestDto prescriptionRequestDto)
    {
        return Ok(await _service.AddPrescriptionAsync(prescriptionRequestDto));
    }
}
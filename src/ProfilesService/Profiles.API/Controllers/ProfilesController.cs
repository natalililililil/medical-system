using MediatR;
using MedicalSystem.Shared.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Profiles.Application.Features.Commands.Doctor.Create;
using Profiles.Application.Features.Commands.Patient.Create;
using Profiles.Application.Features.Commands.Receptionist.Create;

namespace Profiles.API.Controllers;

[ApiController]
[Route("api/profiles")]
[EnableRateLimiting("WritePolicy")]
public class ProfilesController(IMediator _mediator, ILogger<ProfilesController> _logger) : ControllerBase
{
    [HttpPost("patients")]
    public async Task<ActionResult> CreatePatient([FromBody] CreatePatientCommand command)
    {
        _logger.LogInformation("Attempting to create patient profile");

        await _mediator.Send(command);

        return Ok(new MessageResponse("Patient created successfully"));
    }

    [HttpPost("doctors")]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command)
    {
        _logger.LogInformation("Attempting to create doctor profile");

        await _mediator.Send(command);

        return Ok(new MessageResponse("Doctor created successfully"));
    }
    [HttpPost("receptionist")]
    public async Task<IActionResult> CreateReceptionist([FromBody] CreateReceptionistCommand command)
    {
        _logger.LogInformation("Attempting to create receptionist profile");

        await _mediator.Send(command);

        return Ok(new MessageResponse("Receptionist created successfully"));
    }
}
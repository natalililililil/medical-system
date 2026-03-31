using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Profiles.API.Models.Requests;
using Profiles.Application.Features.Commands.Patient.Update;

namespace Profiles.API.Controllers;

[ApiController]
[Route("api/profiles/patient")]
public class PatientController(IMediator _mediator, ILogger<PatientController> _logger) : BaseProfilesController
{
    [Authorize(Roles = "Patient, Receptionist")]
    [HttpPut("update")]
    [EnableRateLimiting("WritePolicy")]
    public async Task<ActionResult> UpdatePatientProfile([FromBody] UpdatePatientRequest request)
    {
        _logger.LogInformation("Updating patient profile");

        await _mediator.Send(new UpdatePatientProfileCommand(CurrentAccountId, request.FirstName, request.LastName, request.MiddleName,
            request.DateOfBirth, request.Phone, request.Phone));

        return Ok(new { message = "Patient's profile updated successfully" });
    }
}

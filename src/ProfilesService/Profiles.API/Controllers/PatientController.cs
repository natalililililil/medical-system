using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Profiles.API.Models.Requests;
using Profiles.Application.Features.Commands.Patient.Update;
using Profiles.Application.Features.Queries.Patient.GetPatientById;

namespace Profiles.API.Controllers;

[Authorize(Roles = "Patient, Receptionist")]
[ApiController]
[Route("api/profiles/patient")]
public class PatientController(IMediator _mediator, ILogger<PatientController> _logger) : BaseProfilesController
{
    [HttpGet("me")]
    [EnableRateLimiting("ReadPolicy")]
    public async Task<ActionResult> GetPatientProfile()
    {
        _logger.LogInformation("Fetching patient profile for account ID: {AccountId}", CurrentAccountId);

        var result = await _mediator.Send(new GetPatientByIdQuery(CurrentAccountId));

        return Ok(result);
    }

    [HttpPatch("update")]
    [EnableRateLimiting("WritePolicy")]
    public async Task<ActionResult> UpdatePatientProfile([FromBody] UpdatePatientRequest request)
    {
        _logger.LogInformation("Updating patient profile");

        await _mediator.Send(new UpdatePatientProfileCommand(CurrentAccountId, request.FirstName, request.LastName, request.MiddleName,
            request.DateOfBirth, request.PhotoUrl, request.Phone));

        return Ok(new { message = "Patient's profile updated successfully" });
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Profiles.API.Models.Requests;
using Profiles.Application.Features.Commands.Patient.Update;
using Profiles.Application.Features.Queries.Patient.GetPatientById;

namespace Profiles.API.Controllers;

[ApiController]
[Route("api/profiles/patient")]
public class PatientController(IMediator _mediator, ILogger<PatientController> _logger) : BaseProfilesController
{
    [Authorize(Roles = "Receptionist")]
    [HttpGet("{id:guid}")]
    [EnableRateLimiting("ReadPolicy")]
    public async Task<ActionResult> GetPatientById(Guid id)
    {
        _logger.LogInformation("Receptionist {CurrentUserId} is fetching patient profile: {Id}", CurrentAccountId, id);

        var result = await _mediator.Send(new GetPatientByIdQuery(id));

        return Ok(result);
    }

    [Authorize(Roles = "Patient")]
    [HttpGet("me")]
    [EnableRateLimiting("ReadPolicy")]
    public async Task<ActionResult> GetPatientProfile()
    {
        _logger.LogInformation("Fetching patient profile for account ID: {AccountId}", CurrentAccountId);

        var result = await _mediator.Send(new GetPatientByIdQuery(CurrentAccountId));

        return Ok(result);
    }

    [Authorize(Roles = "Patient, Receptionist")]
    [HttpPatch("update/{id:guid?}")]
    [EnableRateLimiting("WritePolicy")]
    public async Task<ActionResult> UpdatePatientProfile([FromBody] UpdatePatientRequest request, Guid? id = null)
    {
        _logger.LogInformation("Updating patient profile");

        var targetId = (id.HasValue && User.IsInRole("Receptionist")) ? id.Value : CurrentAccountId;

        await _mediator.Send(new UpdatePatientProfileCommand(targetId, request.FirstName, request.LastName, request.MiddleName,
            request.DateOfBirth, request.PhotoUrl, request.Phone));

        return Ok(new { message = "Patient's profile updated successfully" });
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Profiles.API.Models.Requests;
using Profiles.Application.Features.Commands.Patient.Update;
using Profiles.Application.Features.Queries.Receptionist.GetAllProfiles;
using Profiles.Application.Features.Queries.Receptionist.GetReceptionistById;

namespace Profiles.API.Controllers;

[ApiController]
[Authorize(Roles = "Receptionist")]
[Route("api/profiles/receptionist")]
public class ReceptionistController(IMediator _mediator, ILogger<ReceptionistController> _logger) : BaseProfilesController
{
    [HttpGet("all-users")]
    [EnableRateLimiting("ReadPolicy")]
    public async Task<ActionResult> GetAllUsers()
    {
        _logger.LogInformation("Receptionist {AccountId} is fetching the list of all users", CurrentAccountId);

        var result = await _mediator.Send(new GetAllProfilesQuery());

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [EnableRateLimiting("ReadPolicy")]
    public async Task<ActionResult> GetReceptionistById(Guid id)
    {
        _logger.LogInformation("Receptionist {CurrentUserId} is fetching receptionist profile: {Id}", CurrentAccountId, id);

        var result = await _mediator.Send(new GetReceptionistByIdQuery(id));

        return Ok(result);
    }

    [HttpGet("me")]
    [EnableRateLimiting("ReadPolicy")]
    public async Task<ActionResult> GetReceptionistProfile()
    {
        _logger.LogInformation("Fetching receptionist profile for account ID: {AccountId}", CurrentAccountId);

        var result = await _mediator.Send(new GetReceptionistByIdQuery(CurrentAccountId));

        return Ok(result);
    }

    [HttpPatch("update/{id:guid?}")]
    [EnableRateLimiting("WritePolicy")]
    public async Task<ActionResult> UpdateReceptionistProfile([FromBody] UpdateReceptionistRequest request, Guid? id = null)
    {
        _logger.LogInformation("Updating receptionist profile");

        var targetId = (id.HasValue && User.IsInRole("Receptionist")) ? id.Value : CurrentAccountId;

        await _mediator.Send(new UpdateReceptionistProfileCommand(targetId, request.FirstName, request.LastName, request.MiddleName,
            request.OfficeId, request.PhotoUrl));

        return Ok(new { message = "Receptionist's profile updated successfully" });
    }
}

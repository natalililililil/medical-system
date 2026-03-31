using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Profiles.API.Models.Requests;
using Profiles.Application.Features.Commands.Patient.Update;

namespace Profiles.API.Controllers;

[ApiController]
[Authorize(Roles = "Receptionist")]
[Route("api/profiles/receptionist")]
public class ReceptionistController(IMediator _mediator, ILogger<ReceptionistController> _logger) : BaseProfilesController
{
    [HttpPut("update")]
    [EnableRateLimiting("WritePolicy")]
    public async Task<ActionResult> UpdateReceptionistProfile([FromBody] UpdateReceptionistRequest request)
    {
        _logger.LogInformation("Updating receptionist profile");

        await _mediator.Send(new UpdateReceptionistProfileCommand(CurrentAccountId, request.FirstName, request.LastName, request.MiddleName,
            request.OfficeId, request.Photo));

        return Ok(new { message = "Receptionist's profile updated successfully" });
    }
}

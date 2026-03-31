using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Profiles.API.Models.Requests;
using Profiles.Application.Features.Commands.Doctor.Update;
using Profiles.Application.Features.DTOS;
using Profiles.Application.Features.Queries.Doctor.GetDoctorById;
using Profiles.Application.Features.Queries.Doctor.GetDoctors;
using System.Security.Claims;

namespace Profiles.API.Controllers;

[ApiController]
[Route("api/profiles/doctors")]
public class DoctorsController(IMediator _mediator, ILogger<DoctorsController> _logger) : ControllerBase
{
    [HttpGet]
    [EnableRateLimiting("ReadPolicy")]
    public async Task<ActionResult<List<DoctorDto>>> GetDoctors([FromQuery] GetDoctorsQuery query)
    {
        _logger.LogInformation("Fetching doctors list with parameters: {@Query}", query);

        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [EnableRateLimiting("ReadPolicy")]
    public async Task<ActionResult<DoctorDetailsDto>> GetDoctorById(Guid id)
    {
        _logger.LogInformation("Fetching doctor details for ID: {DoctorId}", id);

        var result = await _mediator.Send(new GetDoctorByIdQuery(id));

        return Ok(result);
    }

    [Authorize(Roles = "Doctor, Receptionist")]
    [HttpPut("update")]
    [EnableRateLimiting("WritePolicy")]
    public async Task<ActionResult> UpdateDoctorProfile([FromBody] UpdateDoctorRequest request)
    {
        _logger.LogInformation("Updating doctor profile");

        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var accountId))
        {
            _logger.LogWarning("Unauthorized update attempt: User ID not found in token");
            throw new UnauthorizedException("USER_NOT_IDENTIFIED", "Cannot find user ID in token");
        }

        await _mediator.Send(new UpdateDoctorProfileCommand(accountId, request.FirstName, request.LastName, request.MiddleName, 
            request.DateOfBirth, request.CareerStartYear, request.SpecializationId, request.OfficeId, request.PhotoUrl));

        return Ok(new { message = "Doctor's profile updated successfully" });
    }
}
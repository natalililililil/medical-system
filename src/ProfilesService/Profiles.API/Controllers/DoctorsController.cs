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
[Route("api/profiles/doctor")]
public class DoctorsController(IMediator _mediator, ILogger<DoctorsController> _logger) : BaseProfilesController
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
    [HttpGet("me")]
    [EnableRateLimiting("ReadPolicy")]
    public async Task<ActionResult<DoctorDetailsDto>> GetMyProfile()
    {
        _logger.LogInformation("Fetching doctor details for ID: {DoctorId}", CurrentAccountId);

        var result = await _mediator.Send(new GetDoctorByIdQuery(CurrentAccountId));

        return Ok(result);
    }

    [Authorize(Roles = "Doctor, Receptionist")]
    [HttpPatch("update")]
    [EnableRateLimiting("WritePolicy")]
    public async Task<ActionResult> UpdateDoctorProfile([FromBody] UpdateDoctorRequest request)
    {
        _logger.LogInformation("Updating doctor profile");

        await _mediator.Send(new UpdateDoctorProfileCommand(CurrentAccountId, request.FirstName, request.LastName, request.MiddleName, 
            request.DateOfBirth, request.CareerStartYear, request.SpecializationName, request.OfficeId, request.Status, request.PhotoUrl));

        return Ok(new { message = "Doctor's profile updated successfully" });
    }
}
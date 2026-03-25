using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Profiles.Application.Features.DTOS;
using Profiles.Application.Features.Queries.Doctor.GetDoctorById;
using Profiles.Application.Features.Queries.Doctor.GetDoctors;

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
}
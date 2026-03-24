using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Features.DTOS;
using Profiles.Application.Features.Queries.Doctors.GetDoctorById;
using Profiles.Application.Features.Queries.Doctors.GetDoctors;

namespace Profiles.API.Controllers;

[ApiController]
[Route("api/profiles/doctors")]
public class DoctorsController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<DoctorDto>>> GetDoctors([FromQuery] GetDoctorsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DoctorDetailsDto>> GetDoctorById(Guid id)
    {
        var result = await _mediator.Send(new GetDoctorByIdQuery(id));
        return Ok(result);
    }
}
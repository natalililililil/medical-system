using MedicalSystem.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Profiles.API.Controllers;

[ApiController]
[Route("api/profiles")]
public class BaseProfilesController : ControllerBase
{
    protected Guid CurrentAccountId
    {
        get
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var accountId))
            {
                throw new UnauthorizedException("USER_NOT_IDENTIFIED", "Identity not found in token");
            }

            return accountId;
        }
    }
}
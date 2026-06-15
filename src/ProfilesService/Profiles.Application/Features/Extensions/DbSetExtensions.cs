using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Profiles.Application.Features.Extensions;

public static class DbSetExtensions
{
    public static async Task<T> GetProfileOrThrowAsync<T>(this IQueryable<T> set, Guid accountId, ILogger _logger, CancellationToken ct)
        where T : class
    {
        if (accountId == Guid.Empty)
        {
            throw new UnauthorizedException("INVALID_ACCOUNT_ID", "Account ID is missing or invalid.");
        }

        var profile = await set.FirstOrDefaultAsync(p => EF.Property<Guid>(p, "AccountId") == accountId, ct);

        if (profile == null)
        {
            _logger.LogWarning("Profile not found for AccountId: {AccountId}", accountId);
            throw new NotFoundException($"PROFILE_NOT_FOUND", $"Profile not found for account {accountId}");
        }

        return profile;
    }
}
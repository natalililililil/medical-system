using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Profiles.Application.Common.Interfaces;
using Profiles.Domain.Entities;

namespace Profiles.Application.Features.Commands.Doctor.Create;

public class CreateDoctorHandler(IProfilesDbContext context) : IRequestHandler<CreateDoctorCommand, Guid>
{
    public async Task<Guid> Handle(CreateDoctorCommand request, CancellationToken ct)
    {
        var existing = await context.DoctorProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.AccountId == request.AccountId, ct);

        if (existing != null)
        {
            throw new ConflictException("DOCTOR_EXISTS", "Doctor with this account already exists");

        }

        var doctor = new DoctorProfile(
            request.AccountId, request.FirstName, request.LastName, request.MiddleName,
            request.DateOfBirth, request.CareerStartYear, request.PhotoUrl, request.SpecializationId,
            request.OfficeId);

        context.DoctorProfiles.Add(doctor);
        return doctor.AccountId;
    }
}
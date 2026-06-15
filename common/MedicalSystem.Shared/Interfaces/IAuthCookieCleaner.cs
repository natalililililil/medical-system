using Microsoft.AspNetCore.Http;

namespace MedicalSystem.Shared.Interfaces;

public interface IAuthCookieCleaner
{
    void Clear(HttpResponse response);
}
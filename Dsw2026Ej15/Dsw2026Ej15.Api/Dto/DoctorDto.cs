

namespace Dsw2026Ej15.Api.Dto;

public record DoctorDto
{
    public record Request(string Name, string LicenseNumber, Guid SpecialityId);
    public record Response(Guid Id, string Name, string LicenseNumber, bool IsActive, string SpecialityName);

}


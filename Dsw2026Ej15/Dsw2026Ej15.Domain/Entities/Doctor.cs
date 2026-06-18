using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Entities;

public class Doctor: BaseEntity
{
    public string Name { get; init; }
    public string LicenseNumber { get; init; }
    public bool IsActive { get; set; }
    public Speciality Speciality { get; init; }

    public Doctor(string name, string licenseNumber, Speciality speciality, bool isActive = true) : base()
    {
        Name = name;
        LicenseNumber = licenseNumber;
        IsActive = isActive;
        Speciality = speciality;
    }

    // Constructor overload that allows specifying the Id (useful for updates/replacements)
    public Doctor(Guid id, string name, string licenseNumber, Speciality speciality, bool isActive = true) : base(id)
    {
        Name = name;
        LicenseNumber = licenseNumber;
        IsActive = isActive;
        Speciality = speciality;
    }
}

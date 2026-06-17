using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data
{
    public interface IPersistence
    {
        Task<Speciality?> GetSpecialityByIdAsync(Guid id);
        Task AddDoctorsAsync(Doctor doctor);
        Task<IEnumerable<Doctor>> GetActiveDoctorsAsync();
        Task<Doctor?> GetActiveDoctorByIdAsync(Guid id); 
        Task<bool> DesactivateDoctorAsync(Guid id);

       


    }
}

using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data
{
    public interface IPersistence
    {
        Task<Speciality?> GetSpecialityAsyncById(Guid id);

        //Metodos
        Task AddDoctorsAsync(Doctor doctor);
        Task <IEnumerable<Doctor>> GetDoctorsAsync();
        Task<Doctor?> GetDoctorAsyncById(Guid id);
        Task<bool> DesactivateDoctorAsync(Guid id);



    }
}

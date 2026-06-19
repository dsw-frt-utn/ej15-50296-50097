using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;

namespace Dsw2026Ej15.Data
{
    public class PersistenceInMemory:IPersistence
    {
        private readonly List<Doctor> _doctors = new();
        private readonly List<Speciality> _specialities= new ();

        private readonly object _lock = new (); 

        public PersistenceInMemory()
        {
            LoadSpecialitiesAsync().GetAwaiter().GetResult(); 
        }

        // Implementacion

        public Task<Speciality?> GetSpecialityByIdAsync(Guid id)
        {
            lock (_lock)
            {
                Speciality? foundSpeciality = null;

                foreach (var speciality in _specialities)
                {
                    if (speciality.Id == id)
                    {
                        foundSpeciality = speciality;
                        break;
                    }
                }

                return Task.FromResult(foundSpeciality);
            }
        }
        public Task AddDoctorsAsync(Doctor doctor)
        {
            lock (_lock)
            {
                _doctors.Add(doctor);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Doctor>> GetActiveDoctorsAsync()
        {

            lock (_lock)
            {
                List<Doctor> doctorsActive = new List<Doctor>();
                foreach (var doctor in _doctors)
                {
                    if (doctor.IsActive)
                    {
                        doctorsActive.Add(doctor);
                    }
                }

                return Task.FromResult<IEnumerable<Doctor>>(doctorsActive);
            }

        }

        public Task <Doctor?> GetActiveDoctorByIdAsync(Guid id)
        {
            lock (_lock)
            {
                Doctor? foundDoctor = null;
                foreach(var doctor in _doctors)
                {
                    if(doctor.Id == id)
                    {
                        foundDoctor = doctor;
                        break;
                    }
                }

                return Task.FromResult(foundDoctor);
            }
        }

        public Task<bool> DesactivateDoctorAsync(Guid id)
        {
            lock (_lock)
            {
                Doctor? stateDoctor = null;
                foreach (var doctor in _doctors)
                {
                    if (doctor.Id == id && doctor.IsActive == true)
                    {
                        stateDoctor = doctor;
                        doctor.IsActive = false;
                        break;
                    }
                }

                if (stateDoctor == null)
                {
                    return Task.FromResult(false);
                }

                // successfully deactivated
                return Task.FromResult(true);
            }
        }  

        private async Task LoadSpecialitiesAsync()
        {
            try
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "Source", "specialities.json"); 
                {
                    var json = await File.ReadAllTextAsync(filePath);
                    

                    var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    

                    var loadedSpecialities = JsonSerializer.Deserialize<List<Speciality>>(json, option);

                    if (loadedSpecialities != null)
                    {
                        lock (_lock)
                        {
                            _specialities.AddRange(loadedSpecialities);
                           
                        }
                    }

                }
            }catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar las especialidades desde el archivo JSON: {ex.Message}");
            }
    }

        public Task<Doctor> UpdateDoctorsAsync(Doctor doctor)
        {
           throw new NotImplementedException();

        }
}
}

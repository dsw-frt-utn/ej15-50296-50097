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

        private readonly object _lock = new (); // este objeto de tipo object se utiliza para sincronizar
                                                // el acceso a los recursos compartidos, en este caso,
                                                // las listas de doctores y especialidades. Al usar un lock,
                                                // se asegura que solo un hilo pueda acceder a estas listas al mismo tiempo,
                                                // evitando problemas de concurrencia y garantizando la integridad de los datos.

        public PersistenceInMemory()
        {
            LoadSpecialitiesAsync().GetAwaiter().GetResult(); // este método se llama para cargar las especialidades en la lista de especialidades
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
                var filePath = Path.Combine(AppContext.BaseDirectory, "Source", "specialities.json"); // esta línea de código construye la ruta al archivo JSON que contiene las especialidades.
                                                                                                      // AppContext.BaseDirectory devuelve el directorio base de la aplicación, y luego se combinan con "Data" y "specialities.json" para obtener la ruta completa al archivo.
                if (File.Exists(filePath))
                {
                    var json = await File.ReadAllTextAsync(filePath);
                    //esta línea de código lee el contenido del archivo JSON de forma asíncrona
                    //utilizando el método File.ReadAllTextAsync. El resultado se almacena en la variable
                    //json como una cadena de texto.

                    var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    // esta línea de código crea una instancia de JsonSerializerOptions y establece
                    // la propiedad PropertyNameCaseInsensitive en true. Esto significa que al deserializar el JSON,
                    // no se tendrá en cuenta la sensibilidad a mayúsculas y minúsculas en los nombres de las propiedades.
                    // Por ejemplo, si el JSON tiene una propiedad "name" y la clase tiene una propiedad "Name",
                    // se considerarán equivalentes durante la deserialización.

                    var loadedSpecialities = JsonSerializer.Deserialize<List<Speciality>>(json, option);//option puede no estar
                                                                                                        // esta línea de código deserializa el contenido del archivo JSON en una lista de objetos Speciality utilizando
                                                                                                        // el método JsonSerializer.Deserialize. El resultado se almacena en la variable loadedSpecialities.
                                                                                                        //deserializar es el proceso de convertir una cadena JSON en un objeto o una estructura de datos en memoria.
                                                                                                        // var especialidadesCargadas= JsonSerializer.Deserializer<List<Speciality>>(json,option)

                    if (loadedSpecialities != null)
                    {
                        lock (_lock)
                        {
                            _specialities.AddRange(loadedSpecialities);
                            // esta sección de código se ejecuta dentro de un bloque lock para garantizar que solo un hilo
                            // pueda acceder a la lista de especialidades al mismo tiempo.
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

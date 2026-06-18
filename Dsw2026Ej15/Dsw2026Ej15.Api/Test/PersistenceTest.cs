using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Api.Test;

public class PersistenceTest
{
    public static async Task TestPersistenceAsync(WebApplication app)
    {
        
// ====================================================================
// 2. SECCIÓN DE PRUEBA DE PERSISTENCIA (Solo para verificar que funciona)
// ====================================================================
using (var scope = app.Services.CreateScope())
{
    // Solicitamos la instancia de persistencia que maneja la aplicación
    var persistence = scope.ServiceProvider.GetRequiredService<IPersistence>();

    Console.WriteLine("\n====== INICIANDO PRUEBAS DE PERSISTENCIA EN MEMORIA ======");

    try
    {
        // PRUEBA A: Verificar si el archivo 'specialities.json' se cargó con éxito
        // Usamos el ID real de Cardiología que está dentro de tu archivo JSON
        var cardiologiaId = Guid.Parse("8a1f3b78-3f66-4d68-8d6e-1c5b9c7a2f41"); 
        var especialidad = await persistence.GetSpecialityByIdAsync(cardiologiaId);
        //guid.Parce hace que el string que le pasamos se convierta en un Guid, que es el tipo de dato que se usa para los IDs en este proyecto.

        if (especialidad != null)
        {
            Console.WriteLine($"✅ PRUEBA JSON PASADA: Especialidad encontrada -> {especialidad.Name}: {especialidad.Description}");
        }
        else
        {
            Console.WriteLine("❌ PRUEBA JSON FALLIDA: No se encontró la especialidad. Verifica que 'specialities.json' esté en la raíz de la API y con la propiedad 'Copiar en el directorio de salida' activada.");
        }

        // PRUEBA B: Insertar un médico de prueba si la especialidad existe
        if (especialidad != null)
        {
            var medicoTest = new Doctor("Dr. René Favaloro", "MN-12345", especialidad);
            

            await persistence.AddDoctorsAsync(medicoTest);
            Console.WriteLine($"✅ PRUEBA ALMACENAMIENTO: '{medicoTest.Name}' guardado exitosamente en la lista en memoria.");

            // PRUEBA C: Listar todos los médicos activos para comprobar que se sostiene la información
            var medicosActivos = await persistence.GetActiveDoctorsAsync();
            Console.WriteLine("\n--- Médicos Activos Detectados en Memoria ---");
            foreach (var doc in medicosActivos)
            {
                Console.WriteLine($"- Nombre: {doc.Name} | Matrícula: {doc.LicenseNumber} | Especialidad: {doc.Speciality.Name}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"💥 ERROR DURANTE LAS PRUEBAS: {ex.Message}");
    }

    Console.WriteLine("====== FIN DE LAS PRUEBAS DE PERSISTENCIA ======\n");
}
// ====================================================================

    }
}

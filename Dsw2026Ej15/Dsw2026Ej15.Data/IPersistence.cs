using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data
{
    public interface IPersistence
    {
        Task<Speciality?> GetSpecialityAsync(Guid id); //esta tarea devuelve una especialidad, por eso es Speciality?,
                                                           //el ? indica que puede ser null, es decir,
                                                           //que no se encontró una especialidad con ese id.

        //Metodos
        Task AddDoctorsAsync(Doctor doctor);
        //esta tarea lo que hace es agregar un doctor a la base de datos, no devuelve nada,
        //por eso es void
        Task<IEnumerable<Doctor>> GetDoctorsAsync();
        //esta tarea devuelve una lista de doctores, por eso es IEnumerable<Doctor>,
        //el IEnumerable es una interfaz que representa una colección de objetos que se pueden enumerar, en este caso, una lista de doctores.
        Task<Doctor?> GetDoctorAsync(Guid id); //esta tarea devuelve un doctor, por eso es Doctor?,
                                                   //el ? indica que puede ser null, es decir,
                                                   //que no se encontró un doctor con ese id.
        Task<bool> DesactivateDoctorAsync(Guid id);//esta tarea devuelve un booleano,
                                                   //por eso es bool, el booleano indica si se desactivó
                                                   //el doctor o no.

       


    }
}

using Dsw2026Ej15.Api.Dto;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain.Exceptions;
using Dsw2026Ej15.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Dsw2026Ej15.Api.Controllers;

[ApiController]
[Route("api/doctors")]

public class DoctorsController : ControllerBase
{
    private IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }
   

    [HttpPost]
    public async Task<IActionResult> CreateADoctor([FromBody] DoctorDto.Request request)
    {
        if (string.IsNullOrWhiteSpace(request?.Name) || string.IsNullOrWhiteSpace(request?.LicenseNumber))
        {
            throw new ValidationException("El nombre y la matrícula del médico son obligatorios.");
        }

        var speciality = await _persistence.GetSpecialityByIdAsync(request.SpecialityId);
        if (speciality == null) 
        {
            throw new ValidationException("La especialidad indicada no existe.");
        }

        var doctor = new Doctor(request.Name, request.LicenseNumber, speciality);
        await _persistence.AddDoctorsAsync(doctor);

        var dtoResponse = new DoctorDto.Response(
            Id: (Guid)doctor.Id,
            Name: doctor.Name,
            LicenseNumber: doctor.LicenseNumber,
            IsActive: doctor.IsActive,
            SpecialityName: doctor.Speciality.Name
        );

        return Created($"/api/doctors/", dtoResponse);
    }

    [HttpGet]
    public async Task<IActionResult> GetDoctors()
    {
        var doctors = await _persistence.GetActiveDoctorsAsync();
        var dtoResponse = doctors.Select(d => new DoctorDto.Response(
            Id: (Guid)d.Id,
            Name: d.Name,
            LicenseNumber: d.LicenseNumber,
            IsActive: d.IsActive,
            SpecialityName: d.Speciality.Name
        )).ToList();
        return Ok(dtoResponse);
       

        
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDoctorsById([FromRoute] Guid id)
    {
        var doctor = await _persistence.GetActiveDoctorByIdAsync(id);
        if (doctor == null)
        {
           throw new NotFoundException("Médico no encontrado o Médico inactivo.");
        }
        var dtoResponse = new DoctorDto.Response(
            Id: (Guid)doctor.Id,
            Name: doctor.Name,
            LicenseNumber: doctor.LicenseNumber,
            IsActive: doctor.IsActive,
            SpecialityName: doctor.Speciality.Name
        );
        return Ok(dtoResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDoctor([FromRoute] Guid id)
    {
        var success = await _persistence.DesactivateDoctorAsync(id);
        if (!success)
        {
            throw new NotFoundException("Médico no encontrado o Médico inactivo.");
        } 
        return NoContent();
    }

}


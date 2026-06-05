using Core.MedicalOperational.Application.DTOs.MedicalAppointments;
using Core.MedicalOperational.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Core.MedicalOperational.Api.Controllers;

[ApiController]
[Route("api/medical-appointments")]
public class MedicalAppointmentsController : ControllerBase
{
    private readonly IMedicalAppointmentService _medicalAppointmentService;

    public MedicalAppointmentsController(IMedicalAppointmentService medicalAppointmentService)
    {
        _medicalAppointmentService = medicalAppointmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _medicalAppointmentService.GetAllAsync(cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _medicalAppointmentService.GetByIdAsync(id, cancellationToken);

        if (response is null)
        {
            return NotFound("Medical appointment not found.");
        }

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMedicalAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _medicalAppointmentService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateMedicalAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _medicalAppointmentService.UpdateAsync(id, request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _medicalAppointmentService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = "Admin,OperationalManager,Receptionist,Doctor")]
    [HttpGet("doctor/{doctorId:int}")]
    public async Task<IActionResult> GetByDoctorId(
    int doctorId,
    CancellationToken cancellationToken)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "Doctor")
        {
            var doctorIdClaim = User.FindFirst("DoctorId")?.Value;

            if (!int.TryParse(doctorIdClaim, out var authenticatedDoctorId))
            {
                return Forbid();
            }

            if (authenticatedDoctorId != doctorId)
            {
                return Forbid();
            }
        }

        var response = await _medicalAppointmentService.GetByDoctorIdAsync(
            doctorId,
            cancellationToken);

        return Ok(response);
    }
}
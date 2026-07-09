using System.Security.Claims;
using Core.MedicalOperational.Application.DTOs.ClientAppointments;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.MedicalOperational.Api.Controllers;

[ApiController]
[Route("api/client-appointments")]
[Authorize(Roles = "Admin,OperationalManager,Receptionist,Client")]
public class ClientAppointmentsController : ControllerBase
{
    private readonly IClientAppointmentService _clientAppointmentService;

    public ClientAppointmentsController(IClientAppointmentService clientAppointmentService)
    {
        _clientAppointmentService = clientAppointmentService;
    }

    [HttpPost("availability")]
    public async Task<IActionResult> GetAvailability(
        [FromBody] AppointmentAvailabilityRequest request,
        CancellationToken cancellationToken)
    {
        var ownershipResult = ValidatePatientOwnership(request.PatientId);

        if (ownershipResult is not null)
        {
            return ownershipResult;
        }

        try
        {
            var response = await _clientAppointmentService.GetAvailabilityAsync(request, cancellationToken);
            return Ok(response);
        }
        catch (DomainValidationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (BusinessRuleException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [HttpPost("schedule")]
    public async Task<IActionResult> Schedule(
        [FromBody] ScheduleClientAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var ownershipResult = ValidatePatientOwnership(request.PatientId);

        if (ownershipResult is not null)
        {
            return ownershipResult;
        }

        try
        {
            var response = await _clientAppointmentService.ScheduleAsync(request, cancellationToken);
            return CreatedAtAction(nameof(Schedule), new { id = response.AppointmentId }, response);
        }
        catch (DomainValidationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (BusinessRuleException exception)
        {
            var nextAvailableSlots = await TryGetNextAvailableSlotsAsync(request, cancellationToken);

            return Conflict(new
            {
                message = exception.Message,
                isAvailable = false,
                nextAvailableSlots
            });
        }
    }

    [HttpPatch("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(
        int id,
        [FromBody] CancelClientAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var ownershipResult = ValidatePatientOwnership(request.PatientId);

        if (ownershipResult is not null)
        {
            return ownershipResult;
        }

        try
        {
            var response = await _clientAppointmentService.CancelAsync(id, request, cancellationToken);
            return Ok(response);
        }
        catch (DomainValidationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (BusinessRuleException exception) when (exception.Message.Contains("not allowed", StringComparison.OrdinalIgnoreCase))
        {
            return Forbid();
        }
        catch (BusinessRuleException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    private IActionResult? ValidatePatientOwnership(int patientId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (!string.Equals(role, "Client", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var patientIdClaim = User.FindFirst("PatientId")?.Value;

        if (!int.TryParse(patientIdClaim, out var authenticatedPatientId))
        {
            return Forbid();
        }

        if (authenticatedPatientId != patientId)
        {
            return Forbid();
        }

        return null;
    }

    private async Task<List<AvailableSlotResponse>> TryGetNextAvailableSlotsAsync(
        ScheduleClientAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var availability = await _clientAppointmentService.GetAvailabilityAsync(
                new AppointmentAvailabilityRequest
                {
                    PatientId = request.PatientId,
                    DoctorId = request.DoctorId,
                    ProcedureId = request.ProcedureId,
                    RequestedDate = request.StartDate.Date,
                    PreferredStartTime = request.StartDate.ToString("HH:mm"),
                    PreferredEndTime = request.StartDate.AddHours(4).ToString("HH:mm")
                },
                cancellationToken);

            return availability.AvailableSlots;
        }
        catch
        {
            return [];
        }
    }
}

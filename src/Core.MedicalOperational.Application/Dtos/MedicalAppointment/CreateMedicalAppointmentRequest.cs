using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Application.DTOs.MedicalAppointments;

public class CreateMedicalAppointmentRequest
{
    public string AppointmentCode { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public int ProcedureId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string? CancellationReason { get; set; }
    public DateTime? CancelledAt { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
}

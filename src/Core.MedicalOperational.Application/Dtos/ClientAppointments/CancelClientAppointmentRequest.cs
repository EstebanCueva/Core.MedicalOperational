namespace Core.MedicalOperational.Application.DTOs.ClientAppointments;

public class CancelClientAppointmentRequest
{
    public int PatientId { get; set; }
    public string CancellationReason { get; set; } = string.Empty;
    public DateTime RequestedAt { get; set; }
}

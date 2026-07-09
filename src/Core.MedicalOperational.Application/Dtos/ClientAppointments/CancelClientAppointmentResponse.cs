namespace Core.MedicalOperational.Application.DTOs.ClientAppointments;

public class CancelClientAppointmentResponse
{
    public int AppointmentId { get; set; }
    public string PreviousStatus { get; set; } = string.Empty;
    public string CurrentStatus { get; set; } = string.Empty;
    public DateTime CancelledAt { get; set; }
    public string Message { get; set; } = string.Empty;
}

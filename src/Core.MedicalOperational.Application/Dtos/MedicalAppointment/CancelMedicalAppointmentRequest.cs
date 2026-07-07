namespace Core.MedicalOperational.Application.Dtos.MedicalAppointment;

public class CancelMedicalAppointmentRequest
{
    public string? Reason { get; set; }
    public DateTime? SearchFrom { get; set; }
}
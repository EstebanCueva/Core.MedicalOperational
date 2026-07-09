namespace Core.MedicalOperational.Application.DTOs.ClientAppointments;

public class AvailableSlotResponse
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}

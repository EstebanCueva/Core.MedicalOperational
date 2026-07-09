namespace Core.MedicalOperational.Application.DTOs.ClientAppointments;

public class AppointmentAvailabilityResponse
{
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public int ProcedureId { get; set; }
    public string ProcedureName { get; set; } = string.Empty;
    public DateTime RequestedDate { get; set; }
    public List<AvailableSlotResponse> AvailableSlots { get; set; } = [];
    public string Message { get; set; } = string.Empty;
}

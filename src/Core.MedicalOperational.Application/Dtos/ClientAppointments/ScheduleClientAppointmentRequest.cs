namespace Core.MedicalOperational.Application.DTOs.ClientAppointments;

public class ScheduleClientAppointmentRequest
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int ProcedureId { get; set; }
    public int RoomId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Notes { get; set; } = string.Empty;
}

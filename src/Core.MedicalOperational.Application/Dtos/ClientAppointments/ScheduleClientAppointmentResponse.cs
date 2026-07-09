namespace Core.MedicalOperational.Application.DTOs.ClientAppointments;

public class ScheduleClientAppointmentResponse
{
    public int AppointmentId { get; set; }
    public string AppointmentCode { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int RoomId { get; set; }
    public int ProcedureId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

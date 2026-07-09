namespace Core.MedicalOperational.Application.DTOs.ClientAppointments;

public class AppointmentAvailabilityRequest
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int ProcedureId { get; set; }
    public DateTime RequestedDate { get; set; }
    public string PreferredStartTime { get; set; } = string.Empty;
    public string PreferredEndTime { get; set; } = string.Empty;
}

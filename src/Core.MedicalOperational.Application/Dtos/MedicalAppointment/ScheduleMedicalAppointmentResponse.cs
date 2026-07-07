using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Application.Dtos.MedicalAppointment;

public class ScheduleMedicalAppointmentResponse
{
    public int AppointmentId { get; set; }
    public int AssignmentId { get; set; }
    public int DoctorId { get; set; }
    public int RoomId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
    public AssignmentStatus AssignmentStatus { get; set; }
    public string Message { get; set; } = string.Empty;
}
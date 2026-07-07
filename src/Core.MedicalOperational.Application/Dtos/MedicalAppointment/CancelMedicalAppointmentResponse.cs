using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Application.DTOs.MedicalAppointments;

public class CancelMedicalAppointmentResponse
{
    public int AppointmentId { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
    public int CancelledAssignments { get; set; }
    public int? SuggestedDoctorId { get; set; }
    public int? SuggestedRoomId { get; set; }
    public DateTime? RecommendedRescheduleStartDate { get; set; }
    public DateTime? RecommendedRescheduleEndDate { get; set; }
    public string Message { get; set; } = string.Empty;
    public string RescheduleExplanation { get; set; } = string.Empty;
}
using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Domain.Entities;

public class ExistingAssignment
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int RoomId { get; set; }
    public int AppointmentId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AssignmentStatus Status { get; set; } = AssignmentStatus.Active;
}
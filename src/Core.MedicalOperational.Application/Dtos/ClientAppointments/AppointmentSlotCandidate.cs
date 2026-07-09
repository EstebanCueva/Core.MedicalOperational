using Core.MedicalOperational.Domain.Entities;

namespace Core.MedicalOperational.Application.DTOs.ClientAppointments;

public class AppointmentSlotCandidate
{
    public int PatientId { get; init; }
    public Doctor Doctor { get; init; } = null!;
    public MedicalProcedure Procedure { get; init; } = null!;
    public MedicalRoom Room { get; init; } = null!;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public IReadOnlyCollection<ExistingAssignment> ExistingAssignments { get; init; } = [];
}

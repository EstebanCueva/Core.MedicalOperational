using Core.MedicalOperational.Domain.Entities;

namespace Core.MedicalOperational.Application.DTOs.ClientAppointments;

public class ClientAppointmentContext
{
    public Patient Patient { get; init; } = null!;
    public Doctor Doctor { get; init; } = null!;
    public MedicalProcedure Procedure { get; init; } = null!;
    public IReadOnlyCollection<MedicalRoom> Rooms { get; init; } = [];
    public IReadOnlyCollection<ExistingAssignment> ExistingAssignments { get; init; } = [];
}

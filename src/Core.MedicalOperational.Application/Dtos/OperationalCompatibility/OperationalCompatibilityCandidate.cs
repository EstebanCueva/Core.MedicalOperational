using Core.MedicalOperational.Domain.Entities;

namespace Core.MedicalOperational.Application.DTOs.OperationalCompatibility;

public class OperationalCompatibilityCandidate
{
    public OperationalCompatibilityAnalysisContext Context { get; init; } = null!;
    public MedicalAppointment Appointment { get; init; } = null!;
    public Patient Patient { get; init; } = null!;
    public MedicalProcedure Procedure { get; init; } = null!;
    public Doctor Doctor { get; init; } = null!;
    public MedicalRoom Room { get; init; } = null!;
}

using Core.MedicalOperational.Application.DTOs.OperationalCompatibility;
using Core.MedicalOperational.Application.Interfaces.Services;

namespace Core.MedicalOperational.Application.Services.Rules;

public class DoctorSpecialtyCompatibilityRule : IOperationalCompatibilityRule
{
    public Task ApplyAsync(
        OperationalCompatibilityCandidate candidate,
        OperationalCompatibilityEvaluation evaluation,
        CancellationToken cancellationToken = default)
    {
        foreach (var doctorSpecialty in candidate.Context.DoctorSpecialties)
        {
            if (doctorSpecialty.DoctorId == candidate.Doctor.Id &&
                doctorSpecialty.SpecialtyId == candidate.Procedure.RequiredSpecialtyId)
            {
                return Task.CompletedTask;
            }
        }

        AddUnique(evaluation.Conflicts, "Doctor does not have the required specialty for the procedure.");
        return Task.CompletedTask;
    }

    private static void AddUnique(List<string> values, string value)
    {
        if (!values.Contains(value))
        {
            values.Add(value);
        }
    }
}

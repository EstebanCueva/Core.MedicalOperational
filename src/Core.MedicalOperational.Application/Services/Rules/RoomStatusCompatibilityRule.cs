using Core.MedicalOperational.Application.DTOs.OperationalCompatibility;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Application.Services.Rules;

public class RoomStatusCompatibilityRule : IOperationalCompatibilityRule
{
    public Task ApplyAsync(
        OperationalCompatibilityCandidate candidate,
        OperationalCompatibilityEvaluation evaluation,
        CancellationToken cancellationToken = default)
    {
        if (candidate.Room.Status != RoomStatus.Available)
        {
            var message = $"Room is not available. Current status: {candidate.Room.Status}.";

            if (!evaluation.Conflicts.Contains(message))
            {
                evaluation.Conflicts.Add(message);
            }
        }

        return Task.CompletedTask;
    }
}

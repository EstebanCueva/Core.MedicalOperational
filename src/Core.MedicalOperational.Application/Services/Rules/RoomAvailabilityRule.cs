using Core.MedicalOperational.Application.DTOs.ClientAppointments;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Application.Services.Rules;

public class RoomAvailabilityRule : IAppointmentAvailabilityRule
{
    public Task<bool> IsSatisfiedAsync(
        AppointmentSlotCandidate candidate,
        CancellationToken cancellationToken = default)
    {
        if (candidate.Room.Status != RoomStatus.Available)
        {
            return Task.FromResult(false);
        }

        foreach (var assignment in candidate.ExistingAssignments)
        {
            if (assignment.RoomId != candidate.Room.Id || assignment.Status != AssignmentStatus.Active)
            {
                continue;
            }

            if (HasOverlap(candidate.StartDate, candidate.EndDate, assignment.StartDate, assignment.EndDate))
            {
                return Task.FromResult(false);
            }
        }

        return Task.FromResult(true);
    }

    private static bool HasOverlap(DateTime firstStart, DateTime firstEnd, DateTime secondStart, DateTime secondEnd)
    {
        return firstStart < secondEnd && secondStart < firstEnd;
    }
}

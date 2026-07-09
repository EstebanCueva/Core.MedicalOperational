using Core.MedicalOperational.Application.DTOs.OperationalCompatibility;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Application.Services.Rules;

public class ScheduleConflictCompatibilityRule : IOperationalCompatibilityRule
{
    public Task ApplyAsync(
        OperationalCompatibilityCandidate candidate,
        OperationalCompatibilityEvaluation evaluation,
        CancellationToken cancellationToken = default)
    {
        foreach (var existingAssignment in candidate.Context.ExistingAssignments)
        {
            if (existingAssignment.Status != AssignmentStatus.Active ||
                existingAssignment.AppointmentId == candidate.Appointment.Id)
            {
                continue;
            }

            if (!HasDateOverlap(
                    candidate.Appointment.StartDate,
                    candidate.Appointment.EndDate,
                    existingAssignment.StartDate,
                    existingAssignment.EndDate))
            {
                continue;
            }

            if (existingAssignment.DoctorId == candidate.Doctor.Id)
            {
                AddUnique(evaluation.Conflicts, "Doctor has another assignment during the requested time.");
            }

            if (existingAssignment.RoomId == candidate.Room.Id)
            {
                AddUnique(evaluation.Conflicts, "Room has another assignment during the requested time.");
            }
        }

        return Task.CompletedTask;
    }

    private static bool HasDateOverlap(DateTime firstStart, DateTime firstEnd, DateTime secondStart, DateTime secondEnd)
    {
        return firstStart < secondEnd && secondStart < firstEnd;
    }

    private static void AddUnique(List<string> values, string value)
    {
        if (!values.Contains(value))
        {
            values.Add(value);
        }
    }
}

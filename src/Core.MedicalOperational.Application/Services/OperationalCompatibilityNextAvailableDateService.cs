using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Application.Services;

public class OperationalCompatibilityNextAvailableDateService : IOperationalCompatibilityNextAvailableDateService
{
    public DateTime? Calculate(
        MedicalAppointment appointment,
        Doctor doctor,
        MedicalRoom room,
        IReadOnlyCollection<ExistingAssignment> existingAssignments,
        IReadOnlyCollection<string> missingEquipment,
        IReadOnlyCollection<string> missingSupplies,
        IReadOnlyCollection<string> conflicts)
    {
        if (missingEquipment.Count > 0 || missingSupplies.Count > 0)
        {
            return null;
        }

        var hasScheduleConflict = false;
        var latestConflictEndDate = appointment.EndDate;

        foreach (var conflict in conflicts)
        {
            if (conflict == "Doctor has another assignment during the requested time." ||
                conflict == "Room has another assignment during the requested time.")
            {
                hasScheduleConflict = true;
                break;
            }
        }

        if (!hasScheduleConflict)
        {
            return null;
        }

        foreach (var existingAssignment in existingAssignments)
        {
            if (existingAssignment.Status != AssignmentStatus.Active ||
                existingAssignment.AppointmentId == appointment.Id)
            {
                continue;
            }

            var isSameDoctor = existingAssignment.DoctorId == doctor.Id;
            var isSameRoom = existingAssignment.RoomId == room.Id;

            if (!isSameDoctor && !isSameRoom)
            {
                continue;
            }

            if (!HasDateOverlap(appointment.StartDate, appointment.EndDate, existingAssignment.StartDate, existingAssignment.EndDate))
            {
                continue;
            }

            if (existingAssignment.EndDate > latestConflictEndDate)
            {
                latestConflictEndDate = existingAssignment.EndDate;
            }
        }

        return latestConflictEndDate.AddMinutes(15);
    }

    private static bool HasDateOverlap(DateTime firstStart, DateTime firstEnd, DateTime secondStart, DateTime secondEnd)
    {
        return firstStart < secondEnd && secondStart < firstEnd;
    }
}

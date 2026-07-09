using Core.MedicalOperational.Domain.Entities;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IOperationalCompatibilityNextAvailableDateService
{
    DateTime? Calculate(
        MedicalAppointment appointment,
        Doctor doctor,
        MedicalRoom room,
        IReadOnlyCollection<ExistingAssignment> existingAssignments,
        IReadOnlyCollection<string> missingEquipment,
        IReadOnlyCollection<string> missingSupplies,
        IReadOnlyCollection<string> conflicts);
}

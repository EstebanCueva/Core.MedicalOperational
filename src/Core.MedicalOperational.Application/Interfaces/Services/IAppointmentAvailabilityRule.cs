using Core.MedicalOperational.Application.DTOs.ClientAppointments;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IAppointmentAvailabilityRule
{
    Task<bool> IsSatisfiedAsync(
        AppointmentSlotCandidate candidate,
        CancellationToken cancellationToken = default);
}

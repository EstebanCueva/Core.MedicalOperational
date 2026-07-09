using Core.MedicalOperational.Application.DTOs.ClientAppointments;
using Core.MedicalOperational.Domain.Entities;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IClientAppointmentDomainService
{
    Task<ClientAppointmentContext> BuildContextAsync(
        int patientId,
        int doctorId,
        int procedureId,
        CancellationToken cancellationToken = default);

    Task<MedicalRoom> GetAvailableRoomAsync(int roomId, CancellationToken cancellationToken = default);

    Task<MedicalAppointment> GetOwnedAppointmentAsync(
        int appointmentId,
        int patientId,
        CancellationToken cancellationToken = default);

    TimeSpan ParseTime(string value, string fieldName);
    void ValidateAvailabilityRequest(AppointmentAvailabilityRequest request);
    void ValidateScheduleRequest(ScheduleClientAppointmentRequest request);
    void ValidateCancellationRequest(int appointmentId, CancelClientAppointmentRequest request);
    void EnsureSlotDurationMatchesProcedure(DateTime startDate, DateTime endDate, int estimatedDurationMinutes);
    string GenerateAppointmentCode(DateTime startDate);
}

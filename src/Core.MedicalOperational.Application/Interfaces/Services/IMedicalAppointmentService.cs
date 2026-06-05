using Core.MedicalOperational.Application.DTOs.MedicalAppointments;

namespace Core.MedicalOperational.Application.Interfaces.Services;

public interface IMedicalAppointmentService
{
    Task<IReadOnlyCollection<MedicalAppointmentResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MedicalAppointmentResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MedicalAppointmentResponse> CreateAsync(CreateMedicalAppointmentRequest request, CancellationToken cancellationToken = default);
    Task<MedicalAppointmentResponse> UpdateAsync(int id, UpdateMedicalAppointmentRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<MedicalAppointmentResponse>> GetByDoctorIdAsync(int doctorId,CancellationToken cancellationToken = default);
}

using Core.MedicalOperational.Application.DTOs.ClientAppointments;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;

namespace Core.MedicalOperational.Application.Services.Rules;

public class DoctorSpecialtyAvailabilityRule : IAppointmentAvailabilityRule
{
    private readonly IDoctorSpecialtyRepository _doctorSpecialtyRepository;

    public DoctorSpecialtyAvailabilityRule(IDoctorSpecialtyRepository doctorSpecialtyRepository)
    {
        _doctorSpecialtyRepository = doctorSpecialtyRepository;
    }

    public async Task<bool> IsSatisfiedAsync(
        AppointmentSlotCandidate candidate,
        CancellationToken cancellationToken = default)
    {
        var doctorSpecialties = await _doctorSpecialtyRepository.GetAllAsync(cancellationToken);

        foreach (var doctorSpecialty in doctorSpecialties)
        {
            if (doctorSpecialty.DoctorId == candidate.Doctor.Id &&
                doctorSpecialty.SpecialtyId == candidate.Procedure.RequiredSpecialtyId)
            {
                return true;
            }
        }

        return false;
    }
}

using Core.MedicalOperational.Application.DTOs.Doctors;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Domain.Exceptions;

namespace Core.MedicalOperational.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DoctorService(IDoctorRepository doctorRepository, IUnitOfWork unitOfWork)
    {
        _doctorRepository = doctorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyCollection<DoctorResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var doctors = await _doctorRepository.GetAllAsync(cancellationToken);
        var response = new List<DoctorResponse>();

        foreach (var doctor in doctors)
        {
            response.Add(MapToResponse(doctor));
        }

        return response;
    }

    public async Task<DoctorResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id, cancellationToken);
        return doctor is null ? null : MapToResponse(doctor);
    }

    public async Task<DoctorResponse> CreateAsync(
        CreateDoctorRequest request,
        CancellationToken cancellationToken = default)
    {
        var entity = new Doctor
        {
            DoctorCode = request.DoctorCode,
            FullName = request.FullName,
            LicenseNumber = request.LicenseNumber,
            IsActive = true
        };

        var created = await _doctorRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToResponse(created);
    }

    public async Task<DoctorResponse> UpdateAsync(int id, UpdateDoctorRequest request, CancellationToken cancellationToken = default)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id, cancellationToken);

        if (doctor is null)
        {
            throw new DomainValidationException("Doctor not found.");
        }

        doctor.DoctorCode = request.DoctorCode;
        doctor.FullName = request.FullName;
        doctor.LicenseNumber = request.LicenseNumber;
        doctor.IsActive = request.IsActive;

        await _doctorRepository.UpdateAsync(doctor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToResponse(doctor);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _doctorRepository.DeleteAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static DoctorResponse MapToResponse(Doctor doctor)
    {
        return new DoctorResponse
        {
            Id = doctor.Id,
            DoctorCode = doctor.DoctorCode,
            FullName = doctor.FullName,
            LicenseNumber = doctor.LicenseNumber,
            IsActive = doctor.IsActive
        };
    }
}

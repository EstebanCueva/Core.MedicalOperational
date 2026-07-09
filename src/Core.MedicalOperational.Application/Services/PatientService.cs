using Core.MedicalOperational.Application.DTOs.Patients;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Domain.Exceptions;

namespace Core.MedicalOperational.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IPatientRepository patientRepository, IUnitOfWork unitOfWork)
    {
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyCollection<PatientResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var patients = await _patientRepository.GetAllAsync(cancellationToken);
        var response = new List<PatientResponse>();

        foreach (var patient in patients)
        {
            response.Add(MapToResponse(patient));
        }

        return response;
    }

    public async Task<PatientResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var patient = await _patientRepository.GetByIdAsync(id, cancellationToken);
        return patient is null ? null : MapToResponse(patient);
    }

    public async Task<PatientResponse> CreateAsync(
        CreatePatientRequest request,
        CancellationToken cancellationToken = default)
    {
        var entity = new Patient
        {
            PatientCode = request.PatientCode,
            FullName = request.FullName,
            DocumentNumber = request.DocumentNumber,
            BirthDate = request.BirthDate,
            IsActive = true
        };

        var created = await _patientRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToResponse(created);
    }

    public async Task<PatientResponse> UpdateAsync(int id, UpdatePatientRequest request, CancellationToken cancellationToken = default)
    {
        var patient = await _patientRepository.GetByIdAsync(id, cancellationToken);

        if (patient is null)
        {
            throw new DomainValidationException("Patient not found.");
        }

        patient.PatientCode = request.PatientCode;
        patient.FullName = request.FullName;
        patient.DocumentNumber = request.DocumentNumber;
        patient.BirthDate = request.BirthDate;
        patient.IsActive = request.IsActive;

        await _patientRepository.UpdateAsync(patient, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToResponse(patient);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _patientRepository.DeleteAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static PatientResponse MapToResponse(Patient patient)
    {
        return new PatientResponse
        {
            Id = patient.Id,
            PatientCode = patient.PatientCode,
            FullName = patient.FullName,
            DocumentNumber = patient.DocumentNumber,
            BirthDate = patient.BirthDate,
            IsActive = patient.IsActive
        };
    }
}

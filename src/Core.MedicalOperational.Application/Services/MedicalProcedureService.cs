using Core.MedicalOperational.Application.DTOs.MedicalProcedures;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Domain.Entities;
using Core.MedicalOperational.Domain.Exceptions;

namespace Core.MedicalOperational.Application.Services;

public class MedicalProcedureService : IMedicalProcedureService
{
    private readonly IMedicalProcedureRepository _medicalProcedureRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MedicalProcedureService(IMedicalProcedureRepository medicalProcedureRepository, IUnitOfWork unitOfWork)
    {
        _medicalProcedureRepository = medicalProcedureRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyCollection<MedicalProcedureResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var procedures = await _medicalProcedureRepository.GetAllAsync(cancellationToken);
        var response = new List<MedicalProcedureResponse>();

        foreach (var procedure in procedures)
        {
            response.Add(MapToResponse(procedure));
        }

        return response;
    }

    public async Task<MedicalProcedureResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var procedure = await _medicalProcedureRepository.GetByIdAsync(id, cancellationToken);
        return procedure is null ? null : MapToResponse(procedure);
    }

    public async Task<MedicalProcedureResponse> CreateAsync(
        CreateMedicalProcedureRequest request,
        CancellationToken cancellationToken = default)
    {
        var entity = new MedicalProcedure
        {
            ProcedureCode = request.ProcedureCode,
            Name = request.Name,
            RequiredSpecialtyId = request.RequiredSpecialtyId,
            EstimatedDurationMinutes = request.EstimatedDurationMinutes,
            IsActive = true
        };

        var created = await _medicalProcedureRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToResponse(created);
    }

    public async Task<MedicalProcedureResponse> UpdateAsync(int id, UpdateMedicalProcedureRequest request, CancellationToken cancellationToken = default)
    {
        var procedure = await _medicalProcedureRepository.GetByIdAsync(id, cancellationToken);

        if (procedure is null)
        {
            throw new DomainValidationException("Medical procedure not found.");
        }

        procedure.ProcedureCode = request.ProcedureCode;
        procedure.Name = request.Name;
        procedure.RequiredSpecialtyId = request.RequiredSpecialtyId;
        procedure.EstimatedDurationMinutes = request.EstimatedDurationMinutes;
        procedure.IsActive = request.IsActive;

        await _medicalProcedureRepository.UpdateAsync(procedure, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapToResponse(procedure);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _medicalProcedureRepository.DeleteAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static MedicalProcedureResponse MapToResponse(MedicalProcedure procedure)
    {
        return new MedicalProcedureResponse
        {
            Id = procedure.Id,
            ProcedureCode = procedure.ProcedureCode,
            Name = procedure.Name,
            RequiredSpecialtyId = procedure.RequiredSpecialtyId,
            EstimatedDurationMinutes = procedure.EstimatedDurationMinutes,
            IsActive = procedure.IsActive
        };
    }
}

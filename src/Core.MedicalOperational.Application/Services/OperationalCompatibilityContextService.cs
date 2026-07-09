using Core.MedicalOperational.Application.DTOs.OperationalCompatibility;
using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Application.Interfaces.Services;

namespace Core.MedicalOperational.Application.Services;

public class OperationalCompatibilityContextService : IOperationalCompatibilityContextService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ISpecialtyRepository _specialtyRepository;
    private readonly IDoctorSpecialtyRepository _doctorSpecialtyRepository;
    private readonly IMedicalProcedureRepository _medicalProcedureRepository;
    private readonly IMedicalRoomRepository _medicalRoomRepository;
    private readonly IMedicalEquipmentRepository _medicalEquipmentRepository;
    private readonly IRoomEquipmentRepository _roomEquipmentRepository;
    private readonly IProcedureRequiredEquipmentRepository _procedureRequiredEquipmentRepository;
    private readonly IMedicalSupplyRepository _medicalSupplyRepository;
    private readonly ISupplyStockRepository _supplyStockRepository;
    private readonly IProcedureRequiredSupplyRepository _procedureRequiredSupplyRepository;
    private readonly IMedicalAppointmentRepository _medicalAppointmentRepository;
    private readonly IExistingAssignmentRepository _existingAssignmentRepository;

    public OperationalCompatibilityContextService(
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        ISpecialtyRepository specialtyRepository,
        IDoctorSpecialtyRepository doctorSpecialtyRepository,
        IMedicalProcedureRepository medicalProcedureRepository,
        IMedicalRoomRepository medicalRoomRepository,
        IMedicalEquipmentRepository medicalEquipmentRepository,
        IRoomEquipmentRepository roomEquipmentRepository,
        IProcedureRequiredEquipmentRepository procedureRequiredEquipmentRepository,
        IMedicalSupplyRepository medicalSupplyRepository,
        ISupplyStockRepository supplyStockRepository,
        IProcedureRequiredSupplyRepository procedureRequiredSupplyRepository,
        IMedicalAppointmentRepository medicalAppointmentRepository,
        IExistingAssignmentRepository existingAssignmentRepository)
    {
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _specialtyRepository = specialtyRepository;
        _doctorSpecialtyRepository = doctorSpecialtyRepository;
        _medicalProcedureRepository = medicalProcedureRepository;
        _medicalRoomRepository = medicalRoomRepository;
        _medicalEquipmentRepository = medicalEquipmentRepository;
        _roomEquipmentRepository = roomEquipmentRepository;
        _procedureRequiredEquipmentRepository = procedureRequiredEquipmentRepository;
        _medicalSupplyRepository = medicalSupplyRepository;
        _supplyStockRepository = supplyStockRepository;
        _procedureRequiredSupplyRepository = procedureRequiredSupplyRepository;
        _medicalAppointmentRepository = medicalAppointmentRepository;
        _existingAssignmentRepository = existingAssignmentRepository;
    }

    public async Task<OperationalCompatibilityAnalysisContext> BuildAsync(CancellationToken cancellationToken = default)
    {
        return new OperationalCompatibilityAnalysisContext
        {
            Patients = await _patientRepository.GetAllAsync(cancellationToken),
            Doctors = await _doctorRepository.GetAllAsync(cancellationToken),
            Specialties = await _specialtyRepository.GetAllAsync(cancellationToken),
            DoctorSpecialties = await _doctorSpecialtyRepository.GetAllAsync(cancellationToken),
            Procedures = await _medicalProcedureRepository.GetAllAsync(cancellationToken),
            Rooms = await _medicalRoomRepository.GetAllAsync(cancellationToken),
            Equipments = await _medicalEquipmentRepository.GetAllAsync(cancellationToken),
            RoomEquipments = await _roomEquipmentRepository.GetAllAsync(cancellationToken),
            RequiredEquipments = await _procedureRequiredEquipmentRepository.GetAllAsync(cancellationToken),
            Supplies = await _medicalSupplyRepository.GetAllAsync(cancellationToken),
            Stocks = await _supplyStockRepository.GetAllAsync(cancellationToken),
            RequiredSupplies = await _procedureRequiredSupplyRepository.GetAllAsync(cancellationToken),
            Appointments = await _medicalAppointmentRepository.GetAllAsync(cancellationToken),
            ExistingAssignments = await _existingAssignmentRepository.GetAllAsync(cancellationToken)
        };
    }
}

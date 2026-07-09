using Core.MedicalOperational.Domain.Entities;

namespace Core.MedicalOperational.Application.DTOs.OperationalCompatibility;

public class OperationalCompatibilityAnalysisContext
{
    public IReadOnlyCollection<Patient> Patients { get; init; } = [];
    public IReadOnlyCollection<Doctor> Doctors { get; init; } = [];
    public IReadOnlyCollection<Specialty> Specialties { get; init; } = [];
    public IReadOnlyCollection<DoctorSpecialty> DoctorSpecialties { get; init; } = [];
    public IReadOnlyCollection<MedicalProcedure> Procedures { get; init; } = [];
    public IReadOnlyCollection<MedicalRoom> Rooms { get; init; } = [];
    public IReadOnlyCollection<MedicalEquipment> Equipments { get; init; } = [];
    public IReadOnlyCollection<RoomEquipment> RoomEquipments { get; init; } = [];
    public IReadOnlyCollection<ProcedureRequiredEquipment> RequiredEquipments { get; init; } = [];
    public IReadOnlyCollection<MedicalSupply> Supplies { get; init; } = [];
    public IReadOnlyCollection<SupplyStock> Stocks { get; init; } = [];
    public IReadOnlyCollection<ProcedureRequiredSupply> RequiredSupplies { get; init; } = [];
    public IReadOnlyCollection<MedicalAppointment> Appointments { get; init; } = [];
    public IReadOnlyCollection<ExistingAssignment> ExistingAssignments { get; init; } = [];
}

using Core.MedicalOperational.Domain.Enums;

namespace Core.MedicalOperational.Domain.Entities;

public class MedicalAppointment
{
    public int Id { get; set; }
    public string AppointmentCode { get; set; } = string.Empty;
    public int PatientId { get; set; }
    public int ProcedureId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
}
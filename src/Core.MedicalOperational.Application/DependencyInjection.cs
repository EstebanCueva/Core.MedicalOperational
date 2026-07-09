using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Application.Services;
using Core.MedicalOperational.Application.Services.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace Core.MedicalOperational.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IOperationalCompatibilityService, OperationalCompatibilityService>();
        services.AddScoped<IOperationalCompatibilityContextService, OperationalCompatibilityContextService>();
        services.AddScoped<IOperationalCompatibilityPersistenceService, OperationalCompatibilityPersistenceService>();
        services.AddScoped<IOperationalCompatibilityNextAvailableDateService, OperationalCompatibilityNextAvailableDateService>();
        services.AddScoped<IOperationalCompatibilityRule, DoctorSpecialtyCompatibilityRule>();
        services.AddScoped<IOperationalCompatibilityRule, RoomStatusCompatibilityRule>();
        services.AddScoped<IOperationalCompatibilityRule, EquipmentCompatibilityRule>();
        services.AddScoped<IOperationalCompatibilityRule, SupplyCompatibilityRule>();
        services.AddScoped<IOperationalCompatibilityRule, ScheduleConflictCompatibilityRule>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IMedicalProcedureService, MedicalProcedureService>();
        services.AddScoped<IMedicalRoomService, MedicalRoomService>();
        services.AddScoped<IMedicalAppointmentService, MedicalAppointmentService>();
        services.AddScoped<IClientAppointmentDomainService, ClientAppointmentDomainService>();
        services.AddScoped<IAppointmentAvailabilityService, AppointmentAvailabilityService>();
        services.AddScoped<IAppointmentSchedulingService, AppointmentSchedulingService>();
        services.AddScoped<IAppointmentCancellationService, AppointmentCancellationService>();
        services.AddScoped<IAppointmentAvailabilityRule, DoctorSpecialtyAvailabilityRule>();
        services.AddScoped<IAppointmentAvailabilityRule, DoctorScheduleAvailabilityRule>();
        services.AddScoped<IAppointmentAvailabilityRule, RoomAvailabilityRule>();
        services.AddScoped<IClientAppointmentService, ClientAppointmentService>();

        return services;
    }
}

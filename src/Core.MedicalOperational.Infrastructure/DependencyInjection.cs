using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Infrastructure.Persistence;
using Core.MedicalOperational.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.MedicalOperational.Application.Interfaces.Services;
using Core.MedicalOperational.Infrastructure.Security;

namespace Core.MedicalOperational.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<MedicalOperationalDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
        services.AddScoped<IDoctorSpecialtyRepository, DoctorSpecialtyRepository>();
        services.AddScoped<IMedicalProcedureRepository, MedicalProcedureRepository>();
        services.AddScoped<IMedicalRoomRepository, MedicalRoomRepository>();
        services.AddScoped<IMedicalEquipmentRepository, MedicalEquipmentRepository>();
        services.AddScoped<IRoomEquipmentRepository, RoomEquipmentRepository>();
        services.AddScoped<IProcedureRequiredEquipmentRepository, ProcedureRequiredEquipmentRepository>();
        services.AddScoped<IMedicalSupplyRepository, MedicalSupplyRepository>();
        services.AddScoped<ISupplyStockRepository, SupplyStockRepository>();
        services.AddScoped<IProcedureRequiredSupplyRepository, ProcedureRequiredSupplyRepository>();
        services.AddScoped<IMedicalAppointmentRepository, MedicalAppointmentRepository>();
        services.AddScoped<IExistingAssignmentRepository, ExistingAssignmentRepository>();
        services.AddScoped<IOperationalCompatibilityResultRepository, OperationalCompatibilityResultRepository>();
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
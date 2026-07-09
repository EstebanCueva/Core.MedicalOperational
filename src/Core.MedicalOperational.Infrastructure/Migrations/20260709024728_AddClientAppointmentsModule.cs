using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.MedicalOperational.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClientAppointmentsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalEquipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalEquipments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalSupplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplyCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalSupplies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecialtyCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomEquipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomEquipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomEquipments_MedicalEquipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "MedicalEquipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoomEquipments_MedicalRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "MedicalRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplyStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplyId = table.Column<int>(type: "int", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    MinimumQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplyStocks_MedicalSupplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "MedicalSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorSpecialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false),
                    IsMainSpecialty = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSpecialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorSpecialties_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoctorSpecialties_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicalProcedures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcedureCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RequiredSpecialtyId = table.Column<int>(type: "int", nullable: false),
                    EstimatedDurationMinutes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalProcedures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalProcedures_Specialties_RequiredSpecialtyId",
                        column: x => x.RequiredSpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicalAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ProcedureId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CancellationReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalAppointments_MedicalProcedures_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "MedicalProcedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalAppointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureRequiredEquipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcedureId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    QuantityRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureRequiredEquipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcedureRequiredEquipments_MedicalEquipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "MedicalEquipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcedureRequiredEquipments_MedicalProcedures_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "MedicalProcedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureRequiredSupplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcedureId = table.Column<int>(type: "int", nullable: false),
                    SupplyId = table.Column<int>(type: "int", nullable: false),
                    QuantityRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedureRequiredSupplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcedureRequiredSupplies_MedicalProcedures_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "MedicalProcedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcedureRequiredSupplies_MedicalSupplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "MedicalSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExistingAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExistingAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExistingAssignments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExistingAssignments_MedicalAppointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "MedicalAppointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExistingAssignments_MedicalRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "MedicalRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OperationalCompatibilityResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnalysisDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ProcedureId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    IsCompatible = table.Column<bool>(type: "bit", nullable: false),
                    MissingEquipment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MissingSupplies = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Conflicts = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationalCompatibilityResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationalCompatibilityResults_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationalCompatibilityResults_MedicalAppointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "MedicalAppointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationalCompatibilityResults_MedicalProcedures_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "MedicalProcedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationalCompatibilityResults_MedicalRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "MedicalRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationalCompatibilityResults_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_DoctorId",
                table: "ApplicationUsers",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_Email",
                table: "ApplicationUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_PatientId",
                table: "ApplicationUsers",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_UserCode",
                table: "ApplicationUsers",
                column: "UserCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DoctorCode",
                table: "Doctors",
                column: "DoctorCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_LicenseNumber",
                table: "Doctors",
                column: "LicenseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSpecialties_DoctorId_SpecialtyId",
                table: "DoctorSpecialties",
                columns: new[] { "DoctorId", "SpecialtyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSpecialties_SpecialtyId",
                table: "DoctorSpecialties",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExistingAssignments_AppointmentId",
                table: "ExistingAssignments",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExistingAssignments_DoctorId",
                table: "ExistingAssignments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_ExistingAssignments_RoomId",
                table: "ExistingAssignments",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalAppointments_AppointmentCode",
                table: "MedicalAppointments",
                column: "AppointmentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalAppointments_PatientId",
                table: "MedicalAppointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalAppointments_ProcedureId",
                table: "MedicalAppointments",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalEquipments_EquipmentCode",
                table: "MedicalEquipments",
                column: "EquipmentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalProcedures_ProcedureCode",
                table: "MedicalProcedures",
                column: "ProcedureCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalProcedures_RequiredSpecialtyId",
                table: "MedicalProcedures",
                column: "RequiredSpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRooms_RoomCode",
                table: "MedicalRooms",
                column: "RoomCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalSupplies_SupplyCode",
                table: "MedicalSupplies",
                column: "SupplyCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationalCompatibilityResults_AppointmentId",
                table: "OperationalCompatibilityResults",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalCompatibilityResults_DoctorId",
                table: "OperationalCompatibilityResults",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalCompatibilityResults_PatientId",
                table: "OperationalCompatibilityResults",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalCompatibilityResults_ProcedureId",
                table: "OperationalCompatibilityResults",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalCompatibilityResults_RoomId",
                table: "OperationalCompatibilityResults",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_DocumentNumber",
                table: "Patients",
                column: "DocumentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientCode",
                table: "Patients",
                column: "PatientCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureRequiredEquipments_EquipmentId",
                table: "ProcedureRequiredEquipments",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureRequiredEquipments_ProcedureId",
                table: "ProcedureRequiredEquipments",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureRequiredSupplies_ProcedureId",
                table: "ProcedureRequiredSupplies",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureRequiredSupplies_SupplyId",
                table: "ProcedureRequiredSupplies",
                column: "SupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomEquipments_EquipmentId",
                table: "RoomEquipments",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomEquipments_RoomId",
                table: "RoomEquipments",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_SpecialtyCode",
                table: "Specialties",
                column: "SpecialtyCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplyStocks_SupplyId",
                table: "SupplyStocks",
                column: "SupplyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "DoctorSpecialties");

            migrationBuilder.DropTable(
                name: "ExistingAssignments");

            migrationBuilder.DropTable(
                name: "OperationalCompatibilityResults");

            migrationBuilder.DropTable(
                name: "ProcedureRequiredEquipments");

            migrationBuilder.DropTable(
                name: "ProcedureRequiredSupplies");

            migrationBuilder.DropTable(
                name: "RoomEquipments");

            migrationBuilder.DropTable(
                name: "SupplyStocks");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "MedicalAppointments");

            migrationBuilder.DropTable(
                name: "MedicalEquipments");

            migrationBuilder.DropTable(
                name: "MedicalRooms");

            migrationBuilder.DropTable(
                name: "MedicalSupplies");

            migrationBuilder.DropTable(
                name: "MedicalProcedures");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Specialties");
        }
    }
}

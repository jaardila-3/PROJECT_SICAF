using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SICAF.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aircrafts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    AircraftType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Tipo de aeronave: AVION o HELICOPTERO"),
                    Registration = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Matrícula de la aeronave sin guiones"),
                    WingType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Tipo de ala: FIXED_WING o ROTARY_WING"),
                    TotalFlightHours = table.Column<double>(type: "float", nullable: false, comment: "Horas totales de vuelo"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircrafts", x => x.Id);
                    table.CheckConstraint("CK_Aircrafts_WingType", "[WingType] IN ('FIJA', 'ROTATORIA')");
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    EntityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nombre de la entidad sobre la cual se realiza la operación"),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad referenciada"),
                    OperationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Tipo de operación realizada (Create, Update, Delete, Read)"),
                    UserRole = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Rol del usuario al momento de realizar la operación"),
                    OldValues = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true, comment: "Valores anteriores de la entidad en formato JSON"),
                    NewValues = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true, comment: "Nuevos valores de la entidad en formato JSON"),
                    AffectedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "ID del usuario afectado por la operación"),
                    AffectedUserIdentificationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Identificación y nombre del usuario afectado por la operación"),
                    LoggedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "ID del usuario"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Nombre de usuario"),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true, comment: "Dirección IP desde donde se realiza la operación"),
                    Module = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Módulo del sistema donde ocurre la operación"),
                    Controller = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Controlador donde ocurre la operación"),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Acción ejecutada en el controlador"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha y hora de creación del registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    CourseName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Nombre del curso"),
                    CourseNumber = table.Column<int>(type: "int", maxLength: 3, nullable: false, comment: "Número del curso (máximo 3 dígitos)"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Descripción del curso"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha de inicio del curso"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha de finalización del curso"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.CheckConstraint("CK_Courses_CourseNumber", "CourseNumber >= 1 AND CourseNumber <= 999");
                });

            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false, comment: "Mensaje descriptivo del error ocurrido"),
                    ExceptionType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Tipo de excepción que se produjo"),
                    StackTrace = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true, comment: "Stack trace completo del error para debugging"),
                    InnerException = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Detalles de la excepción interna si existe"),
                    HttpMethod = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, comment: "Método HTTP de la solicitud que causó el error"),
                    Url = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "URL completa de la solicitud que generó el error"),
                    StatusCode = table.Column<int>(type: "int", nullable: false, comment: "Código de estado HTTP asociado al error"),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Error", comment: "Nivel de severidad del error"),
                    MachineName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Nombre de la máquina donde ocurrió el error"),
                    Environment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Ambiente donde ocurrió el error (Development, Staging, Production)"),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si el error ha sido resuelto"),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Notas sobre cómo se resolvió el error"),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Fecha y hora cuando se resolvió el error"),
                    ResolvedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Usuario que marcó el error como resuelto"),
                    LoggedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "ID del usuario"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Nombre de usuario"),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true, comment: "Dirección IP desde donde se realiza la operación"),
                    Module = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Módulo del sistema donde ocurre la operación"),
                    Controller = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Controlador donde ocurre la operación"),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Acción ejecutada en el controlador"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha y hora de creación del registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterCatalogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    CatalogType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "Código del catalogo"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nombre del catalogo"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Descripción del catalogo"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Orden de visulización"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha de creación del registro"),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Usuario que creo el registro"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterCatalogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Phases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nombre de la fase"),
                    PhaseNumber = table.Column<int>(type: "int", nullable: false, comment: "Número de fase (secuencial)"),
                    TotalMissions = table.Column<int>(type: "int", nullable: false, comment: "Total de misiones de la fase"),
                    WingType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Tipo de ala: FIJA o ROTATORIA"),
                    PrerequisitePhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "ID de la fase prerrequisito"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phases_PrerequisitePhaseId",
                        column: x => x.PrerequisitePhaseId,
                        principalTable: "Phases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Nombre del rol"),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "Descripción del rol"),
                    IsSystemRole = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si el rol es un rol del sistema"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    Code = table.Column<int>(type: "int", nullable: false, comment: "Código único de la tarea"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false, comment: "Nombre descriptivo de la tarea"),
                    WingType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Tipo de ala: FIJA o ROTATORIA"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    DocumentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "Tipo de documento de identificación (CC, CE, etc.)"),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Número del documento de identificación"),
                    Grade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Grado policial del usuario"),
                    SeniorityOrder = table.Column<int>(type: "int", nullable: true, comment: "Orden de antiguedad del usuario"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nombres del usuario"),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Apellidos del usuario"),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Colombia", comment: "Nacionalidad del usuario"),
                    BloodType = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, comment: "Factor RH de sangre (O+, O-, A+, A-, B+, B-, AB+, AB-)"),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false, comment: "Fecha de nacimiento del usuario"),
                    Force = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Fuerza a la que pertenece (Policía Nacional, Ejercito, etc.)"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Nombre de usuario único para login"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Correo electrónico del usuario"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "Número de teléfono del usuario"),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Hash de la contraseña del usuario"),
                    LockoutEnd = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Fecha y hora hasta cuando el usuario está bloqueado"),
                    LockoutReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Razón del bloqueo del usuario"),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Número de intentos fallidos de autenticación"),
                    IsPasswordSetByAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la contraseña del usuario fue establecida por el administrador"),
                    PasswordChangeDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Fecha y hora de cambio de contraseña del usuario"),
                    PhotoData = table.Column<byte[]>(type: "varbinary(max)", nullable: true, comment: "Fotografía del usuario en formato binario"),
                    PhotoContentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Tipo de contenido MIME de la fotografía"),
                    PhotoFileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Nombre de archivo con la extensión"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Missions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    PhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la fase"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Nombre de la misión"),
                    MissionNumber = table.Column<int>(type: "int", nullable: false, comment: "Número secuencial de la misión (1-85)"),
                    FlightHours = table.Column<double>(type: "float(10)", precision: 10, scale: 2, nullable: false, comment: "Horas de vuelo requeridas para la misión"),
                    WingType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Tipo de ala: FIJA o ROTATORIA"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Missions_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "Phases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AviationProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del usuario"),
                    PID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "PID del piloto"),
                    FlightPosition = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false, comment: "Posición de vuelo"),
                    WingType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Tipo de ala de la aeronave"),
                    TotalFlightHours = table.Column<double>(type: "float", nullable: false, comment: "Horas totales de vuelo"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AviationProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AviationProfiles_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NonEvaluableMissionRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del usuario con rol de estudiante"),
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del usuario con rol de instructor"),
                    PhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la fase"),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del curso"),
                    AircraftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la aeronave utilizada en la misión"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha de realización de la misión no evaluable"),
                    NonEvaluableMissionNumber = table.Column<int>(type: "int", nullable: false, comment: "Número de misión no evaluable (1, 2, 3, etc. para MNE1, MNE2, MNE3...)"),
                    Observations = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false, comment: "Observaciones generales de la misión no evaluable"),
                    MachineFlightHours = table.Column<double>(type: "float", nullable: false, comment: "Horas de vuelo máquina"),
                    ManFlightHours = table.Column<double>(type: "float", nullable: false, comment: "Horas de vuelo máquina"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonEvaluableMissionRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NonEvaluableMissionRecords_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NonEvaluableMissionRecords_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NonEvaluableMissionRecords_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NonEvaluableMissionRecords_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "Phases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NonEvaluableMissionRecords_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentCommitteeRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del usuario con rol de estudiante"),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del curso"),
                    PhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la fase"),
                    LeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "ID del usuario con rol de lider de vuelo"),
                    CommitteeNumber = table.Column<int>(type: "int", nullable: false, comment: "Número de veces en comite por fase"),
                    ActaNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Número de acta"),
                    Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Motivo de ir al comite"),
                    Decision = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Decisión: continuar o suspender el curso"),
                    DecisionDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Fecha de la decisión"),
                    DecisionObservations = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true, comment: "Observaciones sobre la decisión"),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la misión fue resuelta"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Fecha en que fue a comité (automático)"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCommitteeRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentCommitteeRecords_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCommitteeRecords_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCommitteeRecords_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "Phases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentCommitteeRecords_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentPhaseProgress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del usuario con rol de estudiante"),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del curso"),
                    PhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la fase"),
                    LeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "ID del usuario con participación de líder de vuelo"),
                    PreviousPhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "ID de la fase anterior"),
                    NextPhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "ID de la fase siguiente"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Fecha de inicio de la fase"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Fecha de finalización de la fase"),
                    IsCurrentPhase = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "Indica si es la fase actual del estudiante"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Estado de la fase (EN PROCESO, COMPLETADA, FALLIDA, SUSPENDIDA)"),
                    CompletedMissions = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Cantidad de misiones completadas"),
                    FailedMissions = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Cantidad de misiones fallidas"),
                    PhasePassed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si el estudiante ha pasado la fase"),
                    Observations = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Observaciones"),
                    IsSuspended = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si el estudiante está suspendido de la fase"),
                    SuspensionDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Fecha de suspensión"),
                    SuspensionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Razón de la suspensión"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPhaseProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentPhaseProgress_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentPhaseProgress_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StudentPhaseProgress_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "Phases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentPhaseProgress_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCourses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del usuario"),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del curso"),
                    AssignmentDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()", comment: "Fecha de inscripción"),
                    ParticipationType = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Tipo de participación en el curso: ESTUDIANTE o INSTRUCTOR o LIDER DE VUELO"),
                    WingType = table.Column<string>(type: "nvarchar(450)", nullable: true, comment: "Tipo de ala"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "Indica si es la inscripción activa del estudiante"),
                    UnassignmentDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Fecha de desactivación"),
                    UnassignmentReason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Razón de la desactivación"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCourses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del usuario"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del rol"),
                    ExpiresAt = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Fecha de expiración del rol (para roles temporales)"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MissionTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    MissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la misión"),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la tarea"),
                    IsP3Task = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si esta tarea es P3 (crítica) en esta misión"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Orden de visualización"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MissionTasks_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Missions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MissionTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentMissionProgress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del usuario con rol de estudiante"),
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del usuario con rol de instructor"),
                    MissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la misión"),
                    PhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la fase"),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del curso"),
                    AircraftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la aeronave utilizada en la misión"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha de realización de la misión"),
                    MissionPassed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la misión fue aprobada"),
                    CriticalFailures = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Cantidad de fallas críticas (N-Rojas)"),
                    Observations = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Observaciones adicionales"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentMissionProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentMissionProgress_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentMissionProgress_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentMissionProgress_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentMissionProgress_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Missions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentMissionProgress_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "Phases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentMissionProgress_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FlightHourLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del usuario"),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del curso"),
                    MissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "ID de la misión (nullable para misiones no evaluables)"),
                    NonEvaluableMissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "ID de la misión no evaluable (nullable para misiones evaluables)"),
                    AircraftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la aeronave utilizada"),
                    MachineFlightHours = table.Column<double>(type: "float(10)", precision: 10, scale: 2, nullable: false, comment: "Horas de vuelo registrada en la aeronave"),
                    ManFlightHours = table.Column<double>(type: "float(10)", precision: 10, scale: 2, nullable: false, comment: "Horas de vuelo registrada para el hombre (MachineFlightHours x 1.3)"),
                    SilaboFlightHours = table.Column<double>(type: "float(10)", precision: 10, scale: 2, nullable: false, comment: "Horas de vuelo en semana"),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Rol del usuario: Estudiante, Instructor"),
                    Observations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "Observaciones adicionales"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha del registro"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightHourLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightHourLogs_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightHourLogs_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightHourLogs_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Missions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightHourLogs_NonEvaluableMissionId",
                        column: x => x.NonEvaluableMissionId,
                        principalTable: "NonEvaluableMissionRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightHourLogs_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NonEvaluableTaskGrades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del usuario con rol de estudiante"),
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del usuario con rol de instructor"),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la tarea"),
                    NonEvaluableMissionRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del registro de misión no evaluable"),
                    Grade = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "Calificación: A, B, C, N, DM, SC (NO permite NR - N-Roja)"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha de la calificación"),
                    EditCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Contador de ediciones (máximo 2)"),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Fecha de la última edición"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonEvaluableTaskGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NonEvaluableTaskGrades_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NonEvaluableTaskGrades_NonEvaluableMissionRecordId",
                        column: x => x.NonEvaluableMissionRecordId,
                        principalTable: "NonEvaluableMissionRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NonEvaluableTaskGrades_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NonEvaluableTaskGrades_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentTaskGrades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del usuario con rol de estudiante"),
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del usuario con rol de Instructor"),
                    MissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la misión"),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la tarea"),
                    StudentMissionProgressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID del progreso de la misión del estudiante"),
                    Grade = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "Calificación: A, B, C, N, NR (N ROJA)"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha de la calificación"),
                    EditCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Contador de ediciones"),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Fecha de la ultima edición"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTaskGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentTaskGrades_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentTaskGrades_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Missions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentTaskGrades_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentTaskGrades_StudentMissionProgresId",
                        column: x => x.StudentMissionProgressId,
                        principalTable: "StudentMissionProgress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentTaskGrades_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NonEvaluableGradeReasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    NonEvaluableTaskGradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la calificación de tarea no evaluable"),
                    ReasonCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Categoría: Mental, Fisica, Emocional"),
                    ReasonDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "Descripción o motivo de la calificación N (No Satisfactorio)"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha del registro del motivo"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonEvaluableGradeReasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NonEvaluableGradeReasons_NonEvaluableTaskGradeId",
                        column: x => x.NonEvaluableTaskGradeId,
                        principalTable: "NonEvaluableTaskGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentGradeNRedReasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identificador único de la entidad"),
                    StudentTaskGradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "ID de la calificación del estudiante"),
                    ReasonCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Categoría: Mental, Fisica, Emocional"),
                    ReasonDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "Descripción o motivo de la N-Roja"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha del motivo de la N-Roja"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indica si la entidad ha sido eliminada lógicamente (soft delete)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGradeNRedReasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentGradeNRedReason_StudentTaskGradeId",
                        column: x => x.StudentTaskGradeId,
                        principalTable: "StudentTaskGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aircraft_IsDeleted",
                table: "Aircrafts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_AircraftType",
                table: "Aircrafts",
                column: "AircraftType");

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_Registration_WingType_Unique",
                table: "Aircrafts",
                columns: new[] { "Registration", "WingType" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_WingType",
                table: "Aircrafts",
                column: "WingType");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_CreatedAt",
                table: "AuditLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Module_Controller_Action",
                table: "AuditLogs",
                columns: new[] { "Module", "Controller", "Action" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_UserId",
                table: "AuditLogs",
                column: "LoggedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_UserId_CreatedAt",
                table: "AuditLogs",
                columns: new[] { "LoggedUserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_AffectedUserId",
                table: "AuditLogs",
                column: "AffectedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityId",
                table: "AuditLogs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityName",
                table: "AuditLogs",
                column: "EntityName");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityName_EntityId",
                table: "AuditLogs",
                columns: new[] { "EntityName", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityName_OperationType_CreatedAt",
                table: "AuditLogs",
                columns: new[] { "EntityName", "OperationType", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_OperationType",
                table: "AuditLogs",
                column: "OperationType");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId_AffectedUserId",
                table: "AuditLogs",
                columns: new[] { "LoggedUserId", "AffectedUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AviationProfile_IsDeleted",
                table: "AviationProfiles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AviationProfiles_PID_Unique",
                table: "AviationProfiles",
                column: "PID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AviationProfiles_UserId",
                table: "AviationProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AviationProfiles_WingType",
                table: "AviationProfiles",
                column: "WingType");

            migrationBuilder.CreateIndex(
                name: "IX_Course_IsDeleted",
                table: "Courses",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseName",
                table: "Courses",
                column: "CourseName");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseNumber_Unique",
                table: "Courses",
                column: "CourseNumber",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Dates",
                table: "Courses",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLog_CreatedAt",
                table: "ErrorLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLog_Module_Controller_Action",
                table: "ErrorLogs",
                columns: new[] { "Module", "Controller", "Action" });

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLog_UserId",
                table: "ErrorLogs",
                column: "LoggedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLog_UserId_CreatedAt",
                table: "ErrorLogs",
                columns: new[] { "LoggedUserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLogs_Environment",
                table: "ErrorLogs",
                column: "Environment");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLogs_ExceptionType",
                table: "ErrorLogs",
                column: "ExceptionType");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLogs_ExceptionType_CreatedAt",
                table: "ErrorLogs",
                columns: new[] { "ExceptionType", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLogs_IsResolved",
                table: "ErrorLogs",
                column: "IsResolved");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLogs_IsResolved_ResolvedAt",
                table: "ErrorLogs",
                columns: new[] { "IsResolved", "ResolvedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLogs_IsResolved_Severity",
                table: "ErrorLogs",
                columns: new[] { "IsResolved", "Severity" });

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLogs_Severity",
                table: "ErrorLogs",
                column: "Severity");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLogs_Severity_CreatedAt",
                table: "ErrorLogs",
                columns: new[] { "Severity", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLogs_StatusCode",
                table: "ErrorLogs",
                column: "StatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLogs_UserId_CreatedAt_Severity",
                table: "ErrorLogs",
                columns: new[] { "LoggedUserId", "CreatedAt", "Severity" });

            migrationBuilder.CreateIndex(
                name: "IX_FlightHourLog_IsDeleted",
                table: "FlightHourLogs",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_FlightHourLogs_AircraftId",
                table: "FlightHourLogs",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightHourLogs_CourseId",
                table: "FlightHourLogs",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightHourLogs_MissionId",
                table: "FlightHourLogs",
                column: "MissionId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightHourLogs_NonEvaluableMissionId",
                table: "FlightHourLogs",
                column: "NonEvaluableMissionId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightHourLogs_UserId",
                table: "FlightHourLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterCatalog_IsDeleted",
                table: "MasterCatalogs",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MasterCatalogs_CatalogType",
                table: "MasterCatalogs",
                column: "CatalogType");

            migrationBuilder.CreateIndex(
                name: "IX_MasterCatalogs_Code_Unique",
                table: "MasterCatalogs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MasterCatalogs_DisplayOrder",
                table: "MasterCatalogs",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_MasterCatalogs_Type_Code_Unique",
                table: "MasterCatalogs",
                columns: new[] { "CatalogType", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mission_IsDeleted",
                table: "Missions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_MissionNumber",
                table: "Missions",
                column: "MissionNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_PhaseId_MissionNumber_Unique",
                table: "Missions",
                columns: new[] { "PhaseId", "MissionNumber" },
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_WingType",
                table: "Missions",
                column: "WingType");

            migrationBuilder.CreateIndex(
                name: "IX_MissionTask_IsDeleted",
                table: "MissionTasks",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MissionTasks_IsP3Task",
                table: "MissionTasks",
                column: "IsP3Task");

            migrationBuilder.CreateIndex(
                name: "IX_MissionTasks_MissionId",
                table: "MissionTasks",
                column: "MissionId");

            migrationBuilder.CreateIndex(
                name: "IX_MissionTasks_TaskId",
                table: "MissionTasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableGradeReason_IsDeleted",
                table: "NonEvaluableGradeReasons",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableGradeReasons_NonEvaluableTaskGradeId",
                table: "NonEvaluableGradeReasons",
                column: "NonEvaluableTaskGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableGradeReasons_ReasonCategory",
                table: "NonEvaluableGradeReasons",
                column: "ReasonCategory");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableMissionRecord_IsDeleted",
                table: "NonEvaluableMissionRecords",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableMissionRecords_AircraftId",
                table: "NonEvaluableMissionRecords",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableMissionRecords_CourseId",
                table: "NonEvaluableMissionRecords",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableMissionRecords_InstructorId",
                table: "NonEvaluableMissionRecords",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableMissionRecords_PhaseId",
                table: "NonEvaluableMissionRecords",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableMissionRecords_Student_Course_Phase_Number_Unique",
                table: "NonEvaluableMissionRecords",
                columns: new[] { "StudentId", "CourseId", "NonEvaluableMissionNumber", "PhaseId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableMissionRecords_StudentId",
                table: "NonEvaluableMissionRecords",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableTaskGrade_IsDeleted",
                table: "NonEvaluableTaskGrades",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableTaskGrades_InstructorId",
                table: "NonEvaluableTaskGrades",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableTaskGrades_NonEvaluableMissionRecordId",
                table: "NonEvaluableTaskGrades",
                column: "NonEvaluableMissionRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableTaskGrades_StudentId",
                table: "NonEvaluableTaskGrades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_NonEvaluableTaskGrades_TaskId",
                table: "NonEvaluableTaskGrades",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Phase_IsDeleted",
                table: "Phases",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Phase_Name",
                table: "Phases",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Phase_PhaseNumber",
                table: "Phases",
                column: "PhaseNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Phase_PrerequisitePhaseId",
                table: "Phases",
                column: "PrerequisitePhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Phases_Number_WingType_Unique",
                table: "Phases",
                columns: new[] { "PhaseNumber", "WingType" },
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Phases_WingType",
                table: "Phases",
                column: "WingType");

            migrationBuilder.CreateIndex(
                name: "IX_Role_IsDeleted",
                table: "Roles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name_Unique",
                table: "Roles",
                column: "Name",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitteeRecord_IsDeleted",
                table: "StudentCommitteeRecords",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitteeRecords_CourseId",
                table: "StudentCommitteeRecords",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitteeRecords_IsResolved",
                table: "StudentCommitteeRecords",
                column: "IsResolved");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitteeRecords_LeaderId",
                table: "StudentCommitteeRecords",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitteeRecords_PhaseId",
                table: "StudentCommitteeRecords",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCommitteeRecords_StudentId",
                table: "StudentCommitteeRecords",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGradeNRedReason_IsDeleted",
                table: "StudentGradeNRedReasons",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGradeNRedReason_ReasonCategory",
                table: "StudentGradeNRedReasons",
                column: "ReasonCategory");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGradeNRedReason_StudentTaskGradeId",
                table: "StudentGradeNRedReasons",
                column: "StudentTaskGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMissionProgress_AircraftId",
                table: "StudentMissionProgress",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMissionProgress_CourseId",
                table: "StudentMissionProgress",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMissionProgress_InstructorId",
                table: "StudentMissionProgress",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMissionProgress_IsDeleted",
                table: "StudentMissionProgress",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMissionProgress_MissionId",
                table: "StudentMissionProgress",
                column: "MissionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMissionProgress_PhaseId",
                table: "StudentMissionProgress",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMissionProgress_StudentId",
                table: "StudentMissionProgress",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPhaseProgress_CourseId",
                table: "StudentPhaseProgress",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPhaseProgress_IsDeleted",
                table: "StudentPhaseProgress",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPhaseProgress_LeaderId",
                table: "StudentPhaseProgress",
                column: "LeaderId",
                filter: "[LeaderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPhaseProgress_PhaseId",
                table: "StudentPhaseProgress",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPhaseProgress_Status",
                table: "StudentPhaseProgress",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPhaseProgress_StudentId",
                table: "StudentPhaseProgress",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTaskGrade_IsDeleted",
                table: "StudentTaskGrades",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTaskGrades_InstructorId",
                table: "StudentTaskGrades",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTaskGrades_MissionId",
                table: "StudentTaskGrades",
                column: "MissionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTaskGrades_StudentId",
                table: "StudentTaskGrades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTaskGrades_StudentMissionProgressId",
                table: "StudentTaskGrades",
                column: "StudentMissionProgressId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTaskGrades_TaskId",
                table: "StudentTaskGrades",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Code_WingType_Unique",
                table: "Tasks",
                columns: new[] { "Code", "WingType" },
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_IsDeleted",
                table: "Tasks",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_WingType",
                table: "Tasks",
                column: "WingType");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourse_IsDeleted",
                table: "UserCourses",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_CourseId",
                table: "UserCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_IsActive",
                table: "UserCourses",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_ParticipationType",
                table: "UserCourses",
                column: "ParticipationType");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_UserId",
                table: "UserCourses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_WingType",
                table: "UserCourses",
                column: "WingType");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_IsDeleted",
                table: "UserRoles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_ExpiresAt",
                table: "UserRoles",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_User_Role_Unique",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_IsDeleted",
                table: "Users",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BirthDate",
                table: "Users",
                column: "BirthDate");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Document_Unique",
                table: "Users",
                columns: new[] { "DocumentType", "IdentificationNumber" },
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_Unique",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL AND IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FullName",
                table: "Users",
                columns: new[] { "Name", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Grade_SeniorityOrder_Unique",
                table: "Users",
                columns: new[] { "Grade", "SeniorityOrder" },
                unique: true,
                filter: "[Grade] IS NOT NULL AND [SeniorityOrder] IS NOT NULL AND IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LockoutEnd",
                table: "Users",
                column: "LockoutEnd");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username_Unique",
                table: "Users",
                column: "Username",
                unique: true,
                filter: "IsDeleted = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "AviationProfiles");

            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DropTable(
                name: "FlightHourLogs");

            migrationBuilder.DropTable(
                name: "MasterCatalogs");

            migrationBuilder.DropTable(
                name: "MissionTasks");

            migrationBuilder.DropTable(
                name: "NonEvaluableGradeReasons");

            migrationBuilder.DropTable(
                name: "StudentCommitteeRecords");

            migrationBuilder.DropTable(
                name: "StudentGradeNRedReasons");

            migrationBuilder.DropTable(
                name: "StudentPhaseProgress");

            migrationBuilder.DropTable(
                name: "UserCourses");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "NonEvaluableTaskGrades");

            migrationBuilder.DropTable(
                name: "StudentTaskGrades");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "NonEvaluableMissionRecords");

            migrationBuilder.DropTable(
                name: "StudentMissionProgress");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Aircrafts");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Missions");

            migrationBuilder.DropTable(
                name: "Phases");
        }
    }
}

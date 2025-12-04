using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SICAF.Business.Interfaces.Academic;
using SICAF.Business.Interfaces.Identity;
using SICAF.Business.Mappers.Identity;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Identity;
using SICAF.Common.Interfaces;
using SICAF.Common.Models.Results;
using SICAF.Data.Entities.Academic;
using SICAF.Data.Entities.Catalogs;
using SICAF.Data.Entities.Identity;
using SICAF.Data.Interfaces.Repositories;

namespace SICAF.Business.Services.Identity;

/// <summary>
/// Servicio de usuario
/// </summary>
/// <param name="unitOfWork"> Unidad de trabajo</param>
public class UserService(
    ILogger<UserService> logger,
    ICourseService courseService,
    IRoleService roleService,
    IPhaseService phaseService,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher
) : IUserService
{
    private readonly ILogger<UserService> _logger = logger;
    private readonly ICourseService _courseService = courseService;
    private readonly IRoleService _roleService = roleService;
    private readonly IPhaseService _phaseService = phaseService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    /// <summary>
    /// Obtiene todos los usuarios del sistema
    /// </summary>
    public async Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.Repository<User>().GetListAsync(
            predicate: null,
            orderBy: q => q.OrderBy(u => u.Name),
            includeFunc: q => q.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).Include(u => u.AviationProfile))
        ?? [];

        var usersDto = users.Select(u => u.MapToDto()).ToList();
        foreach (var item in usersDto)
        {
            var catalogForce = await _unitOfWork.Repository<MasterCatalog>().GetFirstAsync(c => c.Code == item.Force);
            item.Force = catalogForce?.Name ?? item.Force;
        }
        return Result<IEnumerable<UserDto>>.Success(usersDto);
    }

    /// <summary>
    /// Obtiene un usuario por su ID
    /// </summary>
    public async Task<Result<UserDto>> GetUserByIdAsync(Guid userId)
    {
        if (userId == Guid.Empty) return Result<UserDto>.Failure(SystemErrors.UserError.NotFound);

        var user = await _unitOfWork.Repository<User>().GetFirstAsync(
                predicate: u => u.Id == userId,
                includeFunc: q => q
                    .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                    .Include(u => u.AviationProfile)
            );

        if (user is null) return Result<UserDto>.Failure(SystemErrors.UserError.NotFound);
        var force = await _unitOfWork.Repository<MasterCatalog>().GetFirstAsync(c => c.Code == user.Force);
        user.Force = force?.Name ?? user.Force;

        var nationality = await _unitOfWork.Repository<MasterCatalog>().GetFirstAsync(c => c.Code == user.Nationality);
        user.Nationality = nationality?.Name ?? user.Nationality;

        if (!string.IsNullOrEmpty(user.AviationProfile?.FlightPosition))
        {
            var flightPosition = await _unitOfWork.Repository<MasterCatalog>().GetFirstAsync(c => c.Code == user.AviationProfile.FlightPosition);
            user.AviationProfile.FlightPosition = flightPosition?.Name ?? user.AviationProfile.FlightPosition;
        }

        return Result<UserDto>.Success(user.MapToDto());
    }

    /// <summary>
    /// Crea un nuevo usuario
    /// </summary>
    public async Task<Result<UserDto>> CreateUserAsync(RegisterDto createUserDto)
    {
        // Validar datos únicos
        var validationResult = await ValidateUniqueFieldsCreateAsync(createUserDto);
        if (!validationResult.IsSuccess)
            return Result<UserDto>.Failure(validationResult.Error);

        //validar si el rol seleccionado corresponde a un perfil de aviación
        var roles = await _roleService.GetRolesAsync(createUserDto.SelectedRoleIds);
        bool needsAviationProfile = roles.Any(r => AviationConstants.AviationRoles.Contains(r.Name));
        bool areFieldsFilled = !string.IsNullOrWhiteSpace(createUserDto.PID) && !string.IsNullOrWhiteSpace(createUserDto.FlightPosition) && !string.IsNullOrWhiteSpace(createUserDto.WingType);

        if (needsAviationProfile && !areFieldsFilled)
        {
            return Result<UserDto>.Failure(SystemErrors.ProfileError.AviationProfileRequired);
        }

        // validar rol de estudiante
        var hasStudentRole = roles.Any(r => r.Name == SystemRoles.STUDENT);
        if (hasStudentRole)
        {
            // Verificar que se haya seleccionado un programa
            if (!createUserDto.CourseId.HasValue || createUserDto.CourseId.Value == Guid.Empty)
            {
                return Result<UserDto>.Failure(SystemErrors.CourseError.Required);
            }

            // Verificar que el programa exista y no haya finalizado
            var courseResult = await _courseService.GetCourseByIdAsync(createUserDto.CourseId.Value);
            if (!courseResult.IsSuccess || courseResult.Value.EndDate <= DateTime.Now)
            {
                return Result<UserDto>.Failure(SystemErrors.CourseError.NotActive);
            }
        }

        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Crear usuario
            var user = createUserDto.MapToEntity();
            user.Username = createUserDto.Username.ToLower();
            user.IsPasswordSetByAdmin = true;
            user.PasswordHash = _passwordHasher.HashPassword(createUserDto.IdentificationNumber);
            await _unitOfWork.Repository<User>().AddAsync(user);

            // Crear perfil de aviación si es necesario
            if (needsAviationProfile)
            {
                var aviationProfile = new AviationProfile
                {
                    UserId = user.Id,
                    PID = createUserDto.PID!,
                    FlightPosition = createUserDto.FlightPosition!,
                    WingType = createUserDto.WingType!
                };

                await _unitOfWork.Repository<AviationProfile>().AddAsync(aviationProfile);
            }

            // Asignar roles
            if (createUserDto.SelectedRoleIds.Count != 0)
            {
                foreach (var roleId in createUserDto.SelectedRoleIds)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = roleId
                    };
                    await _unitOfWork.Repository<UserRole>().AddAsync(userRole);
                }
            }

            // si es estudiante, es obligatorio inscribir en el programa
            if (hasStudentRole && createUserDto.CourseId.HasValue)
            {
                var participationType = hasStudentRole ? SystemRoles.STUDENT :
                SystemRoles.INSTRUCTOR;
                var userCourse = new UserCourse
                {
                    UserId = user.Id,
                    CourseId = createUserDto.CourseId!.Value,
                    ParticipationType = SystemRoles.STUDENT,
                    WingType = createUserDto.WingType,
                    AssignmentDate = DateTime.Now,
                    IsActive = true
                };
                await _unitOfWork.Repository<UserCourse>().AddAsync(userCourse);

                // es obligatorio inscribir en la fase 1
                var resultphase2 = await _phaseService.GetPhaseByNumberAndWingTypeAsync(2, createUserDto.WingType!);
                if (!resultphase2.IsSuccess || resultphase2.Value.PrerequisitePhaseId == null)
                {
                    return Result<UserDto>.Failure(SystemErrors.PhaseError.PhaseNotFound);
                }

                var phaseProgress = new StudentPhaseProgress
                {
                    StudentId = user.Id,
                    CourseId = createUserDto.CourseId!.Value,
                    PhaseId = resultphase2.Value.PrerequisitePhaseId!.Value,
                    NextPhaseId = resultphase2.Value.Id,
                    IsCurrentPhase = true,
                    StartDate = DateTime.Now
                };
                await _unitOfWork.Repository<StudentPhaseProgress>().AddAsync(phaseProgress);
            }

            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            // Retornar usuario creado
            var createdUser = await GetUserByIdAsync(user.Id);
            return createdUser;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al crear usuario");
            throw;
        }
    }

    /// <summary>
    /// Actualiza un usuario existente
    /// </summary>
    public async Task<Result<UserDto>> UpdateUserAsync(UpdateDto updateUserDto)
    {
        // Validar datos únicos
        var validationResult = await ValidateUniqueFieldsUpdateAsync(updateUserDto);
        if (!validationResult.IsSuccess)
            return Result<UserDto>.Failure(validationResult.Error);

        //validar si el rol seleccionado corresponde a un perfil de aviación
        var roles = await _roleService.GetRolesAsync(updateUserDto.SelectedRoleIds);
        bool needsAviationProfile = roles.Any(r => AviationConstants.AviationRoles.Contains(r.Name));
        bool areFieldsFilled = !string.IsNullOrWhiteSpace(updateUserDto.PID) && !string.IsNullOrWhiteSpace(updateUserDto.FlightPosition) && !string.IsNullOrWhiteSpace(updateUserDto.WingType);
        if (needsAviationProfile && !areFieldsFilled)
        {
            return Result<UserDto>.Failure(SystemErrors.ProfileError.AviationProfileRequired);
        }

        // Si se está asignando rol de estudiante
        var hasStudentRole = roles.Any(r => r.Name == SystemRoles.STUDENT);
        var currentUserRoles = await _unitOfWork.Repository<UserRole>()
            .GetListAsync(ur => ur.UserId == updateUserDto.Id, includeFunc: q => q.Include(ur => ur.Role));
        var hadStudentRole = currentUserRoles.Any(ur => ur.Role.Name == SystemRoles.STUDENT);

        // Si se está agregando el rol de estudiante
        if (hasStudentRole && !hadStudentRole)
        {
            // Verificar si ya tiene un programa activo
            var currentCourseResult = await _courseService.GetStudentCurrentCourseAsync(updateUserDto.Id);

            // Si no tiene programa activo, debe seleccionar uno
            if (!currentCourseResult.IsSuccess)
            {
                // Verificar que se haya seleccionado un programa
                if (!updateUserDto.CourseId.HasValue || updateUserDto.CourseId.Value == Guid.Empty)
                {
                    return Result<UserDto>.Failure(SystemErrors.CourseError.Required);
                }

                // Verificar que el programa seleccionado esté activo
                var courseResult = await _courseService.GetCourseByIdAsync(updateUserDto.CourseId.Value);
                if (!courseResult.IsSuccess || courseResult.Value.EndDate < DateTime.Now)
                {
                    return Result<UserDto>.Failure(SystemErrors.CourseError.NotActive);
                }
            }
        }

        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Obtener el usuario existente
            var user = await _unitOfWork.Repository<User>().GetFirstAsync(u => u.Id == updateUserDto.Id);

            if (user is null)
                return Result<UserDto>.Failure(SystemErrors.UserError.NotFound);

            // Actualizar propiedades
            user.UpdateFromDto(updateUserDto);

            // Manejar bloqueo/desbloqueo
            if (updateUserDto.WantToLock)
            {
                // Bloquear usuario
                user.LockoutEnd = updateUserDto.LockoutEnd ?? DateTime.Now.AddYears(50);
                user.LockoutReason = updateUserDto.LockoutReason;
            }
            else
            {
                // Desbloquear usuario
                user.LockoutEnd = null;
                user.AccessFailedCount = 0;
                user.LockoutReason = null;
            }

            _unitOfWork.Repository<User>().Update(user);

            // Actualizar roles
            await UpdateUserRolesAsync(user, updateUserDto.SelectedRoleIds);

            // Actualizar perfil de aviación
            await UpdateUserAviationProfileAsync(user, updateUserDto, needsAviationProfile);

            // Actualizar WingType en UserCourse activo si existe
            if (needsAviationProfile)
            {
                await UpdateUserCourseWingTypeAsync(user.Id, updateUserDto);
            }

            // Si se está quitando el rol de estudiante, desasignar del programa
            if (hadStudentRole && !hasStudentRole)
            {
                await UnassignUserFromCourseAsync(user.Id);
            }

            // Si se está asignando rol de estudiante y necesita programa
            if (hasStudentRole && !hadStudentRole && updateUserDto.CourseId.HasValue)
            {
                var studentCourse = new UserCourse
                {
                    UserId = user.Id,
                    CourseId = updateUserDto.CourseId!.Value,
                    ParticipationType = SystemRoles.STUDENT,
                    WingType = updateUserDto.WingType,
                    AssignmentDate = DateTime.Now,
                    IsActive = true
                };
                await _unitOfWork.Repository<UserCourse>().AddAsync(studentCourse);

                // es obligatorio inscribir en la fase 1
                var resultphase2 = await _phaseService.GetPhaseByNumberAndWingTypeAsync(2, updateUserDto.WingType!);
                if (!resultphase2.IsSuccess || resultphase2.Value.PrerequisitePhaseId == null)
                {
                    return Result<UserDto>.Failure(SystemErrors.PhaseError.PhaseNotFound);
                }

                var phaseProgress = new StudentPhaseProgress
                {
                    StudentId = user.Id,
                    CourseId = updateUserDto.CourseId!.Value,
                    PhaseId = resultphase2.Value.PrerequisitePhaseId!.Value,
                    NextPhaseId = resultphase2.Value.Id,
                    IsCurrentPhase = true,
                    StartDate = DateTime.Now
                };
                await _unitOfWork.Repository<StudentPhaseProgress>().AddAsync(phaseProgress);
            }

            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            return await GetUserByIdAsync(user.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al actualizar usuario");
            throw;
        }
    }

    /// <summary>
    /// Restablece la contraseña de un usuario usando su número de identificación
    /// </summary>
    public async Task<Result<bool>> AdminResetPasswordAsync(Guid userId)
    {
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
        if (user is null)
            return Result<bool>.Failure(SystemErrors.UserError.NotFound);

        // Usar el número de identificación como nueva contraseña
        user.PasswordHash = _passwordHasher.HashPassword(user.IdentificationNumber);
        user.IsPasswordSetByAdmin = true;
        user.PasswordChangeDate = DateTime.Now;

        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    /// <summary>
    /// Cambia la contraseña de un usuario
    /// </summary>
    public async Task<Result<bool>> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
            return Result<bool>.Failure(SystemErrors.UserError.InvalidPassword);

        var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
        if (user is null)
            return Result<bool>.Failure(SystemErrors.UserError.NotFound);

        // Verificar contraseña actual
        if (!_passwordHasher.VerifyPassword(user.PasswordHash, currentPassword))
            return Result<bool>.Failure(SystemErrors.UserError.InvalidPassword);

        // Actualizar contraseña
        user.PasswordHash = _passwordHasher.HashPassword(newPassword);
        user.IsPasswordSetByAdmin = false;
        user.PasswordChangeDate = DateTime.Now;

        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    public async Task<Result<string>> GetWingTypeAsync(Guid userId)
    {
        var userProfile = await _unitOfWork
                .Repository<AviationProfile>()
                .GetFirstAsync(u => u.UserId == userId);

        return Result<string>.Success(userProfile?.WingType ?? string.Empty);
    }

    #region Métodos privados auxiliares
    /// <summary>
    /// Valida que los campos únicos no estén duplicados antes de crear un usuario
    /// </summary>
    private async Task<Result<bool>> ValidateUniqueFieldsCreateAsync(RegisterDto registerDto)
    {
        // Validar Username único
        var lowerUsername = registerDto.Username.ToLower();
        var existingUsername = await _unitOfWork.Repository<User>()
            .AnyAsync(u => u.Username.ToLower() == lowerUsername);

        if (existingUsername)
            return Result<bool>.Failure(SystemErrors.UserError.UsernameExists);

        // Validar número de identificación único
        var existingIdentification = await _unitOfWork.Repository<User>()
            .AnyAsync(u => u.IdentificationNumber == registerDto.IdentificationNumber);

        if (existingIdentification)
            return Result<bool>.Failure(SystemErrors.UserError.IdentificationExists);

        if (!string.IsNullOrWhiteSpace(registerDto.PhoneNumber))
        {
            // validar teléfono único
            var existingPhone = await _unitOfWork.Repository<User>()
                .AnyAsync(u => u.PhoneNumber == registerDto.PhoneNumber);

            if (existingPhone)
                return Result<bool>.Failure(SystemErrors.UserError.PhoneNumberExists);
        }

        if (!string.IsNullOrWhiteSpace(registerDto.PID))
        {
            // validar PID único
            var existingPID = await _unitOfWork.Repository<AviationProfile>()
                .AnyAsync(a => a.PID == registerDto.PID);

            if (existingPID)
                return Result<bool>.Failure(SystemErrors.ProfileError.AviationProfileExists);
        }

        if (!string.IsNullOrWhiteSpace(registerDto.Grade) && registerDto.SeniorityOrder is not null && registerDto.SeniorityOrder > 0)
        {
            // validar grado y orden de antiguedad únicos
            var existingGrade = await _unitOfWork.Repository<User>()
                .AnyAsync(a => a.Grade == registerDto.Grade && a.SeniorityOrder == registerDto.SeniorityOrder);

            if (existingGrade)
                return Result<bool>.Failure(SystemErrors.UserError.SeniorityOrderExists);
        }

        return Result<bool>.Success(true);
    }

    /// <summary>
    /// Valida que los campos únicos no estén duplicados antes de actualizar un usuario
    /// </summary>
    private async Task<Result<bool>> ValidateUniqueFieldsUpdateAsync(UpdateDto updateDto)
    {
        // Validar Username único
        var lowerUsername = updateDto.Username.ToLower();
        var existingUsername = await _unitOfWork.Repository<User>()
            .AnyAsync(u => u.Username.ToLower() == lowerUsername && u.Id != updateDto.Id);

        if (existingUsername)
            return Result<bool>.Failure(SystemErrors.UserError.UsernameExists);

        // Validar número de identificación único
        var existingIdentification = await _unitOfWork.Repository<User>()
            .AnyAsync(u => u.IdentificationNumber == updateDto.IdentificationNumber && u.Id != updateDto.Id);

        if (existingIdentification)
            return Result<bool>.Failure(SystemErrors.UserError.IdentificationExists);

        if (!string.IsNullOrWhiteSpace(updateDto.PhoneNumber))
        {
            // validar teléfono único
            var existingPhone = await _unitOfWork.Repository<User>()
                .AnyAsync(u => u.PhoneNumber == updateDto.PhoneNumber && u.Id != updateDto.Id);

            if (existingPhone)
                return Result<bool>.Failure(SystemErrors.UserError.PhoneNumberExists);
        }

        if (!string.IsNullOrWhiteSpace(updateDto.PID))
        {
            // validar PID único
            var existingPID = await _unitOfWork.Repository<AviationProfile>()
                .AnyAsync(a => a.PID == updateDto.PID && a.UserId != updateDto.Id);

            if (existingPID)
                return Result<bool>.Failure(SystemErrors.ProfileError.AviationProfileExists);
        }

        if (!string.IsNullOrWhiteSpace(updateDto.Grade) && updateDto.SeniorityOrder is not null && updateDto.SeniorityOrder > 0)
        {
            // validar grado y orden de antiguedad únicos
            var existingGrade = await _unitOfWork.Repository<User>()
                .AnyAsync(a => a.Grade == updateDto.Grade && a.SeniorityOrder == updateDto.SeniorityOrder && a.Id != updateDto.Id);

            if (existingGrade)
                return Result<bool>.Failure(SystemErrors.UserError.SeniorityOrderExists);
        }

        return Result<bool>.Success(true);
    }

    /// <summary>
    /// Actualiza los roles de un usuario
    /// </summary>
    private async Task UpdateUserRolesAsync(User user, List<Guid> selectedRoleIds)
    {
        // Obtener los roles actuales del usuario
        var currentRoles = await _unitOfWork.Repository<UserRole>()
            .GetListAsync(ur => ur.UserId == user.Id);

        // Obtener IDs de roles actuales
        var currentRoleIds = currentRoles.Select(ur => ur.RoleId).ToList();

        // Determinar roles a eliminar
        var rolesToRemove = currentRoleIds.Except(selectedRoleIds).ToList();

        // Determinar roles a agregar
        var rolesToAdd = selectedRoleIds.Except(currentRoleIds).ToList();

        // Eliminar roles que ya no están seleccionados
        foreach (var roleId in rolesToRemove)
        {
            var userRole = await _unitOfWork.Repository<UserRole>()
                .GetFirstAsync(ur => ur.UserId == user.Id && ur.RoleId == roleId);

            if (userRole != null)
            {
                userRole.IsDeleted = true;
                _unitOfWork.Repository<UserRole>().Update(userRole);
            }
        }

        // Agregar nuevos roles
        foreach (var roleId in rolesToAdd)
        {
            // Verificar si el rol existe
            var roleExists = await _unitOfWork.Repository<Role>()
                .AnyAsync(r => r.Id == roleId);

            if (roleExists)
            {
                // Buscar si ya existe la relación (incluir eliminados)
                var existingUserRole = await _unitOfWork.Repository<UserRole>()
                    .GetFirstAsync(
                        ur => ur.UserId == user.Id && ur.RoleId == roleId,
                        ignoreQueryFilters: true
                    );

                if (existingUserRole != null)
                {
                    // Reactivar relación existente
                    existingUserRole.IsDeleted = false;
                    existingUserRole.ExpiresAt = null;
                    _unitOfWork.Repository<UserRole>().Update(existingUserRole);
                }
                else
                {
                    // Crear nueva relación
                    var newUserRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = roleId
                    };
                    await _unitOfWork.Repository<UserRole>().AddAsync(newUserRole);
                }
            }
        }
    }

    /// <summary>
    /// Actualiza el perfil de aviación de un usuario
    /// </summary>
    private async Task UpdateUserAviationProfileAsync(User user, UpdateDto updateUserDto, bool needsAviationProfile)
    {
        if (needsAviationProfile)
        {
            // Buscar el perfil de aviación existente, incluyendo los eliminados lógicamente
            var existingAviationProfile = await _unitOfWork.Repository<AviationProfile>()
                .GetFirstAsync(a => a.UserId == user.Id, ignoreQueryFilters: true);

            if (existingAviationProfile != null)
            {
                // Actualizar o reactivar perfil existente
                existingAviationProfile.PID = updateUserDto.PID!;
                existingAviationProfile.FlightPosition = updateUserDto.FlightPosition!;
                existingAviationProfile.WingType = updateUserDto.WingType!;
                existingAviationProfile.IsDeleted = false;
                _unitOfWork.Repository<AviationProfile>().Update(existingAviationProfile);
            }
            else
            {
                // Crear nuevo perfil
                var aviationProfile = new AviationProfile
                {
                    UserId = user.Id,
                    PID = updateUserDto.PID!,
                    FlightPosition = updateUserDto.FlightPosition!,
                    WingType = updateUserDto.WingType!
                };
                await _unitOfWork.Repository<AviationProfile>().AddAsync(aviationProfile);
            }
        }
        else
        {
            // Si ya no tiene roles de aviación, marcar el perfil como eliminado
            if (user.AviationProfile != null)
            {
                user.AviationProfile.IsDeleted = true;
                _unitOfWork.Repository<AviationProfile>().Update(user.AviationProfile);
            }
        }
    }

    /// <summary>
    /// Actualiza el WingType en el UserCourse activo del usuario si existe
    /// </summary>
    private async Task UpdateUserCourseWingTypeAsync(Guid userId, UpdateDto updateDto)
    {
        if (string.IsNullOrEmpty(updateDto.WingType))
            return;

        // Obtener el UserCourse activo del usuario (debería ser solo uno)
        var activeUserCourse = await _unitOfWork.Repository<UserCourse>()
            .GetFirstAsync(uc => uc.UserId == userId);

        // Si tiene un programa activo, actualizar el WingType
        if (activeUserCourse != null && activeUserCourse.WingType != updateDto.WingType)
        {
            activeUserCourse.WingType = updateDto.WingType;
            _unitOfWork.Repository<UserCourse>().Update(activeUserCourse);

            // es obligatorio desactivar la fase donde se encuentra si cambio de ala y asignar la nueva fase
            var currentPhaseProgress = await _unitOfWork.Repository<StudentPhaseProgress>().GetFirstAsync(spp => spp.StudentId == updateDto.Id && spp.IsCurrentPhase);
            // Desactivar el progreso de fase actual si existe
            if (currentPhaseProgress != null)
            {
                currentPhaseProgress.IsCurrentPhase = false;
                currentPhaseProgress.Status = UserConstants.StudentStatus.ChangeWing;
                currentPhaseProgress.EndDate = DateTime.Now;
                //currentPhaseProgress.IsDeleted = true;
                _unitOfWork.Repository<StudentPhaseProgress>().Update(currentPhaseProgress);
            }

            var resultphase2 = await _phaseService.GetPhaseByNumberAndWingTypeAsync(2, updateDto.WingType);
            if (!resultphase2.IsSuccess || resultphase2.Value.PrerequisitePhaseId == null)
            {
                throw new Exception(SystemErrors.PhaseError.PhaseNotFound.Message);
            }
            // Asignar la nueva fase
            var phaseProgress = new StudentPhaseProgress
            {
                StudentId = updateDto.Id,
                CourseId = updateDto.CourseId!.Value,
                PhaseId = resultphase2.Value.PrerequisitePhaseId!.Value,
                NextPhaseId = resultphase2.Value.Id,
                IsCurrentPhase = true,
                StartDate = DateTime.Now
            };
            await _unitOfWork.Repository<StudentPhaseProgress>().AddAsync(phaseProgress);
        }
    }

    /// <summary>
    /// Desasigna al usuario de su programa activo
    /// </summary>
    private async Task UnassignUserFromCourseAsync(Guid userId)
    {
        // Obtener el UserCourse activo del usuario
        var activeUserCourse = await _unitOfWork.Repository<UserCourse>()
            .GetFirstAsync(uc => uc.UserId == userId && uc.IsActive);

        // Si tiene un programa activo, desasignarlo
        if (activeUserCourse != null)
        {
            activeUserCourse.IsActive = false;
            activeUserCourse.UnassignmentDate = DateTime.Now;
            activeUserCourse.UnassignmentReason = UserConstants.StudentStatus.StudentRoleRemoved;
            _unitOfWork.Repository<UserCourse>().Update(activeUserCourse);
        }

        // Desactivar el progreso de fase actual si existe
        var currentPhaseProgress = await _unitOfWork.Repository<StudentPhaseProgress>()
            .GetFirstAsync(spp => spp.StudentId == userId && spp.IsCurrentPhase);

        if (currentPhaseProgress != null)
        {
            currentPhaseProgress.IsCurrentPhase = false;
            currentPhaseProgress.Status = UserConstants.StudentStatus.StudentRoleRemoved;
            currentPhaseProgress.EndDate = DateTime.Now;
            //currentPhaseProgress.IsDeleted = true;
            _unitOfWork.Repository<StudentPhaseProgress>().Update(currentPhaseProgress);
        }
    }
    #endregion
}
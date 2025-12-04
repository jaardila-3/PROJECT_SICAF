using SICAF.Common.Models.Results;

namespace SICAF.Common.Constants;

/// <summary>
/// Catálogo centralizado de errores del sistema
/// </summary>
public static class SystemErrors
{
    // Errores generales del sistema
    public static class GeneralError
    {
        public static Error NotFound => new("RESOURCE_NOT_FOUND", "Recurso no encontrado");
    }

    // Errores de Usuario
    public static class UserError
    {
        public static Error NotFound => new("USER_NOT_FOUND", "Usuario no encontrado");
        public static Error SeniorityOrderExists => new("USER_SENIORITY_ORDER_EXISTS", "Ya existe un usuario con esa jerarquia");
        public static Error InvalidPassword => new("USER_INVALID_PASSWORD", "Contraseña incorrecta");
        public static Error Blocked => new("USER_BLOCKED", "Usuario bloqueado");
        public static Error UsernameExists => new("USERNAME_EXISTS", "El nombre de usuario ya está registrado");
        public static Error IdentificationExists => new("USER_IDENTIFICATION_EXISTS", "La identificación ya está registrada");
        public static Error PhoneNumberExists => new("USER_PHONE_NUMBER_EXISTS", "El número de teléfono ya está registrado");
    }

    // Errores de Curso/programa
    public static class CourseError
    {
        public static Error NotFound => new("COURSE_NOT_FOUND", "Programa no encontrado");
        public static Error NumberExists => new("COURSE_NUMBER_EXISTS", "Ya existe un Programa con ese número");
        public static Error NotActive => new("COURSE_NOT_ACTIVE", "El Programa no está activo");
        public static Error NoActiveCourses => new("NO_ACTIVE_COURSES", "No existen cursos activos");
        public static Error Required => new("COURSE_REQUIRED", "El Programa es obligatorio");
        public static Error InvalidDateRange => new("COURSE_INVALID_DATES", "La fecha de finalización debe ser posterior a la fecha de inicio");
    }

    // Errores de Inscripción
    public static class EnrollmentError
    {
        public static Error StudentAlreadyEnrolled => new("STUDENT_ALREADY_ENROLLED", "El estudiante ya está inscrito en un Programa activo");
        public static Error StudentNotEnrolled => new("STUDENT_NOT_ENROLLED", "El estudiante no está inscrito en el Programa");
    }

    // Errores de perfiles
    public static class ProfileError
    {
        public static Error AviationProfileRequired => new("AVIATION_PROFILE_REQUIRED", "PID y Tipo de Ala son obligatorios para roles de aviación");
        public static Error AviationProfileExists => new("AVIATION_PROFILE_EXISTS", "El PID ya existe");
    }

    public static class PhaseError
    {
        public static Error PhaseNotFound => new("PHASE_NOT_FOUND", "Fase no encontrada");
        public static Error NotCurrentPhase => new("NOT_CURRENT_PHASE", "No es la fase actual");
    }

    public static class MissionError
    {
        public static Error MissionNotFound => new("MISSION_NOT_FOUND", "Misión no encontrada");
    }

    public static class TaskError
    {
        public static Error TaskNotFound => new("TASK_NOT_FOUND", "Tarea no encontrada");
        public static Error EvaluationEditTimeLimitExceeded => new("EVALUATION_EDIT_TIME_LIMIT_EXCEEDED", "El tiempo de edición de la evaluación ha sido excedido");
        public static Error InstructorMismatch => new("INSTRUCTOR_MISMATCH", "Solo el instructor que calificó puede editar las calificaciones");
    }

    public static class EvaluationError
    {
        public static Error EvaluationSaveError => new("EVALUATION_SAVE_ERROR", "Error al guardar la evaluación");
        public static Error EvaluationExists => new("EVALUATION_EXISTS", "Ya existe una evaluación con la misma fecha");
        public static Error InvalidChronologicalOrder(DateTime lastEvaluationDate) => new("INVALID_CHRONOLOGICAL_ORDER", $"La fecha de evaluación debe ser posterior a la última misión evaluada ({lastEvaluationDate:dd/MM/yyyy})");
        public static Error FutureEvaluationDate => new("FUTURE_EVALUATION_DATE", "No se pueden grabar misiones con fechas futuras");
        public static Error EvaluationDateTooOld => new("EVALUATION_DATE_TOO_OLD", "No se pueden grabar misiones con más de 30 días de atraso");
    }
}
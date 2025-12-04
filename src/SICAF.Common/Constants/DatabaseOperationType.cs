namespace SICAF.Common.Constants;

public static class DatabaseOperationType
{
    public const string Create = "Creación";
    public const string Read = "Lectura";
    public const string Update = "Actualización";
    public const string Delete = "Eliminación";
    public const string Login = "Inicio de sesión";
    public const string AdminResetPassword = "Restablecimiento de contraseña por administrador";
    public const string ChangePassword = "Cambio de contraseña";
    public const string Unlock = "Desbloqueo de cuenta";
    public const string Lock = "Bloqueo de cuenta";
    public const string ChangeCourse = "Cambio de programa";
    public const string EnrollStudent = "Inscripción de estudiante";
    public const string AssignedInstructor = "Asignación de instructor";
    public const string UnassignedInstructor = "Desasignación de instructor";

    public static string[] ValidTypes =
    [
        Create, Read, Update, Login, AdminResetPassword, ChangePassword, Unlock, Lock, ChangeCourse, EnrollStudent
    ];
}
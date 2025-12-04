namespace SICAF.Common.Constants;

public static class UserConstants
{
    public const string ALL_OPTION = "TODAS";

    public static class StudentStatus
    {
        public const string PendingCommittee = "PENDIENTE COMITE";
        public const string Suspended = "SUSPENDIDO";
        public const string Active = "ACTIVO";
        public const string PhaseCompleted = "FASE COMPLETADA";
        public const string CourseCompleted = "PROGRAMA COMPLETADO";
        public const string ChangeCourse = "CAMBIO DE PROGRAMA";
        public const string StudentRoleRemoved = "ROL DE ESTUDIANTE REMOVIDO";
        public const string ChangeWing = "CAMBIO DE ALA";

        public static string GetStatusBadgeClass(string status)
        {
            return status switch
            {
                Active => "bg-success",
                PendingCommittee => "bg-warning",
                Suspended or StudentRoleRemoved or ChangeWing or ChangeCourse => "bg-danger",
                CourseCompleted => "bg-info",
                PhaseCompleted => "bg-primary",
                _ => "bg-secondary"
            };
        }
    }

    public static class CommitteeReasons
    {
        public const string MaxCommitteesPerPhase = "MÁXIMO DE COMITES POR FASE, ESTUDIANTE SUSPENDIDO";
        public const string MaxFailedMissionsForCommittee = "MÁXIMO DE MISIONES FALLIDAS POR FASE, ESTUDIANTE EN COMITE";
        public const string MaxTotalFailedMissionsInCourse = "MÁXIMO DE MISIONES FALLIDAS POR PROGRAMA, ESTUDIANTE SUSPENDIDO";
        public const string FactorEmotional = "ESTUDIANTE CON FACTOR DE RIESGO EMOCIONAL";
    }

    public static class CommitteeDecisions
    {
        public const string ContinueCourse = "CONTINUAR PROGRAMA";
        public const string Suspendecourse = "SUSPENDER PROGRAMA";
    }

}

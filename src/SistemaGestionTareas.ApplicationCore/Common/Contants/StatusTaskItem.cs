namespace SistemaGestionTareas.ApplicationCore.Common.Contants
{
    public static class StatusTaskItem
    {
        public const string Pendiente = "Pendiente";
        public const string Completado = "Completado";
        public const string Todos = "";

        public static void ValidateStatus(string? status)
        {
            if(status != null)
            {
                if (status != Pendiente && status != Completado)
                    throw new ArgumentException("Status inválido");
            }
        }
    }
}

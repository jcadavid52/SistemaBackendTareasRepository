namespace SistemaGestionTareas.ApplicationCore.Common.Contants
{
    public class TimeZone
    {
        public DateTime horaColombia = TimeZoneInfo
    .ConvertTimeFromUtc(DateTime.UtcNow,
        TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));
    }
}

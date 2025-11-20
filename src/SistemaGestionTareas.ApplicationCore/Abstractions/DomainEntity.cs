namespace SistemaGestionTareas.ApplicationCore.Abstractions
{
    public abstract class DomainEntity<TKey>
    {
        public TKey Id { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = GetTimeZone();
        public DateTime UpdatedAt { get; set;} = GetTimeZone();

        private static DateTime GetTimeZone()
        {
            return TimeZoneInfo
            .ConvertTimeFromUtc(DateTime.UtcNow,
             TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));
        }
    }
}

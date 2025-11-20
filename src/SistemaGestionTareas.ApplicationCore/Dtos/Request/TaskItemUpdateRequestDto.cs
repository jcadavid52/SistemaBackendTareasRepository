using System.Text.Json.Serialization;

namespace SistemaGestionTareas.ApplicationCore.Dtos.Request
{
    public record TaskItemUpdateRequestDto(
          string? Title,
          string? Description,
          string? Status
    )
    {
        [JsonIgnore]
        public int Id { get; init; }
    }
}

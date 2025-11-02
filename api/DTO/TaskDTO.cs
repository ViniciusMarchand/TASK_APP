using System.Text.Json.Serialization;
using api.Enums;

namespace api.DTO;

public record TaskDTO
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status Status { get; init; }
}
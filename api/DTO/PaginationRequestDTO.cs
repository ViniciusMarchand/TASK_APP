using System.ComponentModel.DataAnnotations;
using api.Enums;

namespace api.DTO;

public record PaginationRequestDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be at least 1")]
    public int PageNumber { get; init; }

    [Range(1, 50, ErrorMessage = "PageSize must be between 1 and 50")]
    public int PageSize { get; init; }

    public Status? StatusFilter { get; init; }
}
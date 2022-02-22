using _4kTiles_Backend.DataObjects.DTO.Response;

namespace _4kTiles_Backend.DataObjects.DTO.Pagination;

public record PaginationResponseDTO<T> : ResponseDTO<IEnumerable<T>>
{
    public int TotalRecords { get; set; } = 0;
}
using System.ComponentModel.DataAnnotations;

namespace _4kTiles_Backend.DataObjects.DTO.Library;

public class LibraryGenreDTO
{
    [Required]
    public string Name { get; set; }
}
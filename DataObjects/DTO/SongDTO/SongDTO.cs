using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace _4kTiles_Backend.DataObjects.DTO.SongDTO
{
    public class SongDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Song name must be from 1 to 100 characters")]
        public string SongName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Song name must be from 1 to 100 characters")]
        public string Author { get; set; }
        [Required]
        public int Bpm { get; set; }
        [Required]
        public string Notes { get; set; } = null!;
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public bool IsPublic { get; set; }
    }
}
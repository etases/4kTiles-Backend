using System.ComponentModel.DataAnnotations;

namespace _4kTiles_Backend.DataObjects.DTO.Song
{
    public class CreateSongDTO
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
        public bool IsPublic { get; set; }
        [Required]
        public ICollection<string> Genres { get; set; }
    }
}
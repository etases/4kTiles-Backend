namespace _4kTiles_Backend.DataObjects.DTO.Song
{
    public class SongInfoDTO
    {
        public int SongId { get; set; }
        public string SongName { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int CreatorId { get; set; }
        public string CreatorName { get; set; } = null!;
        public int Bpm { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<string> Genres { get; set; } = null!;
    }
}

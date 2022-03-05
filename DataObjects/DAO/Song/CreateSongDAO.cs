namespace _4kTiles_Backend.DataObjects.DAO.Song;

public class CreateSongDAO
{
    public string SongName { get; set; }
    public string Author { get; set; }
    public int Bpm { get; set; }
    public string Notes { get; set; }
    public DateTime ReleaseDate { get; set; } = DateTime.Now;
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
    public bool IsPublic { get; set; }
    public ICollection<string> Genres { get; set; }
    public int CreatorId { get; set; }
}
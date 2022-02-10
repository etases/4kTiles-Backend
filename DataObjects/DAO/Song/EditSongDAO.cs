namespace _4kTiles_Backend.DataObjects.DAO.Song;


public class EditSongDAO
{
    public string SongName { get; set; }
    public string Author { get; set; }
    public int Bpm { get; set; }
    public string Notes { get; set; }
    public bool IsPublic { get; set; }
    public ICollection<string> Tags { get; set; }
}
namespace _4kTiles_Backend.DataObjects.DTO.Leaderboard
{
    public class LeaderboardUserDTO
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public int SongId { get; set; }
        public string SongName { get; set; }
        public int BestScore { get; set; }
    }
}
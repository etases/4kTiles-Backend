using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace _4kTiles_Backend.DataObjects.DTO.LeaderboardDTO
{
    public class LeaderboardAccountDTO
    {
        [Required]
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public int BestScore { get; set; }
    }
}

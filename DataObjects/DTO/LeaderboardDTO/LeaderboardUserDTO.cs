using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _4kTiles_Backend.DataObjects.DTO.LeaderboardDTO
{
    public class LeaderboardUserDTO
    {
        [JsonIgnore]
        public int AccountId { get; set; }
        public string UserName { get; set; }
        [JsonIgnore]
        public int SongId { get; set; }
        public string SongName { get; set; }
        public int BestScore { get; set; }
    }
}
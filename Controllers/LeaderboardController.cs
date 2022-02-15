using _4kTiles_Backend.DataObjects.DTO.LeaderboardDTO;
using _4kTiles_Backend.DataObjects.DTO.Response;
using _4kTiles_Backend.Entities;
using _4kTiles_Backend.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _4kTiles_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardRepository _leaderboardRepository;

        public LeaderboardController(ILeaderboardRepository leaderboardRepository)
        {
            _leaderboardRepository = leaderboardRepository;
        }

        [HttpGet("Song/{songId}/{limit?}")]
        public List<LeaderboardAccountDTO> GetLeaderboardBySongId(int songId, int limit = 10)
        {
            return _leaderboardRepository.GetTopNLeaderboardBySongId(songId, limit);
        }

        [HttpGet("Account/{accountId}")]
        public List<LeaderboardUserDTO> GetLeaderboardByUserId(int accountId)
        {
            return _leaderboardRepository.GetTopOneByUserId(accountId);
        }

        [HttpPut]
        [Authorize]
        public ActionResult<ResponseDTO<AccountSong>> AddUserBestScore(int accountId, int songId, int score)
        {
            AccountSong accountSong = _leaderboardRepository.AddUserBestScore(accountId, songId, score);
            return Ok(new ResponseDTO<AccountSong>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Successfully added user best score",
                Data = accountSong
            });
        }
    }
}

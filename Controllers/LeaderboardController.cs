using _4kTiles_Backend.DataObjects.DTO.LeaderboardDTO;
using _4kTiles_Backend.DataObjects.DTO.Response;
using _4kTiles_Backend.Entities;
using Microsoft.AspNetCore.Mvc;
using _4kTiles_Backend.Services.Repositories;

namespace _4kTiles_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardRepository _leaderboardRepository;

        public LeaderboardController(
            ILeaderboardRepository leaderboardRepository
        )
        {
            _leaderboardRepository = leaderboardRepository;
        }

        [HttpGet("getTopNLeaderboardBySongId/{songId}/{n}")]
        public IActionResult GetLeaderboardBySongId(int songId, int? limit)
        {
            List<LeaderboardAccountDTO> leaderboard =
                _leaderboardRepository
                    .getTopNLeaderboardBySongId(songId, limit);
            return Ok(leaderboard);
        }

        [HttpGet("getTopOneByUserId/{accountId}")]
        public IActionResult GetLeaderboardByUserId(int accountId)
        {
            List<LeaderboardUserDTO> leaderboard =
                _leaderboardRepository
                    .getTopOneByUserId(accountId);
            return Ok(leaderboard);
        }

        [HttpPut]
        public IActionResult AddUserBestScore(int accountId, int songId, int score)
        {
            AccountSong accountSong =
                _leaderboardRepository
                    .addUserBestScore(accountId, songId, score);
            return Ok(new ResponseDTO {
                StatusCode = StatusCodes.Status200OK,
                Message = "Successfully added user best score",
                Data = accountSong
            });
        }
    }
}

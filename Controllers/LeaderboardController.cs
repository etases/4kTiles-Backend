using _4kTiles_Backend.DataObjects.DTO.LeaderboardDTO;
using _4kTiles_Backend.DataObjects.DTO.Pagination;
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

        [HttpGet("Song/{songId}")]
        public PaginationResponseDTO<LeaderboardAccountDTO> GetLeaderboardBySongId(int songId, [FromQuery] PaginationParameter pagination)
        {
            var leaderboard = _leaderboardRepository.GetTopLeaderboardBySongId(songId, pagination);
            return new PaginationResponseDTO<LeaderboardAccountDTO>()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get leaderboard by song id",
                TotalRecords = leaderboard.TotalRecords,
                Data = leaderboard.Payload
            };
        }

        [HttpGet("Account/{accountId}")]
        public PaginationResponseDTO<LeaderboardUserDTO> GetLeaderboardByUserId(int accountId, [FromQuery] PaginationParameter pagination)
        {
            var leaderboard = _leaderboardRepository.GetTopOneByUserId(accountId, pagination);
            return new PaginationResponseDTO<LeaderboardUserDTO>()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get leaderboard by account id",
                TotalRecords = leaderboard.TotalRecords,
                Data = leaderboard.Payload
            };
        }

        [HttpPut("Admin/{accountId:int}")]
        [Authorize("Manager")]
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

        [HttpPut("User")]
        [Authorize]
        public ActionResult<ResponseDTO<AccountSong>> AddUserBestScore(int songId, int score)
        {
            var badResponse = BadRequest(new ResponseDTO
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsError = true,
                Message = "Invalid token"
            });
            string? accountValueClaim = User.FindFirst("accountId")?.Value;
            if (accountValueClaim is null) return badResponse;
            if (!int.TryParse(accountValueClaim, out var accountId)) return badResponse;

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

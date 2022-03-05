using _4kTiles_Backend.DataObjects.DTO.Leaderboard;
using _4kTiles_Backend.DataObjects.DTO.Pagination;
using _4kTiles_Backend.DataObjects.DTO.Response;
using _4kTiles_Backend.DataObjects.DTO.Song;
using _4kTiles_Backend.Entities;
using _4kTiles_Backend.Services.Repositories;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _4kTiles_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardRepository _leaderboardRepository;
        private readonly IMapper _mapper;

        public LeaderboardController(ILeaderboardRepository leaderboardRepository, IMapper mapper)
        {
            _leaderboardRepository = leaderboardRepository;
            _mapper = mapper;
        }

        [HttpGet("Song/{songId}")]
        public async Task<ActionResult<PaginationResponseDTO<LeaderboardAccountDTO>>> GetLeaderboardBySongId(int songId, [FromQuery] PaginationParameter pagination)
        {
            var leaderboard = await _leaderboardRepository.GetTopLeaderboardBySongId(songId, pagination);
            return leaderboard != null
                ? new PaginationResponseDTO<LeaderboardAccountDTO>()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Get leaderboard by song id",
                    TotalRecords = leaderboard.TotalRecords,
                    Data = leaderboard.Payload
                }
                : NotFound(new ResponseDTO(statusCode: StatusCodes.Status404NotFound, isError: true,
                    message: "Song Not Found"));
        }

        [HttpGet("Account/{accountId}")]
        public async Task<ActionResult<PaginationResponseDTO<LeaderboardUserDTO>>> GetLeaderboardByUserId(int accountId, [FromQuery] PaginationParameter pagination)
        {
            var leaderboard = await _leaderboardRepository.GetTopOneByUserId(accountId, pagination);
            return leaderboard != null
                ? new PaginationResponseDTO<LeaderboardUserDTO>()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Get leaderboard by account id",
                    TotalRecords = leaderboard.TotalRecords,
                    Data = leaderboard.Payload
                }
                : NotFound(new ResponseDTO(statusCode: StatusCodes.Status404NotFound, isError: true,
                    message: "Account Not Found"));
        }

        [HttpPut("Admin/{accountId:int}")]
        [Authorize("Manager")]
        public async Task<ActionResult<ResponseDTO<AccountScoreDTO>>> AddUserBestScore(int accountId, int songId, int score)
        {
            AccountSong? accountSong = await _leaderboardRepository.AddUserBestScore(accountId, songId, score);
            return accountSong == null
                ? BadRequest(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest, IsError = true, Message = "Invalid account id / song id"
                })
                : Ok(new ResponseDTO<AccountScoreDTO>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Successfully added user best score",
                    Data = _mapper.Map<AccountScoreDTO>(accountSong)
                });
        }

        [HttpPut("User")]
        [Authorize]
        public async Task<ActionResult<ResponseDTO<AccountScoreDTO>>> AddUserBestScore(int songId, int score)
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

            AccountSong? accountSong = await _leaderboardRepository.AddUserBestScore(accountId, songId, score);
            return accountSong == null
                ? badResponse
                : Ok(new ResponseDTO<AccountScoreDTO>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Successfully added user best score",
                    Data = _mapper.Map<AccountScoreDTO>(accountSong)
                });
        }
    }
}

using _4kTiles_Backend.DataObjects.DAO.Song;
using _4kTiles_Backend.DataObjects.DTO.Response;
using _4kTiles_Backend.DataObjects.DTO.Song;
using _4kTiles_Backend.Entities;
using _4kTiles_Backend.Services.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _4kTiles_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ISongRepository _songRepository;
        private readonly IMapper _mapper;

        public SongController(ISongRepository songRepository, IMapper mapper)
        {
            _songRepository = songRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the song by its Id
        /// </summary>
        /// <param name="id">song id</param>
        /// <returns>the response of the song content</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseDTO<SongDTO>>> GetSongByID(int id)
        {
            var song = await _songRepository.GetSongByID(id);
            return song == null
                ? NotFound(new ResponseDTO(statusCode: StatusCodes.Status404NotFound, isError: true,
                    message: "Song Not Found"))
                : Ok(new ResponseDTO<SongDTO> {StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<SongDTO>(song), Message = "Song Found"});
        }

        /// <summary>
        /// Create new song
        /// </summary>
        /// <param name="song">song DTO</param>
        /// <returns>Success message</returns>
        [HttpPost("Create")]
        public async Task<ActionResult<ResponseDTO<SongDTO>>> CreateNewSong([FromBody] CreateSongDTO song)
        {
            var badResponse = BadRequest(new ResponseDTO
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsError = true,
                Message = "Invalid token"
            });

            // Get the account Id
            string? accountValueClaim = User.FindFirst("accountId")?.Value;
            if (accountValueClaim is null) return badResponse;
            if (!int.TryParse(accountValueClaim, out var accountId)) return badResponse;

            //Mapping data from source object to new obj.
            CreateSongDAO songDAO = _mapper.Map<CreateSongDAO>(song);
            songDAO.CreatorId = accountId;
            var dao = await _songRepository.CreateSong(songDAO);

            return dao == null
                ? badResponse
                : Created("success", new ResponseDTO<SongDTO>
                {
                    StatusCode = StatusCodes.Status201Created,
                    Message = "Song Created.",
                    Data = _mapper.Map<SongDTO>(dao)
                });

        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<ResponseDTO>> DeactivateSong(int id)
        {
            var badResponse = BadRequest(new ResponseDTO
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorCode = 1,
                Message = "Invalid token"
            });

            // Get the account Id
            string? accountValueClaim = User.FindFirst("accountId")?.Value;
            if (accountValueClaim is null) return badResponse;
            if (!int.TryParse(accountValueClaim, out var accountId)) return badResponse;

            var song = await _songRepository.GetSongByID(id);
            if (song == null)
            {
                return NotFound(new ResponseDTO { StatusCode = StatusCodes.Status404NotFound, ErrorCode = -1, Message = "Song not exist!" });
            }
            else if (song.CreatorId != id)
            {
                return BadRequest(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorCode = 2,
                    Message = "You are not the creator of the song"
                });
            }

            await _songRepository.DeactivateSong(id);
            return Ok(new ResponseDTO { StatusCode = StatusCodes.Status200OK, Message = "Song Deactivated." });
        }

        [HttpDelete("Delete/Admin/{id:int}")]
        public async Task<ActionResult<ResponseDTO>> DeactivateSongWithAdmin(int id)
        {
            int result = await _songRepository.DeactivateSong(id);
            if (result == -1)
            {
                return NotFound(new ResponseDTO { StatusCode = StatusCodes.Status404NotFound, IsError = true, Message = "Song not exist!" });
            }
            return Ok(new ResponseDTO { StatusCode = StatusCodes.Status200OK, Message = "Song Deactivated." });
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<ResponseDTO>> UpdateSong(int id, [FromBody] EditSongDTO song)
        {
            var badResponse = BadRequest(new ResponseDTO
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsError = true,
                Message = "Invalid token"
            });

            // Get the account Id
            string? accountValueClaim = User.FindFirst("accountId")?.Value;
            if (accountValueClaim is null) return badResponse;
            if (!int.TryParse(accountValueClaim, out var accountId)) return badResponse;

            EditSongDAO editSongDAO = _mapper.Map<EditSongDAO>(song);
            editSongDAO.CallerAccountId = accountId;
            int result = await _songRepository.EditSong(id, editSongDAO);

            return result switch
            {
                -1 => NotFound(new ResponseDTO(statusCode: StatusCodes.Status404NotFound, isError: true, message: "Song not found")),
                -2 => badResponse,
                _ => Ok(new ResponseDTO(statusCode: StatusCodes.Status200OK, message: "Song Updated"))
            };
        }
    }
}
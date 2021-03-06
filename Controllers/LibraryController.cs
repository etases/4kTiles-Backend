#nullable disable
using _4kTiles_Backend.DataObjects.DTO.Library;

using Microsoft.AspNetCore.Mvc;

using _4kTiles_Backend.Services.Repositories;
using _4kTiles_Backend.DataObjects.DTO.Response;
using _4kTiles_Backend.DataObjects.DTO.Pagination;
using _4kTiles_Backend.DataObjects.DTO.Song;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;

namespace _4kTiles_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {

        private readonly ILibraryRepository _libraryService;
        private readonly IMapper _mapper;

        public LibraryController(ILibraryRepository libraryService, IMapper mapper)
        {
            _libraryService = libraryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get public songs
        /// </summary>
        /// <returns>List of public songs</returns>
        [HttpGet]
        public async Task<ActionResult<PaginationResponseDTO<SongInfoDTO>>> GetPublicSongs([FromQuery] PaginationParameter pagination)
        {
            var publicSong =await _libraryService.GetPublicSongs(pagination);

            return Ok(new PaginationResponseDTO<SongInfoDTO>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get public songs",
                TotalRecords = publicSong.TotalRecords,
                Data = _mapper.Map<IEnumerable<SongInfoDTO>>(publicSong.Payload)
            });
        }

        /// <summary>
        /// Get private songs by user id 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pagination"></param>
        /// <returns>List of private songs of user</returns>
        [HttpGet("private")]
        [Authorize]
        public async Task<ActionResult<PaginationResponseDTO<SongInfoDTO>>> GetPrivateSongs([FromQuery] PaginationParameter pagination)
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
            var accountSong = await _libraryService.GetPrivateSongs(accountId, pagination);
            return accountSong == null
                ? badResponse
                : Ok(new PaginationResponseDTO<SongInfoDTO>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success get private songs",
                    TotalRecords = accountSong.TotalRecords,
                    Data = _mapper.Map<IEnumerable<SongInfoDTO>>(accountSong.Payload)
                });
        }

        /// <summary>
        /// Search songs by Filters
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pagination"></param>
        /// <returns>List of public songs satisfied the Filter</returns>
        [HttpGet("search")]
        public async Task<ActionResult<PaginationResponseDTO<SongInfoDTO>>> GetSongByFilters([FromQuery] string searchString, [FromQuery] PaginationParameter pagination)
        {
            LibraryFilterDTO filter = new LibraryFilterDTO();

            if (!string.IsNullOrEmpty(searchString))
            {
                string[] vs = searchString.Split(',');
                foreach (var s in vs)
                {
                    var ts = s.Trim();
                    if (ts.StartsWith("#"))
                    {
                        filter.Author = ts[1..];
                    }
                    else if (ts.StartsWith("@"))
                    {
                        filter.Genre = ts[1..];
                    }
                    else
                    {
                        filter.Name = ts;
                    }
                }
            }

            var result = await _libraryService.GetSongByFilters(filter, pagination);
            return Ok(new PaginationResponseDTO<SongInfoDTO>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get songs by filter",
                TotalRecords = result.TotalRecords,
                Data = _mapper.Map<IEnumerable<SongInfoDTO>>(result.Payload)
            });
        }

        /// <summary>
        /// Get songs by Genre
        /// </summary>
        /// <param name="genre"></param>
        /// <param name="pagination"></param>
        /// <returns>List of songs sastified the Genre</returns>
        [HttpGet("genre/song")]
        public async Task<ActionResult<PaginationResponseDTO<SongInfoDTO>>> GetSongByGenre([FromQuery] LibraryGenreDTO genre, [FromQuery] PaginationParameter pagination)
        {
            var result =await _libraryService.GetSongByGenre(genre.Name, pagination);
            if (result == null)
            {
                return BadRequest(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsError = true,
                    Message = "There is no song match your Genre"
                });
            }
            return Ok(new PaginationResponseDTO<SongInfoDTO>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get songs by Genre",
                TotalRecords = result.TotalRecords,
                Data = _mapper.Map<IEnumerable<SongInfoDTO>>(result.Payload)
            });
        }
        
        /// <summary>
        /// Get all genres
        /// </summary>
        /// <returns></returns>
        [HttpGet("genre")]
        public async Task<ActionResult<ResponseDTO<ICollection<string>>>> GetGenres()
        {
            var genres = await _libraryService.GetGenres();
            return Ok(new ResponseDTO<ICollection<string>>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "All genres",
                Data = genres
            });
        }
    }
}

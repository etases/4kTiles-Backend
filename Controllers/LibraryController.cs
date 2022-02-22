#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _4kTiles_Backend.Context;
using _4kTiles_Backend.Entities;
using _4kTiles_Backend.Services.Repositories;
using _4kTiles_Backend.DataObjects.DTO.LibraryFilterDTO;
using _4kTiles_Backend.DataObjects.DTO.Response;
using _4kTiles_Backend.DataObjects.DTO.LibraryGenreDTO;
using _4kTiles_Backend.DataObjects.DTO.Pagination;

namespace _4kTiles_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {

        private readonly ILibraryRepository _libraryService;

        public LibraryController(ILibraryRepository libraryService)
        {
            _libraryService = libraryService;
        }

        /// <summary>
        /// Get public songs
        /// </summary>
        /// <returns>List of public songs</returns>
        [HttpGet]
        public async Task<ActionResult<PaginationResponseDTO<Song>>> GetPublicSongs([FromQuery] PaginationParameter pagination)
        {
            var publicSong =await _libraryService.GetPublicSongs(pagination);

            return Ok(new PaginationResponseDTO<Song>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get public songs",
                TotalRecords = publicSong.TotalRecords,
                Data = publicSong.Payload
            });
        }

        /// <summary>
        /// Get private songs by user id (not use owner check)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pagination"></param>
        /// <returns>List of private songs of user</returns>
        [HttpPost]
        public async Task<ActionResult<PaginationResponseDTO<Song>>> GetPrivateSongs([FromBody] LibraryUserIdDTO userId, [FromQuery] PaginationParameter pagination)
        {
            var accountSong = await _libraryService.GetPrivateSongs(userId.Id, pagination);
            return accountSong == null
                ? BadRequest(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsError = true,
                    Message = "User Id is Invalid (id= " + userId.Id + ")",
                })
                : Ok(new PaginationResponseDTO<Song>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success get private songs",
                    TotalRecords = accountSong.TotalRecords,
                    Data = accountSong.Payload
                });
        }

        /// <summary>
        /// Search songs by Filters
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pagination"></param>
        /// <returns>List of public songs satisfied the Filter</returns>
        [HttpPost("search")]
        public async Task<ActionResult<PaginationResponseDTO<Song>>> GetSongByFilters([FromBody] LibraryFilterDTO filter, [FromQuery] PaginationParameter pagination)
        {
            var result = await _libraryService.GetSongByFilters(filter, pagination);
            return Ok(new PaginationResponseDTO<Song>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get songs by filter",
                TotalRecords = result.TotalRecords,
                Data = result.Payload
            });
        }

        /// <summary>
        /// Get songs by Genre
        /// </summary>
        /// <param name="genre"></param>
        /// <param name="pagination"></param>
        /// <returns>List of songs sastified the Genre</returns>
        [HttpPost("genre")]
        public async Task<ActionResult<PaginationResponse<Song>>> GetSongByGenre([FromBody] LibraryGenreDTO genre, [FromQuery] PaginationParameter pagination)
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
            return Ok(new PaginationResponseDTO<Song>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get songs by Genre",
                TotalRecords = result.TotalRecords,
                Data = result.Payload
            });
        }

    }
}

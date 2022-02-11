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
        public async Task<IActionResult> GetPublicSongs()
        {
            var publicSong =await _libraryService.GetPublicSongs();

            return Ok(new ResponseDTO
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get public songs",
                Data = publicSong
            });
        }

        /// <summary>
        /// Get private songs by user id (not use owner check)
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>List of private songs of user</returns>
        [HttpPost]
        public async Task<IActionResult> GetPrivateSongs([FromBody] LibraryUserIdDTO UserId)
        {
            var accountSong = await _libraryService.GetPrivateSongs(UserId.Id);
            if (accountSong == null)
            {
                return BadRequest(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "User Id is Invalid (id= " + UserId.Id + ")",
                    Data = ""
                });
            }
            return Ok(new ResponseDTO
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get songs of " + accountSong[0].Author + " (id= " + User.Id + ")",
                Data = accountSong
            });
        }

        /// <summary>
        /// Search songs by Filters
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns>List of public songs satisfied the Filter</returns>
        [HttpPost("search")]
        public async Task<IActionResult> GetSongByFilters([FromBody] LibraryFilterDTO Filter)
        {
            var result =await _libraryService.GetSongByFilters(Filter);
            if (result == null)
            {
                return BadRequest(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "There is no song match your filter",
                    Data = ""
                });
            }
            return Ok(new ResponseDTO
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get songs by filter",
                Data = result
            });
        }

        /// <summary>
        /// Get songs by Genre
        /// </summary>
        /// <param name="Genre"></param>
        /// <returns>List of songs sastified the Genre</returns>
        [HttpPost("genre")]
        public async Task<IActionResult> GetSongByGenre([FromBody] LibraryGenreDTO Genre)
        {
            var result =await _libraryService.GetSongByGenre(Genre.Name);
            if (result == null)
            {
                return BadRequest(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "There is no song match your Genre",
                    Data = ""
                });
            }
            return Ok(new ResponseDTO
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get songs by Genre",
                Data = result
            });
        }

    }
}

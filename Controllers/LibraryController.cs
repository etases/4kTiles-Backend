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

        // GET: api/Library
        [HttpGet]
        public async Task<IActionResult> GetPublicSongs()
        {
            var publicSong = _libraryService.GetPublicSongs();

            return Ok(new ResponseDTO
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get public songs",
                Data = publicSong
            });
        }

        // Post: api/Library
        [HttpPost]
        public async Task<IActionResult> GetPrivateSongs([FromBody] LibraryUserIdDTO User)
        {
            var accountSong = _libraryService.GetPrivateSongs(User.Id);
            if (accountSong == null)
            {
                return BadRequest(new ResponseDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "User Id is Invalid (id= " + User.Id + ")",
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

        // Oost: api/Library/search
        [HttpPost("search")]
        public async Task<IActionResult> GetSongByFilters([FromBody] LibraryFilterDTO Filter)
        {
            var result = _libraryService.GetSongByFilters(Filter);
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
        // Oost: api/Library/genre
        [HttpPost("genre")]
        public async Task<IActionResult> GetSongByGenre([FromBody] LibraryGenreDTO Genre)
        {
            var result = _libraryService.GetSongByGenre(Genre.Name);
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

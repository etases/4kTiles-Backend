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

namespace _4kTiles_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {

        private readonly ILibraryService _libraryService;

        public LibraryController( ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        // GET: api/Library
        [HttpGet]
        public async Task<IActionResult> GetPublicSongs()
        {
            var publicSong = _libraryService.GetPublicSongs();
            return Ok(new { publicSong, message = "Get public songs" });
        }

        // Post: api/Library
        [HttpPost]
        public async Task<IActionResult> GetPrivateSongs([FromBody] LibraryUserIdDTO User)
        {
            var accountSong = _libraryService.GetPrivateSongs(User.Id);
            if (accountSong == null)
            {
                return BadRequest(new { accountSong, message = "User Id is Invalid (id= " + User.Id + ")" });
            }

            return Ok(new { accountSong, message = "Get songs of " + accountSong[0].Author + " (id= " + User.Id + ")" });
        }

        // Oost: api/Library/search
        [HttpPost("search")]
        public async Task<IActionResult> GetSongByFilters([FromBody] LibraryFilterDTO Filter)
        {
            var result = _libraryService.GetSongByFilters(Filter);
            if (result == null)
            {
                return BadRequest(new { result, message = "There is no song match your filter" });
            }

            return Ok(new { result, message = "Get songs by filter" });
        }


    }
}

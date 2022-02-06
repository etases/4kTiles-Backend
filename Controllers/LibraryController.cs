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
        private readonly ApplicationDbContext _context;
        private readonly IAccountRepository _accountRepository;
        private readonly ILibraryService _libraryService;

        public LibraryController(ApplicationDbContext context, IAccountRepository accountRepository, ILibraryService libraryService)
        {
            _accountRepository = accountRepository;
            _context = context;
            _libraryService = libraryService;
        }

        // GET: api/Library
        [HttpGet]
        public async Task<IActionResult> GetPublicSongs()
        {
            var public_song = _libraryService.GetPublicSongs();
            return Ok(new { public_song, message = "Get public songs" });
        }

        // Post: api/Library
        [HttpPost]
        public async Task<IActionResult> GetPrivateSongs([FromBody] LibraryUserIdDTO User)
        {
            var account_song = _libraryService.GetPrivateSongs(User.user_id);
            if (account_song == null)
            {
                return BadRequest(new { account_song, message = "User Id is Invalid (id= " + User.user_id + ")" });
            }

            return Ok(new { account_song, message = "Get songs of " + account_song[0].Author + " (id= " + User.user_id + ")" });
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

        
        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.SongId == id);
        }
    }
}

using _4kTiles_Backend.DataObjects.DAO.Song;
using _4kTiles_Backend.DataObjects.DTO.Pagination;
using _4kTiles_Backend.DataObjects.DTO.Response;
using _4kTiles_Backend.DataObjects.DTO.SongDTO;
using _4kTiles_Backend.Entities;
using _4kTiles_Backend.Services.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
//using 4kTiles-Backend.Models;

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
        /// Create new song
        /// </summary>
        /// <param name="song">song DTO</param>
        /// <returns>Success message</returns>
        [HttpPost("Create")]
        public async Task<ActionResult<ResponseDTO>> CreateNewSong([FromBody] SongDTO song)
        {
            //Mapping data from source ofject to new obj.
            CreateSongDAO songDAO = _mapper.Map<CreateSongDAO>(song);
            int result = await _songRepository.CreateSong(songDAO);

            return Created("success", new ResponseDTO { StatusCode = StatusCodes.Status201Created, Message = "Song Created." });

        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<ResponseDTO>> DeactivateSong(int id)
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

            EditSongDAO editSongDAO = _mapper.Map<EditSongDAO>(song);
            int existedSong = await _songRepository.EditSong(id, editSongDAO);

            if (existedSong == -1)
            {
                return NotFound(new ResponseDTO { StatusCode = StatusCodes.Status404NotFound, IsError = true, Message = "Song not found" });
            }
            return Ok(new ResponseDTO { StatusCode = StatusCodes.Status200OK, Message = "Song Updated" });

        }
    }
}
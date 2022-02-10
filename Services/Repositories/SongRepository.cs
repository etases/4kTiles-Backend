using _4kTiles_Backend.Context;
using _4kTiles_Backend.DataObjects.DAO.Song;
using _4kTiles_Backend.DataObjects.DTO.Pagination;
using _4kTiles_Backend.Entities;
using _4kTiles_Backend.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Services.Repositories
{
    /// <summary>
    /// Song repository interface
    /// </summary>
    public interface ISongRepository
    {
        Task<Song?> GetSongByID(int id);
        Task<int> CreateSong(CreateSongDAO songDAO);
        Task<int> EditSong(int id, EditSongDAO songDAO);
        Task<int> AdminEditTag(int id, Song song);
        Task<int> DeactivateSong(int id);
        bool CheckSongExist(int id);
    }

    public class SongRepository : ISongRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;

        /// <summary>
        /// SongRepository constructor
        /// </summary>
        /// <param name="dbContext">App db context</param>
        /// <param name="mapper">mapper</param>
        public SongRepository(ApplicationDbContext dbContext, IMapper mapper, IAccountRepository accountRepository)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _accountRepository = accountRepository;
        }


        public async Task<Song?> GetSongByID(int id)
        {
            return await _dbContext.Songs.Where(s => s.SongId == id).FirstOrDefaultAsync();
        }

        public async Task<int> CreateSong(CreateSongDAO songDAO)
        {
            Song newSong = _mapper.Map<Song>(songDAO);
            int rowInsert = 0;
            _dbContext.Songs.Add(newSong);
            rowInsert = await _dbContext.SaveChangesAsync();
            return rowInsert;

        }

        /// <summary>
        /// Edit Song
        /// </summary>
        /// <param name="id">Song Id</param>
        /// <param name="songDAO">song DAO</param>
        /// <returns></returns>
        public async Task<int> EditSong(int id, EditSongDAO songDAO)
        {
            //get song by Id
            var mySong = await _dbContext.Songs.Where(s => s.SongId == id && s.IsDeleted == false).FirstOrDefaultAsync();

            var rowUpdate = 0;

            if (mySong != null)
            {
                _mapper.Map(songDAO, mySong);
                _dbContext.Songs.Update(mySong);
                //TODO edit tag for song.
                // _dbContext.SongTags.RemoveRange(_dbContext.SongTags.Where(s => s.SongId == mySong.SongId));
                rowUpdate = await _dbContext.SaveChangesAsync();
            }
            else
            {
                return -1;
            }

            return rowUpdate;
        }


        public async Task<int> AdminEditTag(int id, Song song)
        {
            var getSong = await _dbContext.Songs.Where(s => s.SongId == id && s.IsDeleted == false).FirstOrDefaultAsync();
            var tagUpdate = 0;
            try
            {
                if (getSong != null)
                {
                    getSong.SongTags = song.SongTags;
                    tagUpdate = await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("SOng not exist!");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return tagUpdate;
        }

        public async Task<int> DeactivateSong(int id)
        {
            //check if song exist
            var existedSong = await _dbContext.Songs.Where(s => s.SongId == id && s.IsDeleted == false).FirstOrDefaultAsync();
            if (existedSong == null)
            {
                return -1;
            }
            existedSong.IsDeleted = true;
            var rowDelete = 0;
            rowDelete = await _dbContext.SaveChangesAsync();
            return rowDelete;
        }

        public bool CheckSongExist(int id)
        {
            return _dbContext.Songs.Any(s => s.SongId == id && s.IsDeleted == false);
        }
    }
}
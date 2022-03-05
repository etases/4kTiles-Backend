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
        Task<SongDAO?> GetSongByID(int id);
        Task<SongDAO?> CreateSong(CreateSongDAO songDAO);
        Task<int> EditSong(int id, EditSongDAO songDAO);
        Task<int> DeactivateSong(int id);
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


        public async Task<SongDAO?> GetSongByID(int id)
        {
            var song = await _dbContext.Songs
                .Include(s => s.Creator)
                .Include(s => s.SongGenres)
                .ThenInclude(sg => sg.Genre)
                .Where(s => s.SongId == id)
                .FirstOrDefaultAsync();
            return song == null ? null : _mapper.Map<SongDAO>(song);
        }

        public async Task<SongDAO?> CreateSong(CreateSongDAO songDAO)
        {
            if (await _accountRepository.GetAccountById(songDAO.CreatorId) == null) return null;

            Song newSong = _mapper.Map<Song>(songDAO);
            _dbContext.Songs.Add(newSong);
            await _dbContext.SaveChangesAsync();

            var genres = songDAO.Genres.Select(s => s.ToLower());
            var songGenres = _dbContext.Genres
                .Where(g => genres.Contains(g.GenreName.ToLower()))
                .Select(g => new SongGenre {Song = newSong, Genre = g});
            await _dbContext.SongGenres.AddRangeAsync(songGenres);
            await _dbContext.SaveChangesAsync();

            return await GetSongByID(newSong.SongId);
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
            var mySong = await _dbContext.Songs.Include(s => s.SongGenres).Where(s => s.SongId == id && s.IsDeleted == false).FirstOrDefaultAsync();

            var rowUpdate = 0;

            if (mySong != null)
            {
                if (songDAO.CallerAccountId != mySong.CreatorId) return -2;

                _mapper.Map(songDAO, mySong);
                _dbContext.Songs.Update(mySong);
                mySong.UpdatedDate = DateTime.Now;
                
                _dbContext.SongGenres.RemoveRange(mySong.SongGenres);

                var genres = songDAO.Genres.Select(s => s.ToLower());
                var songGenres = _dbContext.Genres
                    .Where(g => genres.Contains(g.GenreName.ToLower()))
                    .Select(g => new SongGenre { Song = mySong, Genre = g });
                await _dbContext.SongGenres.AddRangeAsync(songGenres);
                await _dbContext.SaveChangesAsync();

                rowUpdate = await _dbContext.SaveChangesAsync();
            }
            else
            {
                return -1;
            }

            return rowUpdate;
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
    }
}
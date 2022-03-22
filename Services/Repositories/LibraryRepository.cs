using System.Linq;
using _4kTiles_Backend.Context;
using _4kTiles_Backend.DataObjects.DAO.Song;
using _4kTiles_Backend.DataObjects.DTO;
using _4kTiles_Backend.DataObjects.DTO.Library;
using _4kTiles_Backend.DataObjects.DTO.Pagination;
using _4kTiles_Backend.Entities;
using _4kTiles_Backend.Helpers;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Services.Repositories
{
    public interface ILibraryRepository
    {
        Task<PaginationResponse<SongDAO>> GetPublicSongs(PaginationParameter pagination);
        Task<PaginationResponse<SongDAO>?> GetPrivateSongs(int id, PaginationParameter pagination);
        Task<PaginationResponse<SongDAO>> GetSongByFilters(LibraryFilterDTO filter, PaginationParameter pagination);
        Task<PaginationResponse<SongDAO>?> GetSongByGenre(string name, PaginationParameter pagination);
        Task<ICollection<string>> GetGenres();

    }
    public class LibraryRepository : ILibraryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        public LibraryRepository(ApplicationDbContext context, IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _context = context;
        }


        /// <summary>
        /// Get public songs
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns>List of public songs</returns>
        public async Task<PaginationResponse<SongDAO>> GetPublicSongs(PaginationParameter pagination)
        {
            var publicSong = await _context.Songs
                .Include(s => s.Creator)
                .Include(s => s.SongGenres)
                .ThenInclude(sg => sg.Genre)
                .Where(s => s.IsPublic == true && s.IsDeleted == false)
                .GetCount(out var count)
                .GetPage(pagination)
                .ToListAsync();
            return new PaginationResponse<SongDAO>()
            {
                Payload = _mapper.Map<IEnumerable<SongDAO>>(publicSong),
                TotalRecords = count
            };
        }

        /// <summary>
        /// Get private songs of user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pagination"></param>
        /// <returns>List of private songs of user</returns>
        public async Task<PaginationResponse<SongDAO>?> GetPrivateSongs(int id, PaginationParameter pagination)
        {
            var account = await _accountRepository.GetAccountById(id);
            if (account == null)
            {
                return null;
            }
            var accountSong = await _context.Songs
                .Include(s => s.Creator)
                .Include(s => s.SongGenres)
                .ThenInclude(sg => sg.Genre)
                .Where(s => s.CreatorId == account.AccountId)
                .Where(s => s.IsDeleted == false)
                .GetCount(out var count)
                .GetPage(pagination)
                .ToListAsync();

            return new PaginationResponse<SongDAO>()
            {
                TotalRecords = count,
                Payload = _mapper.Map<IEnumerable<SongDAO>>(accountSong)
            };
        }

        /// <summary>
        /// Search songs by Filters
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pagination"></param>
        /// <returns>List of public songs satisfied the Filter</returns>
        public async Task<PaginationResponse<SongDAO>> GetSongByFilters(LibraryFilterDTO filter, PaginationParameter pagination)
        {
            var songQ = _context.Songs
                .Include(s => s.Creator)
                .Include(s => s.SongGenres)
                .ThenInclude(sg => sg.Genre)
                .Where(s => s.IsPublic == true && s.IsDeleted == false);
            if (filter.Name != "")
            {
                songQ = songQ.Where(s => s.SongName.ToLower().Contains(filter.Name.Trim().ToLower()));
            }
            // song by SongName

            if (filter.Author != "")
            {
                songQ = songQ.Where(s => s.Author.ToLower().Contains(filter.Author.Trim().ToLower()));
            }
            //song by author

            var result = await songQ.ToListAsync();

            var pagedResult = result.GetCount(out var count).GetPage(pagination);
            return new PaginationResponse<SongDAO>()
            {
                TotalRecords = count,
                Payload = _mapper.Map<IEnumerable<SongDAO>>(pagedResult)
            };
        }

        /// <summary>
        /// Get songs by Genre
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pagination"></param>
        /// <returns>List of songs satisfied the Genre</returns>
        public async Task<PaginationResponse<SongDAO>?> GetSongByGenre(string name, PaginationParameter pagination)
        {
            List<int> list = new();
            var add = await GenreFilter(name);
            if (add != null)
            {
                list.AddRange((IEnumerable<int>)add);
            }
            else
            {
                return null;
            }
            var result = await _context.Songs
                .Include(s => s.Creator)
                .Include(s => s.SongGenres)
                .ThenInclude(sg => sg.Genre)
                .Where(s => s.IsPublic == true && s.IsDeleted == false).ToListAsync();
            result.RemoveAll(s => !list.Contains(s.SongId));

            var pagedResult = result.GetCount(out var count).GetPage(pagination);
            return new PaginationResponse<SongDAO>()
            {
                TotalRecords = count,
                Payload = _mapper.Map<IEnumerable<SongDAO>>(pagedResult)
            };
        }

        /// <summary>
        /// Get all genres
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<string>> GetGenres()
        {
            return await _context.Genres.Select(g => g.GenreName).ToListAsync();
        }

        /// <summary>
        /// get list of song id sastisfied the Genre
        /// </summary>
        /// <param name="genreName"></param>
        /// <returns>List of song id</returns>
        private async Task<List<int>?> GenreFilter(string genreName)
        {
            var genre =await _context.Genres.Where(g => g.GenreName == genreName).FirstOrDefaultAsync();
            if (genre == null)
            {
                return null;
            }
            var songId =await _context.SongGenres.Where(s => s.GenreId == genre.GenreId).Select(s => s.SongId).ToListAsync();
            return songId;
        }
    }
}
using System.Linq;
using _4kTiles_Backend.Context;
using _4kTiles_Backend.DataObjects.DTO;
using _4kTiles_Backend.DataObjects.DTO.LibraryFilterDTO;
using _4kTiles_Backend.Entities;

using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Services.Repositories
{
    public interface ILibraryRepository
    {
        Task<List<Song>?> GetPublicSongs();
        Task<List<Song>?> GetPrivateSongs(int id);
        Task<List<Song>?> GetSongByFilters(LibraryFilterDTO filter);
        Task<List<Song>?> GetSongByGenre(string name);


    }
    public class LibraryRepository : ILibraryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountRepository _accountRepository;
        public LibraryRepository(ApplicationDbContext context, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _context = context;
        }


        /// <summary>
        /// Get public songs
        /// </summary>
        /// <returns>List of public songs</returns>
        public async Task<List<Song>?> GetPublicSongs()
        {
            var publicSong = await _context.Songs.Where(s => s.IsPublic == true).ToListAsync();
            return publicSong;
        }

        /// <summary>
        /// Get private songs of user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of private songs of user</returns>
        public async Task<List<Song>?> GetPrivateSongs(int id)
        {
            var account = await _accountRepository.GetAccountById(id);
            if (account == null)
            {
                return null;
            }
            string name = account.UserName;
            var accountSong = await _context.Songs.Where(s => s.Author == name).ToListAsync();

            return accountSong;
        }

        /// <summary>
        /// Search songs by Filters
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>List of public songs satisfied the Filter</returns>
        public async Task<List<Song>?> GetSongByFilters(LibraryFilterDTO filter)
        {
            var songQ = _context.Songs.Where(s => s.IsPublic == true);
            if (filter.Name != "")
            {
                songQ = songQ.Where(s => s.SongName.ToLower().Equals(filter.Name.Trim().ToLower()));
            }
            // song by SongName

            if (filter.Author != "")
            {
                songQ = songQ.Where(s => s.Author.ToLower().Equals(filter.Author.Trim().ToLower()));
            }
            //song by author

            var result =await songQ.ToListAsync();

            if (filter.Tag != "")
            {
                List<int> list = new List<int>();
                string[] tags = filter.Tag.Split("#");
                foreach (string tag in tags)
                {
                    if (tag != "")
                    {
                        var add = TagFilter(tag);
                        if (add != null)
                        {
                            list.AddRange(add);
                        }

                    }
                }
                //list= list of song id have tags

                result.RemoveAll(s => !list.Contains(s.SongId));
            }
            return result;
        }

        /// <summary>
        /// Get list song id satisfied the Tags
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns>List of song id</returns>
        private async Task<List<int>?> TagFilter(string tagName)
        {
            var tag =await _context.Tags.Where(t => t.TagName.Trim().ToLower().Equals(tagName.Trim().ToLower())).FirstOrDefaultAsync();
            if (tag == null)
            {
                return null;
            }

            var songId =await _context.SongTags.Where(s => s.TagId == tag.TagId).Select(s => s.SongId).ToListAsync();
            return songId;
        }

        /// <summary>
        /// Get songs by Genre
        /// </summary>
        /// <param name="name"></param>
        /// <returns>List of songs sastified the Genre</returns>
        public async Task<List<Song>?> GetSongByGenre(string name)
        {
            List<int> list = new List<int>();
            var add = GenreFilter(name);
            if (add != null)
            {
                list.AddRange((IEnumerable<int>)add);
            }
            else
            {
                return null;
            }
            var result =await _context.Songs.Where(s => s.IsPublic == true).ToListAsync();
            result.RemoveAll(s => !list.Contains(s.SongId));

            return result;
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
            var songByGenre =await _context.SongGenres.Where(s => s.GenreId == genre.GenreId).Select(s => s.SongId).ToListAsync();
            if (songByGenre == null)
            {
                return null;
            }

            var songId =await _context.SongGenres.Where(s => s.GenreId == genre.GenreId).Select(s => s.SongId).ToListAsync();
            return songId;
        }
    }
}
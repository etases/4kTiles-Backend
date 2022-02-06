using System.Linq;
using _4kTiles_Backend.Context;
using _4kTiles_Backend.DataObjects.DTO;
using _4kTiles_Backend.DataObjects.DTO.LibraryFilterDTO;
using _4kTiles_Backend.Entities;

namespace _4kTiles_Backend.Services.Repositories
{
    public interface ILibraryService
    {
        List<Song>? GetPublicSongs();
        List<Song>? GetPrivateSongs(int id);
        List<Song>? GetSongByFilters(LibraryFilterDTO Filter);


    }





    public class LibraryService : ILibraryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountRepository _accountRepository;
        public LibraryService(ApplicationDbContext context, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _context = context;
        }



        List<Song>? ILibraryService.GetPublicSongs()
        {
            var public_song = _context.Songs.Where(s => s.IsPublic == true).ToList();
            return public_song;
        }

        List<Song>? ILibraryService.GetPrivateSongs(int id)
        {
            var account = _accountRepository.getAccountById(id);
            if (account == null)
            {
                return null;
            }
            string name = account.UserName;
            var account_song = _context.Songs.Where(s => s.Author == name).ToList();

            return account_song;
        }

        List<Song>? ILibraryService.GetSongByFilters(LibraryFilterDTO Filter)
        {
            var songQ = _context.Songs.Where(s => s.IsPublic == true);
            if (Filter.name != "")
            {
                songQ = songQ.Where(s => s.SongName.ToLower().Equals(Filter.name.Trim().ToLower()));
            }
            // song by SongName

            if (Filter.author != "")
            {
                songQ = songQ.Where(s => s.Author.ToLower().Equals(Filter.author.Trim().ToLower()));
            }
            //song by author

            var result = songQ.ToList();

            if (Filter.tag != "")
            {
                List<int> list = new List<int>();
                string[] tags = Filter.tag.Split("#");
                int[] tag_songId = new int[] { };
                foreach (string tag in tags)
                {
                    if (tag != "")
                    {
                        var add = TagFilter(tag);
                        if (add != null)
                        {
                            list.AddRange(TagFilter(tag));
                        }

                    }
                }
                //list= list of song id have tags

                result.RemoveAll(s => !list.Contains(s.SongId));
            }
            return result;
        }

        private List<int>? TagFilter(string tagname)
        {
            var tag = _context.Tags.Where(t => t.TagName.Trim().ToLower().Equals(tagname.Trim().ToLower())).FirstOrDefault();
            if (tag == null)
            {
                return null;
            }

            var songid = _context.SongTags.Where(s => s.TagId == tag.TagId).Select(s => s.SongId).ToList();
            return songid;
        }

    }
}
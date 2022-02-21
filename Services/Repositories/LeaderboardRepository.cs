using System.Linq;
using _4kTiles_Backend.Context;
using _4kTiles_Backend.DataObjects.DTO.LeaderboardDTO;
using _4kTiles_Backend.DataObjects.DTO.Pagination;
using _4kTiles_Backend.Entities;
using _4kTiles_Backend.Helpers;

namespace _4kTiles_Backend.Services.Repositories
{
    public interface ILeaderboardRepository
    {
        PaginationResponse<LeaderboardAccountDTO> GetTopLeaderboardBySongId(int songId, PaginationParameter pagination);
        PaginationResponse<LeaderboardUserDTO> GetTopOneByUserId(int accountId, PaginationParameter pagination);
        AccountSong AddUserBestScore(int accountId, int songId, int score);
    }

    public class LeaderboardRepository : ILeaderboardRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaderboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public AccountSong AddUserBestScore(int accountId, int songId, int score)
        {
            AccountSong accountSong = _context.AccountSongs.FirstOrDefault(
                x => x.AccountId == accountId && x.SongId == songId
            );

            if (accountSong == null)
            {
                accountSong = new AccountSong
                {
                    AccountId = accountId,
                    SongId = songId,
                    BestScore = score
                };

                _context.AccountSongs.Add(accountSong);
            }
            else
            {
                accountSong.BestScore = score;
            }

            _context.SaveChanges();

            return accountSong;
        }

        public PaginationResponse<LeaderboardAccountDTO> GetTopLeaderboardBySongId(int songId, PaginationParameter pagination)
        {
            List<LeaderboardAccountDTO> leaderboard =
                _context
                    .AccountSongs
                    .Where(x => x.SongId == songId)
                    .OrderByDescending(x => x.BestScore)
                    .GetCount(out var count)
                    .GetPage(pagination)
                    .Select(song => new LeaderboardAccountDTO
                    {
                        AccountId = song.AccountId,
                        BestScore = song.BestScore
                    })
                    .ToList();

            foreach (var accountSong in leaderboard)
            {
                accountSong.UserName =
                    _context.Accounts
                        .FirstOrDefault(x => x.AccountId == accountSong.AccountId)
                        .UserName;
            }

            return new PaginationResponse<LeaderboardAccountDTO>()
            {
                TotalRecords = count,
                Payload = leaderboard
            };
        }

        public PaginationResponse<LeaderboardUserDTO> GetTopOneByUserId(int accountId, PaginationParameter pagination)
        {
            List<LeaderboardUserDTO> leaderboard =
                _context.AccountSongs.AsEnumerable()
                    .GroupBy(x => x.SongId)
                    .SelectMany(x => x.OrderByDescending(y => y.BestScore).Take(1))
                    .OrderByDescending(x => x.BestScore)
                    .Where(x => x.AccountId == accountId)
                    .GetCount(out var count)
                    .GetPage(pagination)
                    .Select(accountSong => new LeaderboardUserDTO
                    {
                        SongId = accountSong.SongId,
                        AccountId = accountSong.AccountId,
                        BestScore = accountSong.BestScore
                    })
                    .ToList();

            foreach (var accountSong in leaderboard)
            {
                accountSong.SongName =
                    _context
                        .Songs
                        .FirstOrDefault(x =>
                            x.SongId == accountSong.SongId)
                        .SongName;
                accountSong.UserName =
                    _context
                        .Accounts
                        .FirstOrDefault(x =>
                            x.AccountId == accountId)
                        .UserName;
            }
            return new PaginationResponse<LeaderboardUserDTO>
            {
                TotalRecords = count,
                Payload = leaderboard
            };
        }
    }
}

using System.Linq;
using _4kTiles_Backend.Context;
using _4kTiles_Backend.DataObjects.DTO.LeaderboardDTO;
using _4kTiles_Backend.Entities;

namespace _4kTiles_Backend.Services.Repositories
{
    public interface ILeaderboardRepository
    {
        List<LeaderboardAccountDTO>
        GetTopNLeaderboardBySongId(int songId, int n);

        List<LeaderboardUserDTO> GetTopOneByUserId(int accountId);

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

        public List<LeaderboardAccountDTO>
        GetTopNLeaderboardBySongId(int songId, int n)
        {
            List<AccountSong> leaderboard =
                _context
                    .AccountSongs
                    .Where(x => x.SongId == songId)
                    .OrderByDescending(x => x.BestScore)
                    .Take(n)
                    .ToList();

            List<LeaderboardAccountDTO> leaderboardAccount =
                new();

            foreach (AccountSong accountSong in leaderboard)
            {

                LeaderboardAccountDTO leaderboardAccountDTO =
                    new();

                leaderboardAccountDTO.AccountId = accountSong.AccountId;
                leaderboardAccountDTO.UserName =
                    _context.Accounts
                        .FirstOrDefault(x => x.AccountId == accountSong.AccountId)
                        .UserName;
                leaderboardAccountDTO.BestScore = accountSong.BestScore;

                leaderboardAccount.Add(leaderboardAccountDTO);
            }

            return leaderboardAccount;
        }

        public List<LeaderboardUserDTO> GetTopOneByUserId(int accountId)
        {
            List<AccountSong> leaderboard =
                _context.AccountSongs.AsEnumerable()
                    .GroupBy(x => x.SongId)
                    .SelectMany(x => x.OrderByDescending(y => y.BestScore).Take(1))
                    .OrderByDescending(x => x.BestScore)
                    .Where(x => x.AccountId == accountId)
                    .ToList();

            List<LeaderboardUserDTO> leaderboardUser =
                new();

            foreach (AccountSong accountSong in leaderboard)
            {
                LeaderboardUserDTO leaderboardUserDTO =
                    new();

                leaderboardUserDTO.SongId = accountSong.SongId;
                leaderboardUserDTO.SongName =
                    _context
                        .Songs
                        .FirstOrDefault(x =>
                            x.SongId == accountSong.SongId)
                        .SongName;
                leaderboardUserDTO.AccountId = accountId;
                leaderboardUserDTO.UserName =
                    _context
                        .Accounts
                        .FirstOrDefault(x =>
                            x.AccountId == accountId)
                        .UserName;
                leaderboardUserDTO.BestScore = accountSong.BestScore;

                leaderboardUser.Add(leaderboardUserDTO);
            }
            return leaderboardUser;
        }
    }
}

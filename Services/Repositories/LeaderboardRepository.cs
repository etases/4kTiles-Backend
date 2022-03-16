using System.Linq;
using _4kTiles_Backend.Context;
using _4kTiles_Backend.DataObjects.DTO.Leaderboard;
using _4kTiles_Backend.DataObjects.DTO.Pagination;
using _4kTiles_Backend.Entities;
using _4kTiles_Backend.Helpers;

using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Services.Repositories
{
    public interface ILeaderboardRepository
    {
        Task<PaginationResponse<LeaderboardAccountDTO>?> GetTopLeaderboardBySongId(int songId, PaginationParameter pagination);
        Task<PaginationResponse<LeaderboardUserDTO>?> GetTopOneByUserId(int accountId, PaginationParameter pagination);
        Task<AccountSong?> AddUserBestScore(int accountId, int songId, int score);
    }

    public class LeaderboardRepository : ILeaderboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountRepository _accountRepository;
        private readonly ISongRepository _songRepository;

        public LeaderboardRepository(ApplicationDbContext context, IAccountRepository accountRepository, ISongRepository songRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
            _songRepository = songRepository;
        }

        public async Task<AccountSong?> AddUserBestScore(int accountId, int songId, int score)
        {
            if (await _accountRepository.GetAccountById(accountId) == null) return null;
            if (await _songRepository.GetSongByID(songId) == null) return null;

            AccountSong? accountSong = await _context.AccountSongs.FirstOrDefaultAsync(x => x.AccountId == accountId && x.SongId == songId);

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
                accountSong.BestScore = Math.Max(score, accountSong.BestScore);
            }

            await _context.SaveChangesAsync();

            return accountSong;
        }

        public async Task<PaginationResponse<LeaderboardAccountDTO>?> GetTopLeaderboardBySongId(int songId, PaginationParameter pagination)
        {
            if (await _songRepository.GetSongByID(songId) == null) return null;

            var leaderboard =
                await _context
                    .AccountSongs
                    .Where(x => x.SongId == songId)
                    .Include(x => x.Account)
                    .OrderByDescending(x => x.BestScore)
                    .GetCount(out var count)
                    .GetPage(pagination)
                    .Select(song => new LeaderboardAccountDTO
                    {
                        AccountId = song.AccountId,
                        UserName = song.Account.UserName,
                        BestScore = song.BestScore
                    })
                    .ToListAsync();

            return new PaginationResponse<LeaderboardAccountDTO>()
            {
                TotalRecords = count,
                Payload = leaderboard
            };
        }

        public async Task<PaginationResponse<LeaderboardUserDTO>?> GetTopOneByUserId(int accountId, PaginationParameter pagination)
        {
            if (await _accountRepository.GetAccountById(accountId) == null) return null;

            var leaderboard =
                await _context.AccountSongs
                    .Where(x => x.AccountId == accountId)
                    .OrderByDescending(x => x.BestScore)
                    .Include(x => x.Account)
                    .Include(x => x.Song)
                    .GetCount(out var count)
                    .GetPage(pagination)
                    .Select(accountSong => new LeaderboardUserDTO
                    {
                        SongId = accountSong.SongId,
                        SongName = accountSong.Song.SongName,
                        AccountId = accountSong.AccountId,
                        UserName = accountSong.Account.UserName,
                        BestScore = accountSong.BestScore
                    })
                    .ToListAsync();

            return new PaginationResponse<LeaderboardUserDTO>
            {
                TotalRecords = count,
                Payload = leaderboard
            };
        }
    }
}

using AutoMapper;
using _4kTiles_Backend.Entities;
using _4kTiles_Backend.Context;
using _4kTiles_Backend.DataObjects.DAO.Account;
using _4kTiles_Backend.DataObjects.DTO.Email;
using _4kTiles_Backend.Helpers;

using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Services.Repositories
{
    /// <summary>
    /// Account repository interface
    /// </summary>
    public interface IAccountRepository
    {
        Task<AccountDAO?> Login(string email, string password);

        Task<AccountDAO?> CreateAccount(CreateAccountDAO createAccountDAO);

        Task<AccountDAO?> GetAccountByEmail(string email, bool getDeleted = true);

        Task<AccountDAO?> GetAccountById(int id, bool getDeleted = true);

        Task<ICollection<AccountDAO>> GetAllAccounts(bool getDeleted = true);

        Task<ICollection<string>> GetAccountRoleById(int id);

        Task<bool> DeactivateAccount(int id, string message);

        Task<int> UpdateAccount(UpdateAccountDAO updateAccountDAO);

        string CreateNewResetCode(int accountId);

        Task<bool> ResetAccount(int accountId, string resetCode, string newPassword);
    }

    /// <summary>
    /// Account repository implementation
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        private static readonly Dictionary<int, string> ResetCodes = new();
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Account repository constructor
        /// </summary>
        /// <param name="context">ApplicationDbContext</param>
        /// <param name="mapper">AutoMapper</param>
        public AccountRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get account from user credentials
        /// </summary>
        /// <param name="email">the email</param>
        /// <param name="password">the password</param>
        /// <returns>the account</returns>
        public async Task<AccountDAO?> Login(string email, string password)
        {
            Account? account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            if (account == null || !password.VerifyHash(account.HashedPassword)) return null;
            return await GetAccountByEmail(email, false);
        }

        /// <summary>
        /// Create account
        /// </summary>
        /// <param name="createAccountDAO">Account information from user input</param>
        /// <returns>new account</returns>
        public async Task<AccountDAO?> CreateAccount(CreateAccountDAO createAccountDAO)
        {
            // Check if email exists, return null
            if (await GetAccountByEmail(createAccountDAO.Email) is not null) return null;

            // Map & Add account
            Account account = _mapper.Map<Account>(createAccountDAO);
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            // Add roles
            foreach (string role in createAccountDAO.Roles)
            {
                int roleId = await _context.Roles
                    .Where(r => r.RoleName.ToLower().Equals(role.ToLower()))
                    .Select(r => r.RoleId)
                    .FirstOrDefaultAsync();
                if (roleId <= 0) continue;
                _context.AccountRoles.Add(new AccountRole
                {
                    AccountId = account.AccountId,
                    RoleId = roleId
                });
            }

            await _context.SaveChangesAsync();

            // Return account information
            return await GetAccountById(account.AccountId);
        }

        /// <summary>
        /// Find account by email
        /// </summary>
        /// <param name="email">email from user input</param>
        /// <param name="getDeleted">whether we should get the deleted accounts</param>
        /// <returns>account with provided email</returns>
        public async Task<AccountDAO?> GetAccountByEmail(string email, bool getDeleted = true)
        {
            var account = await _context.Accounts
                .Where(a => a.Email == email)
                .Include(a => a.AccountRoles)
                .ThenInclude(ar => ar.Role)
                .FirstOrDefaultAsync(a => getDeleted || a.IsDeleted != true);
            if (account is null) return null;
            AccountDAO accountDAO = _mapper.Map<AccountDAO>(account);
            return accountDAO;
        }

        public async Task<AccountDAO?> GetAccountById(int id, bool getDeleted = true)
        {
            var account = await _context.Accounts
                .Where(a => a.AccountId == id)
                .Include(a => a.AccountRoles)
                .ThenInclude(ar => ar.Role)
                .FirstOrDefaultAsync(a => getDeleted || a.IsDeleted != true);
            if (account is null) return null;
            AccountDAO accountDAO = _mapper.Map<AccountDAO>(account);
            return accountDAO;
        }

        public async Task<ICollection<AccountDAO>> GetAllAccounts(bool getDeleted = true)
        {
            var account = await _context.Accounts
                .Where(a => getDeleted || a.IsDeleted != true)
                .ToListAsync();
            return _mapper.Map<ICollection<AccountDAO>>(account);
        }

        /// <summary>
        /// Get roles from account id
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>role names</returns>
        public async Task<ICollection<string>> GetAccountRoleById(int id)
        {
            return await _context.AccountRoles
                .Where(ar => ar.AccountId == id)
                .Include(ar => ar.Role)
                .Select(ar => ar.Role.RoleName)
                .ToListAsync();
        }

        /// <summary>
        /// Deactivate the account
        /// </summary>
        /// <param name="id">the account id</param>
        /// <param name="message">the deactivate message</param>
        /// <returns>true if the account is deactivated successfully, false if the account doesn't exist</returns>
        public async Task<bool> DeactivateAccount(int id, string message)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account is null) return false;
            account.IsDeleted = true;
            account.DeletedReason = message;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Update the account info
        /// </summary>
        /// <param name="updateAccountDAO">the dao</param>
        /// <returns>-1 if the account id doesn't exist, otherwise return the row updated (0 or 1)</returns>
        public async Task<int> UpdateAccount(UpdateAccountDAO updateAccountDAO)
        {
            var account = await _context.Accounts.FindAsync(updateAccountDAO.AccountId);
            if (account is null) return -1;
            _mapper.Map(updateAccountDAO, account);
            _context.Accounts.Update(account);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Create a new reset code for the account id
        /// </summary>
        /// <param name="accountId">the account id</param>
        public string CreateNewResetCode(int accountId)
        {
            var resetCode = RandomHelper.RandomNumber(8);
            ResetCodes.Add(accountId, resetCode);
            return resetCode;
        }

        /// <summary>
        /// Set the new password for the account
        /// </summary>
        /// <param name="accountId">the account id</param>
        /// <param name="resetCode">the reset code</param>
        /// <param name="newPassword">the new password</param>
        /// <returns>true if successful, false if the reset code is incorrect</returns>
        public async Task<bool> ResetAccount(int accountId, string resetCode, string newPassword)
        {
            string? actualResetCode = ResetCodes.GetValueOrDefault(accountId);
            if (actualResetCode == null || !actualResetCode.Equals(resetCode)) return false;
            ResetCodes.Remove(accountId);

            var account = (await _context.Accounts.FindAsync(accountId))!;
            if (newPassword.VerifyHash(account.HashedPassword)) return true;
            account.HashedPassword = newPassword.Hash();
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

using AutoMapper;
using _4kTiles_Backend.Entities;

namespace _4kTiles_Backend.Context.Repositories
{
    /// <summary>
    /// Account repository interface
    /// </summary>
    public interface IAccountRepository
    {
        Account createUserAccount(Account account);

        Account createAdminAccount(Account account);

        Account getAccountByEmail(string email);

        Account getAccountById(int id);

        Role getAccountRoleById(int id);
    }

    /// <summary>
    /// Account repository implementation
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        // DbContext instance
        private readonly ApplicationDbContext _context;

        // AutoMapper instance
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
        /// Create account
        /// </summary>
        /// <param name="account">Account information from user input</param>
        /// <returns>new account</returns>
        public Account createUserAccount(Account account)
        {
            // add account to DbContext
            _context.Accounts.Add(account);

            // save changes to database
            _context.SaveChanges();

            // assign role to account
            _context
                .AccountRoles
                .Add(new AccountRole
                {
                    AccountId = _context.Accounts.FirstOrDefault(a => a.Email == account.Email).AccountId,
                    RoleId =
                        _context
                            .Roles
                            .FirstOrDefault<Role>(r => r.RoleName == "User")
                            .RoleId
                });

            // save changes to database
            _context.SaveChanges();
            return account;
        }

        /// <summary>
        /// Create account
        /// </summary>
        /// <param name="account">Account information from user input</param>
        /// <returns>new account</returns>
        public Account createAdminAccount(Account account)
        {
            // add account to DbContext
            _context.Accounts.Add(account);

            // save changes to database
            _context.SaveChanges();

            // assign role to account
            _context
                .AccountRoles
                .Add(new AccountRole
                {
                    AccountId = _context.Accounts.FirstOrDefault(a => a.Email == account.Email).AccountId,
                    RoleId =
                        _context
                            .Roles
                            .FirstOrDefault<Role>(r => r.RoleName == "Admin")
                            .RoleId
                });

            // save changes to database
            _context.SaveChanges();
            return account;
        }

        /// <summary>
        /// Find account by email
        /// </summary>
        /// <param name="email">email from user input</param>
        /// <returns>account with provided email</returns>
        public Account getAccountByEmail(string email)
        {
            return _context.Accounts.FirstOrDefault(a => a.Email == email);
        }

        public Account getAccountById(int id)
        {
            return _context.Accounts.FirstOrDefault(a => a.AccountId == id);
        }

        public Role getAccountRoleById(int id)
        {
            AccountRole accountRole = _context.AccountRoles.FirstOrDefault(a => a.AccountId == id);

            return _context.Roles.FirstOrDefault(r => r.RoleId == accountRole.RoleId);
        }
    }
}

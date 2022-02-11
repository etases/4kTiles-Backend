using _4kTiles_Backend.DataObjects.DAO.Account;
using _4kTiles_Backend.DataObjects.DTO.Account;
using _4kTiles_Backend.Entities;
using _4kTiles_Backend.Helpers;

using AutoMapper;

namespace _4kTiles_Backend.Mappers
{
    /// <summary>
    /// Implements the mapping between the Account entity and the AccountDTO.
    /// </summary>
    public class AccountProfile: Profile
    {
        public AccountProfile()
        {
            // Create the mapping between the Account entity and the AccountDTO.
            CreateMap<Account, AccountDAO>()
                .ForMember(dao => dao.Roles, o => o.MapFrom(account => account.AccountRoles.Select(ar => ar.Role.RoleName).Distinct().ToList()));
            CreateMap<CreateAccountDAO, Account>()
                .ForMember(a => a.HashedPassword, o => o.MapFrom(dao => dao.Password.Hash()));
            CreateMap<AccountDAO, AccountDTO>();
            CreateMap<AccountRegisterDTO, CreateAccountDAO>();
        }
    }
}
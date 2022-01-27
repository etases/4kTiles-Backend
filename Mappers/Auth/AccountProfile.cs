using _4kTiles_Backend.DataObjects.DTO.Auth;
using _4kTiles_Backend.Entities;
using AutoMapper;

namespace _4kTiles_Backend.Mappers.Auth
{
    /// <summary>
    /// Implements the mapping between the Account entity and the AccountDTO.
    /// </summary>
    public class AccountProfile: Profile
    {
        public AccountProfile()
        {
            // Create the mapping between the Account entity and the AccountDTO.
            CreateMap<Account, AccountRegisterDTO>();
            CreateMap<AccountRegisterDTO, Account>();
            CreateMap<Account, AccountLoginDTO>();
            CreateMap<AccountLoginDTO, Account>();
        }
    }
}
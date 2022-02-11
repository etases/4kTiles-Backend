using System.ComponentModel.DataAnnotations;

namespace _4kTiles_Backend.DataObjects.DTO.Account
{
    public class AccountUpdateDTO
    {
        public string? UserName { get; set; }
        public DateTime? Dob { get; set; }
    }
}

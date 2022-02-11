using System.ComponentModel.DataAnnotations;

namespace _4kTiles_Backend.DataObjects.DTO.Account
{
    public class AccountResetDTO : AccountLoginDTO
    {
        [Required]
        public string ResetCode { get; set; } = null!;
    }
}

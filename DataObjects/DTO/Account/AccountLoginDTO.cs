using System.ComponentModel.DataAnnotations;

namespace _4kTiles_Backend.DataObjects.DTO.Account
{
    public class AccountLoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        [MaxLength(255)]
        public string Password { get; set; } = null!;
    }
}
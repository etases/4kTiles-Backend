using System.ComponentModel.DataAnnotations;

namespace _4kTiles_Backend.DataObjects.DTO.Account
{
    public class AccountRegisterDTO
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        [MinLength(6)]
        [MaxLength(255)]
        public string Password { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
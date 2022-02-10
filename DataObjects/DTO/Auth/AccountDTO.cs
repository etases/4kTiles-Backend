namespace _4kTiles_Backend.DataObjects.DTO.Auth
{
    public class AccountDTO
    {
        public int AccountId { get; set; }
        public string UserName { get; set; } = null!;
        public DateTime? Dob { get; set; }
        public string Email { get; set; } = null!;
        public ICollection<string> Roles { get; set; } = new List<string>();
    }
}

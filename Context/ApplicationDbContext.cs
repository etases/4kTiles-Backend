using Microsoft.EntityFrameworkCore;

namespace _4kTiles_Backend.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    }
}

using Microsoft.EntityFrameworkCore;

namespace Angular_auth_Api.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)  : base(options)
        { }

        public DbSet<Users> UserTable { get; set; }

    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskFour.Models;

namespace TaskFour.DataLayer
{
    public class UserDbContext : IdentityDbContext<User>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            :base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; } 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

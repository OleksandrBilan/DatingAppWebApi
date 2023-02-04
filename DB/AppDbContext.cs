using DatingApp.DB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.DB
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<City>().HasData(
                new City(1, "Київ"),
                new City(2, "Львів"),
                new City(3, "Івано-Франківськ"),
                new City(4, "Луцьк"),
                new City(5, "Тернопіль")
            );
        }
    }
}

using DatingApp.DB.Models.Chats;
using DatingApp.DB.Models.Locations;
using DatingApp.DB.Models.Questionnaire;
using DatingApp.DB.Models.Recommendations;
using DatingApp.DB.Models.UserRelated;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.DB
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Sex> Sex { get; set; }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserQuestionAnswer> UsersQuestionsAnswers { get; set; }

        public DbSet<UserLike> UsersLikes { get; set; }
        public DbSet<MutualLike> MutualLikes { get; set; }

        public DbSet<UsersChat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Sex>().HasData(
                new Sex { Id = 1, Name = "Else"},
                new Sex { Id = 2, Name = "Male"},
                new Sex { Id = 3, Name = "Female"}
            );

            builder.Entity<Country>().HasData(
                new Country { Code = "XX", Name = "Default Country"}
            );

            builder.Entity<City>().HasData(
                new City { Id = 1, Name = "Default City", CountryCode = "XX" }
            );

            builder.Entity<User>()
                .HasOne(x => x.Sex)
                .WithMany(x => x.UsersWithSuchSex)
                .HasForeignKey(x => x.SexId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);

            builder.Entity<UserQuestionAnswer>().HasKey(x => new { x.UserId, x.QuestionId });
        }
    }
}

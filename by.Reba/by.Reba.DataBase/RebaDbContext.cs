using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.DataBase
{
    public class RebaDbContext : DbContext
    {
        public DbSet<T_Article> Articles { get; set; }
        public DbSet<T_Category> Categories { get; set; }
        public DbSet<T_Comment> Comments { get; set; }
        public DbSet<T_Notification> Notifications { get; set; }
        public DbSet<T_PositivityRating> PositivityRatings { get; set; }
        public DbSet<T_Role> Roles { get; set; }
        public DbSet<T_Source> Sources { get; set; }
        public DbSet<T_User> Users { get; set; }
        public DbSet<T_UserHistory> UserHistory { get; set; }
        public DbSet<T_UserPreference> Preferences { get; set; }

        public RebaDbContext(DbContextOptions<RebaDbContext> options)
            :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T_UserPreference>()
                        .HasMany(up => up.Categories)
                        .WithMany(c => c.UserPreferences)
                        .UsingEntity(j => j.ToTable("UsersPreferences"));

            modelBuilder.Entity<T_Comment>()
                        .HasOne(c => c.ParentComment)
                        .WithMany(c => c.InnerComments)
                        .HasForeignKey(c => c.ParentCommentId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<T_Comment>()
                        .HasMany(c => c.UsersWithPositiveAssessment)
                        .WithMany(u => u.PositiveComments)
                        .UsingEntity(j => j.ToTable("UsersPositiveComments"));

            modelBuilder.Entity<T_Comment>()
                        .HasMany(c => c.UsersWithNegativeAssessment)
                        .WithMany(u => u.NegativeComments)
                        .UsingEntity(j => j.ToTable("UsersNegativeComments"));

            modelBuilder.Entity<T_Article>()
                        .HasMany(a => a.UsersWithPositiveAssessment)
                        .WithMany(u => u.PositiveArticles)
                        .UsingEntity(j => j.ToTable("UsersPositiveArticles"));

            modelBuilder.Entity<T_Article>()
                        .HasMany(a => a.UsersWithNegativeAssessment)
                        .WithMany(u => u.NegativeArticles)
                        .UsingEntity(j => j.ToTable("UsersNegativeArticles"));

            modelBuilder.Entity<T_UserHistory>()
                        .HasKey(uh => new { uh.UserId, uh.ArticleId, uh.LastVisitTime });

        }
    }
}

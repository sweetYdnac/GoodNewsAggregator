using by.Reba.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.API
{
    public class RebaDbContext: DbContext
    {
        public DbSet<T_Role> Roles { get; set; }
        public DbSet<T_User> Users { get; set; }
        public DbSet<T_Article> Articles { get; set; }
        public DbSet<T_Category> Categories { get; set; }
        public DbSet<T_Comment> Comments { get; set; } 
        public DbSet<T_Notification> Notifications { get; set; }
        public DbSet<T_PositivityRating> PositivityRating { get; set; }

        private const string ConnectionString =
            "Server=DESKTOP-L9PR3S2;Database=RebaDb;Trusted_Connection=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T_User>()
                        .HasMany(u => u.History)
                        .WithMany(a => a.UserHistory)
                        .UsingEntity(j => j.ToTable("History"));

            modelBuilder.Entity<T_User>()
                        .HasMany(u => u.Bookmarks)
                        .WithMany(a => a.UserBookmarks)
                        .UsingEntity(j => j.ToTable("Bookmarks"));
            modelBuilder.Entity<T_Comment>()
                        .HasOne(c => c.ParentComment)
                        .WithMany(c => c.InnerComments)
                        .HasForeignKey(c => c.ParentCommentId)
                        .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

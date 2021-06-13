using FindFriend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FindFriend.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Add> Adds { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Adds)
                .WithOne(a => a.Author)
                .HasForeignKey(a => a.AuthorId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Favourites)
                .WithMany(a => a.Likers);
        }
    }
}
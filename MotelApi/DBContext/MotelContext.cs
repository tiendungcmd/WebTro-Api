using Microsoft.EntityFrameworkCore;
using MotelApi.Models;
namespace MotelApi.DBContext
{
    public class MotelContext : DbContext
    {
        public MotelContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ImageHistory> ImageHistories { get; set; }
        public DbSet<ImageMotel> ImageMotels { get; set; }
        public DbSet<Motel> Motels { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<MotelDetail> MotelDetails { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImageHistory>().HasKey(m => new { m.ImageId, m.HistoryId });
            modelBuilder.Entity<ImageMotel>().HasKey(m => new { m.ImageId, m.MotelId });
            modelBuilder.Entity<UserRole>().HasKey(m => new { m.UserId, m.RoleId });
        }
    }
}

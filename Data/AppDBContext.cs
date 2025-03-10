using Attendance.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UserModel> UserTbl {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<UserModel>();
            modelBuilder.Entity<UserModel>().HasData(
                new UserModel {
                    UserId = 1,
                    Fullname = "Kishan Patel",
                    Email = "kish.v07@gmail.com",
                    Password = hasher.HashPassword(null,"Kish.v1710"),
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow
                }
                );
        }
    }
}

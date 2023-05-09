using Lawliet.Models;
using Microsoft.EntityFrameworkCore;

namespace Lawliet.Data {
    public class UserDataContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<LessonTopic> LessonTopics { get; set; }
        public DbSet<StudentTask> StudentTasks { get; set; }

        public UserDataContext() { }
        public UserDataContext(DbContextOptions<UserDataContext> options) : base(options) {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=test;Username=postgres;Password=123456");
        }
    }
}
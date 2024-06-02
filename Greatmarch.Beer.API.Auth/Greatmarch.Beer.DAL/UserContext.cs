using Greatmarch.Beer.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greatmarch.Beer.DAL
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> dbContextOptions) : base(dbContextOptions) { }
        public DbSet<Chat> SettingPrivacies { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>().HaveConversion<DateTime>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(UserConfigure);
            modelBuilder.Entity<Chat>(ChatConfigure);
        }

        public void UserConfigure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.FirstName).HasMaxLength(30).IsRequired();
            builder.Property(p => p.LastName).HasMaxLength(30).IsRequired();
            builder.Property(r => r.Email).HasMaxLength(200).IsRequired();
            builder.Property(r => r.Password).IsRequired();
        }
        public void ChatConfigure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(20);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(100);
        }
    }
}

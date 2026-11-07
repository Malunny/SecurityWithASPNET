using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace SecWithJWTeBearer;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(users =>
        {
            users.ToTable("users");

            users.HasKey(user => user.Token);
            users.Property(user => user.Token)
                .HasColumnName("token");
            users.Property(user => user.Name)
                .IsRequired().HasColumnName("name");
            users.Property(user => user.Email)
                .IsRequired()
                .HasColumnName("email");
            users.Property(user => user.PasswordHash)
                .IsRequired()
                .HasColumnName("password_hash");
        });
    }
}
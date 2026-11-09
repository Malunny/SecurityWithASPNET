using Domain.Role;
using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace SecWithJWTeBearer;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(roles =>
        {
            roles.ToTable("roles");
            
            roles.HasKey(role => role.Token);
            roles.Property(role => role.Token)
                .HasColumnName("token");
            
            roles.Property(role => role.Name)
                .HasColumnName("name");
        });
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

            users.HasMany(user => user.Roles)
                .WithMany(role => role.Users)
                .UsingEntity<Dictionary<string, object>>(
                    ur => ur.HasOne<Role>().WithMany().HasForeignKey("role_token"),
                    ur => ur.HasOne<User>().WithMany().HasForeignKey("user_token"),
                    ur =>
                    {
                        ur.ToTable("user_roles");
                        ur.HasKey("user_token", "role_token");
                    });
        });
    }
}
using Domain.Entities.Roles;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using SecWithJWTeBearer.Mappings;

namespace SecWithJWTeBearer;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

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

        modelBuilder.ApplyConfiguration(new UserMap());
    }
}
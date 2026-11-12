using Domain.Contexts.Account.Entities.Roles;
using Domain.Contexts.Account.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SecWithJWTeBearer.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder.HasKey(user => user.Token);
        builder.Property(user => user.Token)
            .HasColumnName("token");
        
        builder.Property(user => user.Name)
            .IsRequired().HasColumnName("name");
        
        // EMAIL ----------------------------------------
        
        builder.OwnsOne(user => user.Email)
            .Property(email => email.Address)
            .HasColumnName("email")
            .IsRequired();
        
        builder.OwnsOne(user => user.Email)
            .OwnsOne(email => email.Verification)
            .Property(verification => verification.Code)
            .HasColumnName("email_verification_code")
            .IsRequired();

        builder.OwnsOne(user => user.Email)
            .OwnsOne(email => email.Verification)
            .Property(verification => verification.ExpiresAt)
            .HasColumnName("email_verification_expires_at");
        
        builder.OwnsOne(user => user.Email)
            .OwnsOne(email => email.Verification)
            .Property(verification => verification.VerifiedAt)
            .HasColumnName("email_verification_verified_at");

        builder.OwnsOne(user => user.Email)
            .OwnsOne(email => email.Verification)
            .Ignore(verification => verification.IsActive);
        
        // ------------------------------------------------
        
        // PASSWORD ---------------------------------------
        
        builder.OwnsOne(user => user.Password)
            .Property(password => password.Hash)
            .HasColumnName("password_hash")
            .IsRequired();

        builder.OwnsOne(user => user.Password)
            .Property(password => password.ResetCode)
            .HasColumnName("password_reset_code")
            .IsRequired();
        
        // ------------------------------------------------

        builder.HasMany(user => user.Roles)
            .WithMany(role => role.Users)
            .UsingEntity<Dictionary<string, object>>(
                ur => ur.HasOne<Role>().WithMany().HasForeignKey("role_token"),
                ur => ur.HasOne<User>().WithMany().HasForeignKey("user_token"),
                ur =>
                {
                    ur.ToTable("user_roles");
                    ur.HasKey("user_token", "role_token");
                });
    }
}
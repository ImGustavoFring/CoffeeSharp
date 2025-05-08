using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> entity)
        {
            entity.HasKey(admin => admin.Id)
                .HasName("admins_pkey");

            entity.ToTable("admins");

            entity.Property(admin => admin.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(admin => admin.UserName)
                .HasMaxLength(30)
                .HasColumnName("user_name");

            entity.Property(admin => admin.PasswordHash)
                .HasColumnName("password_hash");

            entity.HasIndex(admin => admin.UserName)
                  .IsUnique();
        }
    }
}

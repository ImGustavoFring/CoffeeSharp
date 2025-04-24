using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> entity)
        {
            entity.HasKey(client => client.Id)
                .HasName("clients_pkey");

            entity.ToTable("clients");

            entity.Property(client => client.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(client => client.TelegramId)
                .ValueGeneratedNever()
                .HasColumnName("telegram_id");

            entity.Property(client => client.Balance)
                .HasColumnName("balance");

            entity.Property(client => client.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasIndex(client => client.Name);

            entity.HasIndex(client => client.TelegramId)
                .IsUnique();
        }
    }
}

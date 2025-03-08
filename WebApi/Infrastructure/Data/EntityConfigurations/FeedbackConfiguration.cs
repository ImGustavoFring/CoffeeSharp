using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> entity)
        {
            entity.HasKey(e => e.Id).HasName("feedbacks_pkey");
            entity.ToTable("feedbacks");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ClientId)
                .HasColumnName("clientid");
            entity.Property(e => e.Content)
                .HasColumnName("content");
            entity.Property(e => e.OrderId)
                .HasColumnName("orderid");
            entity.Property(e => e.RatingId)
                .HasColumnName("ratingid");

            entity.HasOne(d => d.Client)
                .WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("feedbacks_clientid_fkey");

            entity.HasOne(d => d.Rating)
                .WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.RatingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("feedbacks_ratingid_fkey");

            entity.HasOne(d => d.Order)
                .WithOne(p => p.Feedback)
                .HasForeignKey<Feedback>(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("feedbacks_orderid_fkey");
        }
    }
}

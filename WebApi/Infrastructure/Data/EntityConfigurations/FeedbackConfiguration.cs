using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> entity)
        {
            entity.HasKey(feedback => feedback.Id)
                .HasName("feedbacks_pkey");

            entity.ToTable("feedbacks");

            entity.Property(feedback => feedback.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(feedback => feedback.Content)
                .HasColumnName("content");

            entity.Property(feedback => feedback.OrderId)
                .HasColumnName("order_id");

            entity.Property(feedback => feedback.RatingId)
                .HasColumnName("rating_id");

            entity.HasOne(feedback => feedback.Rating)
                .WithMany(rating => rating.Feedbacks)
                .HasForeignKey(feedback => feedback.RatingId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("feedbacks_rating_id_fkey");

            entity.HasOne(feedback => feedback.Order)
                .WithOne(order => order.Feedback)
                .HasForeignKey<Feedback>(feedback => feedback.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("feedbacks_order_id_fkey");
        }
    }
}

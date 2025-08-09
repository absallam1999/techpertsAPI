using TechpertsSolutions.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechpertsSolutions.Repository.Data.Configurtaions;

namespace Repository.Data.Configurtaions
{
    public class DeliveryConfiguration : BaseEntityConfiguration<Delivery>
    {
        public override void Configure(EntityTypeBuilder<Delivery> builder)
        {
            base.Configure(builder);

            // One-to-one with Order
            builder.HasOne(d => d.Order)
                   .WithOne(o => o.Delivery)
                   .HasForeignKey<Delivery>(d => d.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Many-to-one with Customer
            builder.HasOne(d => d.Customer)
                   .WithMany(c => c.Deliveries)
                   .HasForeignKey(d => d.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // One-to-many with DeliveryOffers
            builder.HasMany(d => d.Offers)
                   .WithOne(o => o.Delivery)
                   .HasForeignKey(o => o.DeliveryId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Defaults for background service logic
            builder.Property(d => d.RetryCount)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.Property(d => d.CreatedAt)
                   .IsRequired();

            // Indexes for performance
            builder.HasIndex(d => d.Status);
            builder.HasIndex(d => d.CreatedAt);
        }
    }
 }

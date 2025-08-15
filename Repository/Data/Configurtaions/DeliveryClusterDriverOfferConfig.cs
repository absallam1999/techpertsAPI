using Core.Entities;
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
    public class DeliveryClusterDriverOfferConfig : BaseEntityConfiguration<DeliveryClusterDriverOffer>
    {
        public override void Configure(EntityTypeBuilder<DeliveryClusterDriverOffer> builder)
        {
            base.Configure(builder);

            builder.HasOne(o => o.DeliveryCluster)
                   .WithMany(dc => dc.DriverOffers)
                   .HasForeignKey(o => o.DeliveryClusterId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Driver)
                   .WithMany()
                   .HasForeignKey(o => o.DriverId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(o => o.OfferedPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(o => o.Notes)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.HasIndex(o => new { o.DeliveryClusterId, o.DriverId }).IsUnique();
            builder.HasIndex(o => o.DriverId);
        }
    }
}

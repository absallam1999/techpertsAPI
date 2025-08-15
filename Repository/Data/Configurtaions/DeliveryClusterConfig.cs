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
    public class DeliveryClusterConfig : BaseEntityConfiguration<DeliveryCluster>
    {
        public override void Configure(EntityTypeBuilder<DeliveryCluster> builder)
        {
            base.Configure(builder);

            // Relationships
            builder.HasOne(dc => dc.Delivery)
                   .WithMany()
                   .HasForeignKey(dc => dc.DeliveryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(dc => dc.TechCompany)
                   .WithMany()
                   .HasForeignKey(dc => dc.TechCompanyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(dc => dc.AssignedDriver)
                   .WithMany()
                   .HasForeignKey(dc => dc.AssignedDriverId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Indexes for better query performance
            builder.HasIndex(dc => dc.Status);
            builder.HasIndex(dc => dc.AssignedDriverId);
        }
    }
}

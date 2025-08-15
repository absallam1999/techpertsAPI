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
    public class DeliveryOfferConfig: BaseEntityConfiguration<DeliveryOffer>
    {
        public override void Configure(EntityTypeBuilder<DeliveryOffer> builder)
        {
            base.Configure(builder);

            builder.HasOne(o => o.Delivery)
                   .WithMany(d => d.Offers)
                   .HasForeignKey(o => o.DeliveryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.DeliveryPerson)
                   .WithMany()
                   .HasForeignKey(o => o.DeliveryPersonId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(o => o.ExpiryTime);
        }
    }
}

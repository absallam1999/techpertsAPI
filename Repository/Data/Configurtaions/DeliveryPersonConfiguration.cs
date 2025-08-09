using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechpertsSolutions.Core.Entities;
using TechpertsSolutions.Repository.Data.Configurtaions;

namespace Repository.Data.Configurtaions
{
    public class DeliveryPersonConfig : BaseEntityConfiguration<DeliveryPerson>
    {
        public override void Configure(EntityTypeBuilder<DeliveryPerson> builder)
        {
            base.Configure(builder);

            // One-to-one with AppUser
            builder.HasOne(dp => dp.User)
                   .WithOne(u => u.DeliveryPerson)
                   .HasForeignKey<DeliveryPerson>(dp => dp.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Many-to-one with Role
            builder.HasOne(dp => dp.Role)
                   .WithMany()
                   .HasForeignKey(dp => dp.RoleId)
                   .OnDelete(DeleteBehavior.Restrict);

            // One-to-many with Deliveries (keep history if driver deleted)
            builder.HasMany(dp => dp.Deliveries)
                   .WithOne(d => d.DeliveryPerson)
                   .HasForeignKey(d => d.DeliveryPersonId)
                   .OnDelete(DeleteBehavior.SetNull);

            // One-to-many with DeliveryOffers (restrict to keep history)
            builder.HasMany<DeliveryOffer>()
                   .WithOne(o => o.DeliveryPerson)
                   .HasForeignKey(o => o.DeliveryPersonId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}

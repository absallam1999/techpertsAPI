﻿using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Configurtaions
{
    public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.HasMany(d => d.TechCompanies)
                    .WithMany(t => t.Deliveries);


            builder.HasMany(d => d.Orders)
                    .WithOne(o => o.Delivery)
                    .HasForeignKey(o => o.DeliveryId);

            builder.HasMany(d => d.Customers)
                   .WithOne(c => c.Delivery)
                   .HasForeignKey(c => c.DeliveryId);



        }
    }
}

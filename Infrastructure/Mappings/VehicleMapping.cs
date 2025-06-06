﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using challenge.Domain.Entity;

namespace challenge.Infra.Data.Mappings
{
    public class VehicleMapping : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");
            builder.HasKey(v => v.VehicleId);

            builder.Property(v => v.VehicleId)
                   .IsRequired();

            builder.Property(v => v.LicensePlate)
                   .IsRequired()
                   .HasMaxLength(8);

            builder.Property(v => v.VehicleModel)
                   .IsRequired();

            // Aqui está a correção necessária
            builder.Property(v => v.IsCancel)
                   .HasColumnType("NUMBER(1)") // Define explicitamente o tipo correto para Oracle
                   .IsRequired();

            builder.Property(v => v.UserCancelID)
                   .IsRequired();
        }
    }
}
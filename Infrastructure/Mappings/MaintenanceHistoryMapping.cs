﻿using challenge.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace challenge.Infrastructure.Mappings
{
    /// <summary>
    /// Mapeamento da entidade MaintenanceHistory para o banco de dados.
    /// </summary>
    public class MaintenanceHistoryMapping : IEntityTypeConfiguration<MaintenanceHistory>
    {
        public void Configure(EntityTypeBuilder<MaintenanceHistory> builder)
        {
            builder.ToTable("MaintenanceHistories");


            builder.HasKey(m => m.MaintenanceHistoryID);

            builder.Property(m => m.MaintenanceHistoryID)
                .IsRequired();

            builder.Property(m => m.VehicleID)
                .IsRequired();

            builder.Property(m => m.UserID)
                .IsRequired();

            builder.Property(m => m.MaintenanceDate)
                .IsRequired();

            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(500);

        }
    }
}

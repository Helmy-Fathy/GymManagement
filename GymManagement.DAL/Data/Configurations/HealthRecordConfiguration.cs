using GymManagement.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Configurations
{
    public class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.Property(H => H.Weight)
                   .HasPrecision(10, 2);

            builder.Property(H => H.Height)
                   .HasPrecision(10, 2);

            builder.Property(H => H.BloodType)
                   .HasMaxLength(5);

            builder.Property(H => H.Note)
                   .HasMaxLength(500);

            builder.Property(H => H.UpdatedAt)
                   .HasColumnName("LastUpdated");
        }
    }
}

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
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(U => U.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(U => U.Email)
                   .HasColumnType("varchar")
                   .HasMaxLength(100);

            builder.HasIndex(U => U.Email).IsUnique();
            builder.HasIndex(U => U.PhoneNumber).IsUnique();

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("EmailCheck", "Email like '_%@_%._%'");
                tb.HasCheckConstraint("PhoneCheck", "PhoneNumber like '010%' or PhoneNumber like '011%' or PhoneNumber like '015%' or PhoneNumber like '012%'");
            });

            builder.OwnsOne(U => U.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("Street").HasColumnType("varchar").HasMaxLength(30);
                address.Property(a => a.City).HasColumnName("City").HasColumnType("varchar").HasMaxLength(30);
            });
        } 
    }

}

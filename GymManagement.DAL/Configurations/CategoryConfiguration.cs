using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(C => C.CategryName)
                   .HasColumnType("varchar")
                   .HasMaxLength(20);

            builder.Property(C => C.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasData(
                new Category { Id = 1, CategryName = "Cardio" },
                new Category { Id = 2, CategryName = "Strength" },
                new Category { Id = 3, CategryName = "Yoga" },
                new Category { Id = 4, CategryName = "Boxing" },
                new Category { Id = 5, CategryName = "CrossFit" }
            );
        }
    }
}

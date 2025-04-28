using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(si => si.Id);
        builder.Property(si => si.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(si => si.Quantity)
            .IsRequired();

        builder.Property(si => si.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(si => si.Discount)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(si => si.TotalPrice)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.HasOne(si => si.Item)
            .WithMany()
            .HasForeignKey(si => si.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 
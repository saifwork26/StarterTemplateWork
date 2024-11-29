﻿using Inventory.Domain.Features.CategoryGroup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Features.Category;
internal sealed class CategoryGroupConfig : IEntityTypeConfiguration<CategoryGroupM>
{
    public void Configure(EntityTypeBuilder<CategoryGroupM> builder)
    {
        builder.HasKey(e => e.CategoryGroupId);

        builder.Property(e => e.CategoryGroupCode)
            .HasMaxLength(12)
            .IsRequired();

        builder.HasIndex(e => e.CategoryGroupCode)
            .IsUnique();

        builder.Property(e => e.CategoryGroupDesc)
            .HasMaxLength(50);

        builder.Property(e => e.IsActive)
            .IsRequired();
    }

}
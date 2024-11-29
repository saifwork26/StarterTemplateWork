﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Infrastructure.Outbox;

public sealed class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder), "EntityTypeBuilder cannot be null");
        }

        builder.ToTable("OutboxMessageConsumers");

        builder.HasKey(o => new { o.OutboxMessageId, o.Name });

        builder.Property(o => o.Name).HasMaxLength(500);
    }
}
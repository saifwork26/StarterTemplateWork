﻿using Common.Application.Database;
using Common.Application.Messaging;
using Common.Domain.Abstractions;
using Common.Infrastructure.Outbox;
using Dapper;
using System.Data.Common;

namespace Parent.Infrastructure.Common.Outbox;

internal sealed class IdempotentDomainEventHandler<TDomainEvent>(
        IDomainEventHandler<TDomainEvent> decorated,
        IDbConnectionFactory _dbConnectionFactory)
        : DomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
{
    public override async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenParentConnectionAsync();

        OutboxMessageConsumer outboxMessageConsumer = new(domainEvent.Id, decorated.GetType().Name);

        if (await OutboxConsumerExistsAsync(connection, outboxMessageConsumer))
        {
            return;
        }

        await decorated.Handle(domainEvent, cancellationToken);

        await InsertOutboxConsumerAsync(connection, outboxMessageConsumer);
    }

    private static async Task<bool> OutboxConsumerExistsAsync(
            DbConnection dbConnection,
            OutboxMessageConsumer outboxMessageConsumer)
    {
        const string sql =
            $"""
                SELECT 1
                FROM {Constants.Schema}.OutboxMessageConsumers
                WHERE OutboxMessageId = @OutboxMessageId AND
                        Name = @Name
            """;

        return await dbConnection.ExecuteScalarAsync<bool>(sql, outboxMessageConsumer);
    }

    private static async Task InsertOutboxConsumerAsync(
            DbConnection dbConnection,
            OutboxMessageConsumer outboxMessageConsumer)
    {
        const string sql =
            $"""
                INSERT INTO {Constants.Schema}.OutboxMessageConsumers(OutboxMessageId, Name)
                VALUES (@OutboxMessageId, @Name)
            """;

        await dbConnection.ExecuteAsync(sql, outboxMessageConsumer);
    }
}
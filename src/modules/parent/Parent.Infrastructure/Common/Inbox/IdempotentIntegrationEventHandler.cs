﻿using Common.Application.Database;
using Common.Application.EventBus;
using Common.Infrastructure.Inbox;
using Dapper;
using System.Data.Common;

namespace Parent.Infrastructure.Common.Inbox;

internal sealed class IdempotentIntegrationEventHandler<TIntegrationEvent>(
        IIntegrationEventHandler<TIntegrationEvent> decorated,
        IDbConnectionFactory _dbConnectionFactory)
        : IntegrationEventHandler<TIntegrationEvent>
        where TIntegrationEvent : IIntegrationEvent
{
    public override async Task Handle(
            TIntegrationEvent integrationEvent,
            CancellationToken cancellationToken = default)
    {
        using DbConnection connection = _dbConnectionFactory.GetConnection();

        InboxMessageConsumer inboxMessageConsumer = new(integrationEvent.Id, decorated.GetType().Name);

        if (await InboxConsumerExistsAsync(connection, inboxMessageConsumer))
        {
            return;
        }

        await decorated.Handle(integrationEvent, cancellationToken);

        await InsertInboxConsumerAsync(connection, inboxMessageConsumer);
    }

    private static async Task<bool> InboxConsumerExistsAsync(
            DbConnection dbConnection,
            InboxMessageConsumer inboxMessageConsumer)
    {
        const string sql =
           $"""
                SELECT 1
                FROM {Constants.Schema}.InboxMessageConsumers
                WHERE InboxMessageId = @InboxMessageId AND
                      Name = @Name
            """;

        return await dbConnection.ExecuteScalarAsync<bool>(sql, inboxMessageConsumer);
    }

    private static async Task InsertInboxConsumerAsync(
            DbConnection dbConnection,
            InboxMessageConsumer inboxMessageConsumer)
    {
        const string sql =
            $"""
                INSERT INTO 
                {Constants.Schema}.InboxMessageConsumers(InboxMessageId, Name)
                VALUES (@InboxMessageId, @Name)
            """;

        await dbConnection.ExecuteAsync(sql, inboxMessageConsumer);
    }
}

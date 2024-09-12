﻿using Example1.Domain.Abstractions.CQRS;
using Example1.Domain.Abstractions.CQRS.Command;
using Example1.Domain.Abstractions.CQRS.Query;
using MediatR;
using Microsoft.Extensions.Logging;
using TBotPlatform.Extension;

namespace Example1.Application.Templates;

internal class SenderRun(
    ILogger<SenderRun> logger,
    IMediator mediator
    ) : ISenderRun
{
    async Task<TResponse> ISenderRun.SendAsync<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        LogStart(request);

        try
        {
            return await mediator.Send(request, cancellationToken);
        }
        catch (Exception ex)
        {
            LogError(ex, request);
            throw;
        }
    }

    async Task<TResponse> ISenderRun.SendAsync<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        LogStart(request);

        try
        {
            return await mediator.Send(request, cancellationToken);
        }
        catch (Exception ex)
        {
            LogError(ex, request);
            throw;
        }
    }

    async Task ISenderRun.SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        LogStart(request);

        try
        {
            await mediator.Send(request, cancellationToken);
        }
        catch (Exception ex)
        {
            LogError(ex, request);
            throw;
        }
    }

    private void LogStart(object request)
    {
        logger.LogDebug("Сообщение для {senderRun} типа {requestType}: {request}", nameof(SenderRun), request.GetType().FullName, request.ToJson());
    }

    private void LogError(Exception ex, object request)
    {
        logger.LogError(ex, "Ошибка в {senderRun} типа {requestType}: {request}", nameof(SenderRun), request.GetType().FullName, request.ToJson());
    }
}
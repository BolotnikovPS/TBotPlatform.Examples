using MediatR;

namespace Example1.Domain.Abstractions.CQRS.Query;

public interface IQueryHandler<in TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IQuery<TResponse>;
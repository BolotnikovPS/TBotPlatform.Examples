using MediatR;

namespace Example1.Domain.Abstractions.CQRS.Query;

public interface IQuery<out TResponse>
    : IRequest<TResponse>;
using MediatR;

namespace Example1.Domain.Abstractions.CQRS.Command;

public interface ICommand
    : IRequest;

public interface ICommand<out TResponse>
    : IRequest<TResponse>;
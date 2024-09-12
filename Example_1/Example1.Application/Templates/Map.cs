﻿using Example1.Domain.Abstractions;
using MapsterMapper;

namespace Example1.Application.Templates;

internal class Map(
    IMapper mapper
    ) : IMap
{
    TDestination IMap.Map<TDestination>(object source)
        => mapper.Map<TDestination>(source);

    TDestination IMap.Map<TSource, TDestination>(TSource source)
        => mapper.Map<TSource, TDestination>(source);

    TDestination IMap.Map<TSource, TDestination>(TSource source, TDestination destination)
        => mapper.Map(source, destination);

    object IMap.Map(object source, Type sourceType, Type destinationType)
        => mapper.Map(source, sourceType, destinationType);

    object IMap.Map(object source, object destination, Type sourceType, Type destinationType)
        => mapper.Map(source, destination, sourceType, destinationType);
}
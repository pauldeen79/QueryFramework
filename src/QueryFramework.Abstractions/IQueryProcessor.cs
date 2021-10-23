﻿using System.Collections.Generic;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.Abstractions
{
    public interface IQueryProcessor<in TQuery, out TResult>
    {
        TResult FindOne(TQuery query);
        IReadOnlyCollection<TResult> FindMany(TQuery query);
        IQueryResult<TResult> FindPaged(TQuery query);
    }

    public interface IQueryProcessor<out TResult> : IQueryProcessor<ISingleEntityQuery, TResult>
    {
    }
}

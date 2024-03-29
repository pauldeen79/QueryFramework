﻿namespace QueryFramework.InMemory.Tests.TestHelpers;

public class DataProviderMock : IDataProvider
{
    public bool ReturnValue { get; set; }
    public Func<IQuery, IEnumerable?> ResultDelegate { get; set; } = new Func<IQuery, IEnumerable?>(_ => Enumerable.Empty<object>());

    public bool TryGetData<TResult>(IQuery query, out IEnumerable<TResult>? result) where TResult : class
    {
        result = ResultDelegate.Invoke(query)?.Cast<TResult>();
        return ReturnValue;
    }
}

public class ContextDataProviderMock : IContextDataProvider
{
    public bool ReturnValue { get; set; }
    public Func<IQuery, object?, IEnumerable?> ContextResultDelegate { get; set; } = new Func<IQuery, object?, IEnumerable?>((_, _) => Enumerable.Empty<object>());

    public bool TryGetData<TResult>(IQuery query, out IEnumerable<TResult>? result)
        where TResult : class
        => TryGetData(query, default, out result);

    public bool TryGetData<TResult>(IQuery query, object? context, out IEnumerable<TResult>? result) where TResult : class
    {
        result = ContextResultDelegate.Invoke(query, context)?.Cast<TResult>();
        return ReturnValue;
    }
}

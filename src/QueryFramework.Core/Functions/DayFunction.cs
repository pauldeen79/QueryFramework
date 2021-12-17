﻿using QueryFramework.Abstractions;

namespace QueryFramework.Core.Functions
{
    public record DayFunction : IQueryExpressionFunction
    {
        public DayFunction() { }

        public DayFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
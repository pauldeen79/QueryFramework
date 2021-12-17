﻿using QueryFramework.Abstractions;

namespace QueryFramework.Core.Functions
{
    public record MonthFunction : IQueryExpressionFunction
    {
        public MonthFunction() { }

        public MonthFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
﻿namespace ExpressionFramework.Core.FunctionEvaluators;

public class LowerFunctionEvaluator : IFunctionEvaluator
{
    public bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluatorCallback callback, out object? result)
    {
        if (!(function is LowerFunction))
        {
            result = null;
            return false;
        }

        result = value == null
            ? string.Empty
            : value.ToString().ToLowerInvariant();
        return true;
    }
}

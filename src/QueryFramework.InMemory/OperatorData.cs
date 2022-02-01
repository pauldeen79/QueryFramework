﻿namespace QueryFramework.InMemory;

internal record OperatorData
{
    public object? Value { get; }
    public string? ConditionValueString { get; }
    public object? ConditionValue { get; }

    public OperatorData(object? value, string? conditionValueString, object? conditionValue)
    {
        Value = value;
        ConditionValueString = conditionValueString;
        ConditionValue = conditionValue;
    }
}

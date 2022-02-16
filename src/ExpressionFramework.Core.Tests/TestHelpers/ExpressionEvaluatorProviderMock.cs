namespace ExpressionFramework.Core.Tests.TestHelpers;

public class ExpressionEvaluatorProviderMock : IExpressionEvaluatorProvider
{
    public Func<object?, IExpression, IExpressionEvaluator, Tuple<bool, object?>> Delegate { get; set; }
        = new Func<object?, IExpression, IExpressionEvaluator, Tuple<bool, object?>>((_, _, _) => new Tuple<bool, object?>(default, default));

    public bool TryEvaluate(object? item, IExpression expression, IExpressionEvaluator evaluator, out object? result)
    {
        var x = Delegate.Invoke(item, expression, evaluator);

        result = x.Item2;
        return x.Item1;
    }
}

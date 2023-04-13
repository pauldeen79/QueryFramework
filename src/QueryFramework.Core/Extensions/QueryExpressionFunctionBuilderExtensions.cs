namespace QueryFramework.Core.Extensions;

public static class QueryExpressionFunctionBuilderExtensions
{
    //public static T WithInnerFunction<T>(this T instance, ExpressionBuilder currentExpression)
    //    where T : ExpressionFunctionBuilder
    //    => instance.With(x => x.InnerFunction = currentExpression.GetFunction()?.ToBuilder());

    //public static ComposableEvaluatableBuilder WithCombination(this EvaluatableBuilder instance, Combination combination)
    //    => instance switch
    //    {
    //        ComposableEvaluatableBuilder c => c.WithCombination(combination),
    //        SingleEvaluatableBuilder s => new ComposableEvaluatableBuilder().WithCombination(combination).WithLeftExpression(s.LeftExpression).WithOperator(s.Operator).WithRightExpression(s.RightExpression),
    //        _ => throw new ArgumentException($"Unsupported evaluatable type: [{instance.GetType().FullName}]")
    //    };

    //public static ComposableEvaluatableBuilder WithStartGroup(this EvaluatableBuilder instance, bool startGroup = true)
    //    => instance switch
    //    {
    //        ComposableEvaluatableBuilder c => c.WithStartGroup(startGroup),
    //        SingleEvaluatableBuilder s => new ComposableEvaluatableBuilder().WithStartGroup(startGroup).WithLeftExpression(s.LeftExpression).WithOperator(s.Operator).WithRightExpression(s.RightExpression),
    //        _ => throw new ArgumentException($"Unsupported evaluatable type: [{instance.GetType().FullName}]")
    //    };

    //public static ComposableEvaluatableBuilder WithEndGroup(this EvaluatableBuilder instance, bool endGroup = true)
    //    => instance switch
    //    {
    //        ComposableEvaluatableBuilder c => c.WithEndGroup(endGroup),
    //        SingleEvaluatableBuilder s => new ComposableEvaluatableBuilder().WithEndGroup(endGroup).WithLeftExpression(s.LeftExpression).WithOperator(s.Operator).WithRightExpression(s.RightExpression),
    //        _ => throw new ArgumentException($"Unsupported evaluatable type: [{instance.GetType().FullName}]")
    //    };
}

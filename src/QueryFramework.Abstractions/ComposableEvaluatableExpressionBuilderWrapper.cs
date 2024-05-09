namespace QueryFramework.Abstractions;

public class ComposableEvaluatableExpressionBuilderWrapper<T> : ComposableEvaluatableBuilderWrapperBase<T>
    where T : IQueryBuilder
{
    public ComposableEvaluatableExpressionBuilderWrapper(T instance, ExpressionBuilder leftExpression, Combination? combination = null) : base(instance, leftExpression, combination)
    {
    }

    public ComposableEvaluatableExpressionBuilderWrapper<T> WithStartGroup(bool startGroup = true)
    {
        StartGroup = startGroup;
        return this;
    }

    public ComposableEvaluatableExpressionBuilderWrapper<T> WithEndGroup(bool endGroup = true)
    {
        EndGroup = endGroup;
        return this;
    }

    public ComposableEvaluatableExpressionBuilderWrapper<T> WithCombination(Combination combination)
    {
        Combination = combination;
        return this;
    }
}

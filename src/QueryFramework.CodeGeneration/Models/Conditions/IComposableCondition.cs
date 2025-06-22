namespace QueryFramework.CodeGeneration.Models.Conditions;

internal interface IComposableCondition : ICondition
{
    StringComparison StringComparison { get; set; }

    Abstractions.IExpression LeftExpression { get; set; }
    IOperator Operator { get; set; }
    Abstractions.IExpression RightExpression { get; set; }

    Combination? Combination { get; set; }
    bool StartGroup { get; set; }
    bool EndGroup { get; set; }
}

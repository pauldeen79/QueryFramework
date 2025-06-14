namespace QueryFramework.CodeGeneration.Models.Abstractions;

internal interface ICondition
{
    StringComparison StringComparison { get; set; }

    IEvaluatable LeftExpression { get; set; }
    IOperator Operator { get; set; }
    IEvaluatable RightExpression { get; set; }

    Combination? Combination { get; set; }
    bool StartGroup { get; set; }
    bool EndGroup { get; set; }
}

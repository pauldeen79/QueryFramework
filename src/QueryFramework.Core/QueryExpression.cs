using QueryFramework.Abstractions;

namespace QueryFramework.Core
{
    public sealed record QueryExpression : IQueryExpression, IExpressionContainer
    {
        private readonly string _expression;

        public QueryExpression(string fieldName, string expression = null)
        {
            FieldName = fieldName;
            _expression = expression;
        }

        public string FieldName { get; }

        public string Expression => _expression == null
            ? FieldName
            : string.Format(_expression, FieldName);

        string IExpressionContainer.SourceExpression => _expression;

        public override string ToString() => Expression;
    }
}

using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Core.Builders
{
    public class QueryExpressionBuilder : IQueryExpressionBuilder
    {
        public string FieldName { get; set; }
        public string Expression { get; set; }
        public IQueryExpression Build()
        {
            return new QueryExpression(FieldName, Expression);
        }
        public QueryExpressionBuilder() : this(null)
        {
        }
        public QueryExpressionBuilder(IQueryExpression source)
        {
            if (source != null)
            {
                FieldName = source.FieldName;
                Expression = !(source is IExpressionContainer expressionContainer)
                    ? source.Expression
                    : expressionContainer.SourceExpression;
            }
        }
        public QueryExpressionBuilder(string fieldName, string expression = null)
        {
            FieldName = fieldName;
            Expression = expression;
        }
    }
}

using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Core.Builders
{
    public class QueryExpressionBuilder : IQueryExpressionBuilder
    {
        public string FieldName { get; set; }
        public IQueryExpressionFunction? Function { get; set; }
        public IQueryExpression Build()
        {
            return new QueryExpression(FieldName, Function);
        }
        public QueryExpressionBuilder()
        {
            FieldName = string.Empty;
        }
        public QueryExpressionBuilder(IQueryExpression source) : this(source.FieldName, source.Function)
        {
        }
        public QueryExpressionBuilder(string fieldName) : this(fieldName, null)
        {
        }
        public QueryExpressionBuilder(string fieldName, IQueryExpressionFunction? function)
        {
            FieldName = fieldName;
            Function = function;
        }
    }
}

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
        public QueryExpressionBuilder() : this(null)
        {
        }
        public QueryExpressionBuilder(IQueryExpression? source)
        {
            if (source != null)
            {
                FieldName = source.FieldName;
                Function = source.Function;
            }
            else
            {
                FieldName = string.Empty;
                Function = null;
            }
        }
        public QueryExpressionBuilder(string fieldName, IQueryExpressionFunction? function = null)
        {
            FieldName = fieldName;
            Function = function;
        }
    }
}

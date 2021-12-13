using System;
using QueryFramework.Abstractions;

namespace QueryFramework.Core
{
    public sealed record QueryExpression : IQueryExpression
    {
        public QueryExpression(string fieldName, IQueryExpressionFunction? function = null)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentException("FieldName must have at leat one non-space character", nameof(fieldName));
            }
            FieldName = fieldName;
            Function = function;
        }

        public string FieldName { get; }

        public IQueryExpressionFunction? Function { get; }

        public override string ToString() => Function == null
            ? FieldName
            : $"[{Function.GetType().Name}({FieldName})]";
    }
}

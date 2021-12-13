using System;
using System.Collections.Generic;
using System.Linq;
using QueryFramework.Abstractions;

namespace QueryFramework.Core.Functions
{
    public record CoalesceFunction : IQueryExpression, IQueryExpressionFunction
    {
        public CoalesceFunction(string? fieldName, IQueryExpressionFunction? innerFunction, params IQueryExpression[] innerExpressions)
        {
            if (!innerExpressions.Any() && fieldName == null)
            {
                throw new ArgumentException("There must be at least one inner expression", nameof(innerExpressions));
            }
            FieldName = fieldName ?? string.Empty;
            InnerFunction = innerFunction;
            InnerExpressions = innerExpressions;
        }

        public CoalesceFunction(string fieldName, IQueryExpressionFunction? innerFunction, IEnumerable<IQueryExpression> innerExpressions) : this(fieldName, innerFunction, innerExpressions.ToArray())
        {
        }

        public IQueryExpressionFunction? InnerFunction { get; }

        public IEnumerable<IQueryExpression> InnerExpressions { get; }

        public string FieldName { get; }

        public IQueryExpressionFunction? Function => this;
    }
}

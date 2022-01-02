using System;
using System.Collections.Generic;
using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Core.Functions
{
    public record CoalesceFunction : IQueryExpression, IQueryExpressionFunction
    {
        public CoalesceFunction(string fieldName,
                                IQueryExpressionFunction? innerFunction,
                                params IQueryExpression[] innerExpressions)
        {
            if (!innerExpressions.Any() && string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentException("There must be at least one inner expression", nameof(innerExpressions));
            }
            FieldName = fieldName;
            InnerFunction = innerFunction;
            InnerExpressions = innerExpressions;
        }

        public CoalesceFunction(string fieldName,
                                IQueryExpressionFunction? innerFunction,
                                IEnumerable<IQueryExpression> innerExpressions)
            : this(fieldName, innerFunction, innerExpressions.ToArray())
        {
        }

        public IQueryExpressionFunction? InnerFunction { get; }

        public IEnumerable<IQueryExpression> InnerExpressions { get; }

        public string FieldName { get; }

        public IQueryExpressionFunction? Function => this;
    }

    public class CoalesceFunctionBuilder : IQueryExpressionBuilder
    {
        public string FieldName { get; set; }
        public IQueryExpressionFunction? Function { get; set; }
        public List<IQueryExpressionBuilder> InnerExpressions { get; set; }

        public IQueryExpression Build()
        {
            return new CoalesceFunction(FieldName, Function, InnerExpressions.Select(x => x.Build()));
        }

        public CoalesceFunctionBuilder()
        {
            FieldName = string.Empty;
            InnerExpressions = new List<IQueryExpressionBuilder>();
        }

        public CoalesceFunctionBuilder WithFieldName(string fieldName)
        {
            FieldName = fieldName;
            return this;
        }

        public CoalesceFunctionBuilder WithFunction(IQueryExpressionFunction? function)
        {
            Function = function;
            return this;
        }

        public CoalesceFunctionBuilder AddInnerExpressions(params IQueryExpressionBuilder[] innerExpressions)
        {
            InnerExpressions.AddRange(innerExpressions);
            return this;
        }

        public CoalesceFunctionBuilder AddInnerExpressions(IEnumerable<IQueryExpressionBuilder> innerExpressions)
        {
            InnerExpressions.AddRange(innerExpressions);
            return this;
        }
    }
}

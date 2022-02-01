using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common.Extensions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Builders;

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

        public IQueryExpressionFunctionBuilder ToBuilder()
            => new CoalesceFunctionBuilder
            {
                FieldName = FieldName,
                InnerFunction = InnerFunction?.ToBuilder(),
                InnerExpressions = InnerExpressions.Select(x => new QueryExpressionBuilder(x)).Cast<IQueryExpressionBuilder>().ToList()
            };
    }

    public class CoalesceFunctionBuilder : IQueryExpressionBuilder, IQueryExpressionFunctionBuilder
    {
        public string FieldName { get; set; }
        public IQueryExpressionFunctionBuilder? Function { get; set; }
        public List<IQueryExpressionBuilder> InnerExpressions { get; set; }
        public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

        public IQueryExpression Build()
            => new CoalesceFunction(FieldName, Function?.Build(), InnerExpressions.Select(x => x.Build()));

        public CoalesceFunctionBuilder()
        {
            FieldName = string.Empty;
            InnerExpressions = new List<IQueryExpressionBuilder>();
        }

        public CoalesceFunctionBuilder WithFieldName(string fieldName)
            => this.Chain(x => x.FieldName = fieldName);

        public CoalesceFunctionBuilder WithFunction(IQueryExpressionFunctionBuilder? function)
            => this.Chain(x => x.Function = function);

        public CoalesceFunctionBuilder AddInnerExpressions(params IQueryExpressionBuilder[] innerExpressions)
            => this.Chain(x => x.InnerExpressions.AddRange(innerExpressions));

        public CoalesceFunctionBuilder AddInnerExpressions(IEnumerable<IQueryExpressionBuilder> innerExpressions)
            => this.Chain(x => x.InnerExpressions.AddRange(innerExpressions));

        IQueryExpressionFunction IQueryExpressionFunctionBuilder.Build()
            => new CoalesceFunction(FieldName, Function?.Build(), InnerExpressions.Select(x => x.Build()));
    }
}

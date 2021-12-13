﻿using System;
using QueryFramework.Abstractions;

namespace QueryFramework.SqlServer.QueryExpressions
{
    /// <summary>
    /// Query expression for using strongly-typed dates in queries on SQL server databases.
    /// </summary>
    /// <seealso cref="IQueryExpression" />
    public class Date : IQueryExpression
    {
        private readonly DateTime _value;

        /// <summary>Initializes a new instance of the <see cref="Date" /> class.</summary>
        /// <param name="value">The value.</param>
        public Date(DateTime value) => _value = value;

        /// <summary>Gets the name of the field.</summary>
        /// <value>The name of the field.</value>
        /// <remarks>This property is unused in this implementation.</remarks>
        public string FieldName => string.Empty;

        /// <summary>Gets the expression.</summary>
        /// <value>The expression.</value>
        public IQueryExpressionFunction? Function => new DateFunction(_value);

        private sealed class DateFunction : IQueryExpressionFunction
        {
            private readonly DateTime _value;

            public DateFunction(DateTime value)
            {
                _value = value;
            }

            public string Expression => "'" + _value.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            public IQueryExpressionFunction? InnerFunction => null;
        }
    }
}

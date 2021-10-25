using System.Collections.Generic;
using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class FieldSelectionQueryExtensions
    {
        /// <summary>Gets the fields that should be added to the SELECT clause on a query.</summary>
        /// <param name="fieldSelectionQuery">The field selection query.</param>
        /// <param name="skipFields">The skip fields.</param>
        public static IEnumerable<IQueryExpression> GetSelectFields(this IFieldSelectionQuery fieldSelectionQuery,
                                                                    IQueryProcessorSettings settings)
            => fieldSelectionQuery
                .Fields
                .Where(expression => settings.SkipFields == null || !settings.SkipFields.Contains(expression.FieldName));
    }
}

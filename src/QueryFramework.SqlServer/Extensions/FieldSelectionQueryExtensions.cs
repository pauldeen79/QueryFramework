using System.Collections.Generic;
using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.SqlServer.Extensions
{
    public static class FieldSelectionQueryExtensions
    {
        /// <summary>Gets the fields that should be added to the SELECT clause on a query.</summary>
        /// <param name="fieldSelectionQuery">The field selection query.</param>
        /// <param name="skipFields">The skip fields.</param>
        public static IEnumerable<IQueryExpression> GetSelectFields(this IFieldSelectionQuery fieldSelectionQuery,
                                                                    IEnumerable<string> skipFields = null)
            => fieldSelectionQuery
                .Fields
                .Where(expression => skipFields == null || !skipFields.Contains(expression.FieldName));
    }
}

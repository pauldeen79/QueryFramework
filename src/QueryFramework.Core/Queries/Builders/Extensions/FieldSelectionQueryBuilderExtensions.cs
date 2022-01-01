using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Queries.Builders;
using QueryFramework.Core.Builders;

namespace QueryFramework.Core.Queries.Builders.Extensions
{
    public static class FieldSelectionQueryBuilderExtensions
    {
        /// <summary>
        /// Adds select fields.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="additionalFieldNames">Select fields to add.</param>
        /// <returns>
        /// The FieldSelectionQuery instance.
        /// </returns>
        public static T Select<T>(this T instance, params IQueryExpressionBuilder[] additionalFieldNames)
            where T : IFieldSelectionQueryBuilder
        {
            instance.GetAllFields = false;
            instance.Fields.AddRange(additionalFieldNames);
            return instance;
        }

        /// <summary>
        /// Adds select fields.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="additionalFieldNames">Select fields to add.</param>
        /// <returns>
        /// The FieldSelectionQuery instance.
        /// </returns>
        public static T Select<T>(this T instance, params IQueryExpression[] additionalFieldNames)
            where T : IFieldSelectionQueryBuilder
            => instance.Select(additionalFieldNames.Select(x => new QueryExpressionBuilder(x)).ToArray());

        /// <summary>
        /// Adds select fields.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="additionalFieldNames">Select fields to add.</param>
        /// <returns>
        /// The FieldSelectionQuery instance.
        /// </returns>
        public static T Select<T>(this T instance, params string[] additionalFieldNames)
            where T : IFieldSelectionQueryBuilder
            => instance.Select(additionalFieldNames.Select(s => new QueryExpression(s)).ToArray());

        public static T SelectAll<T>(this T instance)
            where T : IFieldSelectionQueryBuilder
        {
            instance.GetAllFields = true;
            instance.Fields.Clear();
            return instance;
        }

        /// <summary>
        /// Adds distinct select fields.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="additionalFieldNames">Select fields to add.</param>
        /// <returns>
        /// The FieldSelectionQuery instance.
        /// </returns>
        public static T SelectDistinct<T>(this T instance, params IQueryExpressionBuilder[] additionalFieldNames)
            where T : IFieldSelectionQueryBuilder
        {
            instance.Distinct = true;
            instance.GetAllFields = false;
            instance.Fields.AddRange(additionalFieldNames);
            return instance;
        }

        /// <summary>
        /// Adds distinct select fields.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="additionalFieldNames">Select fields to add.</param>
        /// <returns>
        /// The FieldSelectionQuery instance.
        /// </returns>
        public static T SelectDistinct<T>(this T instance, params IQueryExpression[] additionalFieldNames)
            where T : IFieldSelectionQueryBuilder
            => instance.SelectDistinct(additionalFieldNames.Select(x => new QueryExpressionBuilder(x)).ToArray());

        /// <summary>
        /// Adds distinct select fields.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="additionalFieldNames">Select fields to add.</param>
        /// <returns>
        /// The FieldSelectionQuery instance.
        /// </returns>
        public static T SelectDistinct<T>(this T instance, params string[] additionalFieldNames)
        where T : IFieldSelectionQueryBuilder
        {
            instance.Distinct = true;
            return instance.Select(additionalFieldNames.Select(s => new QueryExpression(s)).ToArray());
        }

        public static T GetAllFields<T>(this T instance, bool getAllFields = true)
            where T : IFieldSelectionQueryBuilder
        {
            instance.GetAllFields = getAllFields;
            if (getAllFields)
            {
                instance.Fields.Clear();
            }
            return instance;
        }

        public static T Distinct<T>(this T instance, bool distinct = true)
            where T : IFieldSelectionQueryBuilder
        {
            instance.Distinct = distinct;
            return instance;
        }
    }
}

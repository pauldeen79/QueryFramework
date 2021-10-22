using System.Collections.Generic;
using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Queries.Builders;
using QueryFramework.Core.Builders;

namespace QueryFramework.Core.Queries.Builders.Extensions
{
    public static class AdvancedSingleDataObjectQueryBuilderExtensions
    {
        public static T From<T>(this T instance, string table)
            where T : IAdvancedSingleDataObjectQueryBuilder
        {
            instance.DataObjectName = table;
            return instance;
        }

        public static T WithParameters<T>(this T instance, params IQueryParameterBuilder[] parameters)
            where T : IAdvancedSingleDataObjectQueryBuilder
        {
            instance.Parameters.AddRange(parameters);
            return instance;
        }

        public static T WithParameters<T>(this T instance, params IQueryParameter[] parameters)
            where T : IAdvancedSingleDataObjectQueryBuilder
            => instance.WithParameters(parameters.Select(x => new QueryParameterBuilder(x)).ToArray());

        public static T WithParameters<T>(this T instance, params KeyValuePair<string, object>[] parameters)
            where T : IAdvancedSingleDataObjectQueryBuilder
        {
            instance.Parameters.AddRange(parameters.Select(kvp => new QueryParameterBuilder(new QueryParameter(kvp.Key, kvp.Value))));
            return instance;
        }

        public static T WithParameter<T>(this T instance, string name, object value)
            where T : IAdvancedSingleDataObjectQueryBuilder
        {
            instance.Parameters.Add(new QueryParameterBuilder(new QueryParameter(name, value)));
            return instance;
        }
    }
}

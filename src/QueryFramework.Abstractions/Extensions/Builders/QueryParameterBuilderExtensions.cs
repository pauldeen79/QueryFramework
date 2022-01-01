using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Extensions.Builders
{
    public static class QueryParameterBuilderExtensions
    {
        public static IQueryParameterBuilder WithName(this IQueryParameterBuilder instance, string name)
        {
            instance.Name = name;
            return instance;
        }
        public static IQueryParameterBuilder WithValue(this IQueryParameterBuilder instance, object value)
        {
            instance.Value = value;
            return instance;
        }
    }
}

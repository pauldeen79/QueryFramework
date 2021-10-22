using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Extensions.Builders
{
    public static class QueryParameterBuilderExtensions
    {
        public static IQueryParameterBuilder Clear(this IQueryParameterBuilder instance)
        {
            instance.Name = default;
            instance.Value = default;
            return instance;
        }
        public static IQueryParameterBuilder Update(this IQueryParameterBuilder instance, IQueryParameter source)
        {
            instance.Name = default;
            instance.Value = default;
            if (source != null)
            {
                instance.Name = source.Name;
                instance.Value = source.Value;
            }
            return instance;
        }
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

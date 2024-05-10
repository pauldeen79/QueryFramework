namespace QueryFramework.Abstractions.Extensions;

public static class ParameterizedQueryBuilderExtensions
{
    public static T AddParameter<T>(this T instance, string name, object? value)
        where T : IParameterizedQueryBuilder
        => instance.AddParameters(new QueryParameterBuilder().WithName(name.IsNotNull(nameof(name))).WithValue(value));

    private sealed class QueryParameterBuilder : IQueryParameterBuilder
    {
        public string Name { get; set; } = string.Empty;
        public object? Value { get; set; }
        public IQueryParameter Build() => new QueryParameter(Name, Value);
    }

    private sealed class QueryParameter : IQueryParameter
    {
        public QueryParameter(string name, object? value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public object? Value { get; }

        public IQueryParameterBuilder ToBuilder() => new QueryParameterBuilder { Name = Name, Value = Value };
    }
}

using QueryFramework.Abstractions;

namespace QueryFramework.Core
{
    public sealed record QueryParameter : IQueryParameter
    {
        public string Name { get; }

        public object Value { get; }

        public QueryParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}

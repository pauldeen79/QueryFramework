using QueryFramework.Abstractions;

namespace QueryFramework.Core
{
    public sealed record QueryParameterValue : IQueryParameterValue
    {
        public string Name { get; }

        public QueryParameterValue(string name)
        {
            Name = name;
        }
    }
}

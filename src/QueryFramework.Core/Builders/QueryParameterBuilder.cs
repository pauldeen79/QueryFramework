using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Core.Builders
{
    public class QueryParameterBuilder : IQueryParameterBuilder
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public IQueryParameter Build()
        {
            return new QueryParameter(Name, Value);
        }
        public QueryParameterBuilder(IQueryParameter source = null)
        {
            if (source != null)
            {
                Name = source.Name;
                Value = source.Value;
            }
        }
        public QueryParameterBuilder(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}

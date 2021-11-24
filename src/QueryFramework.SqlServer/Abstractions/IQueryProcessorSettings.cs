using CrossCutting.Data.Abstractions;

namespace QueryFramework.SqlServer.Abstractions
{
    public interface IQueryProcessorSettings : IDatabaseEntityRetrieverSettings
    {
        int? OverrideLimit { get; }
        int? OverrideOffset { get; }
        bool ValidateFieldNames { get; }
        int InitialParameterNumber { get; }
   }
}

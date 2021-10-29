namespace QueryFramework.SqlServer.Abstractions
{
    public interface IQueryProcessorSettings
    {
        string TableName { get; }
        string Fields { get; }
        string DefaultOrderBy { get; }
        string DefaultWhere { get; }
        int? OverrideLimit { get; }
        bool ValidateFieldNames { get; }
        int InitialParameterNumber { get; }
   }
}

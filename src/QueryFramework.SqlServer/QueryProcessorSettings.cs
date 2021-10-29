using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer
{
    public class QueryProcessorSettings : IQueryProcessorSettings
    {
        public string TableName { get; }
        public string Fields { get; }
        public string DefaultOrderBy { get; }
        public string DefaultWhere { get; }
        public int? OverrideLimit { get; }
        public bool ValidateFieldNames { get; }
        public int InitialParameterNumber { get; }

        public QueryProcessorSettings(string tableName = null,
                                      string fields = null,
                                      string defaultOrderBy = null,
                                      string defaultWhere = null,
                                      int? overrideLimit = null,
                                      bool validateFieldNames = true,
                                      int initialParameterNumber = 0)
        {
            TableName = tableName;
            Fields = fields;
            DefaultOrderBy = defaultOrderBy;
            DefaultWhere = defaultWhere;
            OverrideLimit = overrideLimit;
            ValidateFieldNames = validateFieldNames;
            InitialParameterNumber = initialParameterNumber;
        }
    }
}

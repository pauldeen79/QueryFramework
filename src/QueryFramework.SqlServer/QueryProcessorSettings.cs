using System;
using System.Collections.Generic;
using QueryFramework.Abstractions;
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
        public IEnumerable<string> SkipFields { get; }
        public Func<string, string> GetFieldNameDelegate { get; }
        public Func<IEnumerable<string>> GetAllFieldsDelegate { get; }
        public Func<IQueryExpression, bool> ExpressionValidationDelegate { get; }

        public QueryProcessorSettings(string tableName = null,
                                      string fields = null,
                                      string defaultOrderBy = null,
                                      string defaultWhere = null,
                                      int? overrideLimit = null,
                                      bool validateFieldNames = true,
                                      int initialParameterNumber = 0,
                                      IEnumerable<string> skipFields = null,
                                      Func<string, string> getFieldNameDelegate = null,
                                      Func<IEnumerable<string>> getAllFieldsDelegate = null,
                                      Func<IQueryExpression, bool> expressionValidationDelegate = null)
        {
            TableName = tableName;
            Fields = fields;
            DefaultOrderBy = defaultOrderBy;
            DefaultWhere = defaultWhere;
            OverrideLimit = overrideLimit;
            ValidateFieldNames = validateFieldNames;
            InitialParameterNumber = initialParameterNumber;
            SkipFields = skipFields;
            GetFieldNameDelegate = getFieldNameDelegate;
            GetAllFieldsDelegate = getAllFieldsDelegate;
            ExpressionValidationDelegate = expressionValidationDelegate;
        }
    }
}

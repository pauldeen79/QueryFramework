using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    public class TestEntityQueryProcessorSettings : IQueryProcessorSettings
    {
        public string TableName => throw new NotImplementedException();

        public string Fields => throw new NotImplementedException();

        public string DefaultOrderBy
        {
            get
            {
                var builder = new StringBuilder();
                builder.Append("[Name]");
                return builder.ToString();
            }
        }

        public string DefaultWhere
        {
            get
            {
                var builder = new StringBuilder();

                return builder.ToString();
            }
        }

        public int? OverrideLimit => -1;

        public bool ValidateFieldNames => true;

        public int InitialParameterNumber => 0;
    }
}

using System.Diagnostics.CodeAnalysis;
using Moq;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions;
using Xunit;

namespace QueryFramework.Abstractions.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class QueryExpressionBuilderExtensionsTests
    {
        private Mock<IQueryExpressionBuilder> Sut { get; }

        public QueryExpressionBuilderExtensionsTests()
        {
            Sut = new Mock<IQueryExpressionBuilder>();
        }

        [Fact]
        public void WithExpression_Updates_OpenBracket()
        {
            // Act
            var function = new Mock<IQueryExpressionFunction>().Object;
            Sut.Object.WithFunction(function);

            // Assert
            Sut.VerifySet(x => x.Function = function, Times.Once);
        }

        [Fact]
        public void WithFieldName_Updates_OpenBracket()
        {
            // Act
            Sut.Object.WithFieldName("fieldname");

            // Assert
            Sut.VerifySet(x => x.FieldName = "fieldname", Times.Once);
        }
    }
}

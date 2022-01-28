using System.Diagnostics.CodeAnalysis;
using Moq;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Extensions;
using Xunit;

namespace QueryFramework.Core.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class QueryExpressionBuilderExtensionsTests
    {
        private Mock<IQueryExpressionBuilder> Sut { get; }
        private IQueryExpressionFunctionBuilder? Function { get; } = new Mock<IQueryExpressionFunctionBuilder>().Object;

        public QueryExpressionBuilderExtensionsTests()
        {
            Sut = new Mock<IQueryExpressionBuilder>();
            Sut.SetupSet(m => m.Function = Function).Verifiable();
        }

        [Fact]
        public void WithFunction_Updates_Function()
        {
            // Act
            Sut.Object.WithFunction(Function);

            // Assert
            Sut.Verify();
        }

        [Fact]
        public void WithFieldName_Updates_FieldName()
        {
            // Act
            Sut.Object.WithFieldName("fieldname");

            // Assert
            Sut.VerifySet(x => x.FieldName = "fieldname", Times.Once);
        }
    }
}

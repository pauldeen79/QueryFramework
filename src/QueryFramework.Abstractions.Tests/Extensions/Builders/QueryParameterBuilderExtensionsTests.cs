using System.Diagnostics.CodeAnalysis;
using Moq;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions.Builders;
using Xunit;

namespace QueryFramework.Abstractions.Tests.Extensions.Builders
{
    [ExcludeFromCodeCoverage]
    public class QueryParameterBuilderExtensionsTests
    {
        private Mock<IQueryParameterBuilder> Sut { get; }

        public QueryParameterBuilderExtensionsTests()
        {
            Sut = new Mock<IQueryParameterBuilder>();
        }

        [Fact]
        public void WithName_Updates_OpenBracket()
        {
            // Act
            Sut.Object.WithName("name");

            // Assert
            Sut.VerifySet(x => x.Name = "name", Times.Once);
        }

        [Fact]
        public void WithValue_Updates_OpenBracket()
        {
            // Act
            Sut.Object.WithValue("value");

            // Assert
            Sut.VerifySet(x => x.Value = "value", Times.Once);
        }
    }
}

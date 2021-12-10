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
        public void Clear_Clears_All_Properties()
        {
            // Arrange
            Sut.SetupSet(x => x.Value = It.IsAny<object>()).Verifiable();

            // Act
            Sut.Object.Clear();

            // Assert
            Sut.VerifySet(x => x.Name = string.Empty, Times.Once);
            Sut.VerifySet(x => x.Value = It.IsAny<object>());
        }

        [Fact]
        public void Update_Updates_All_Properties()
        {
            // Arrange
            var updateMock = new Mock<IQueryParameter>();
            updateMock.SetupGet(x => x.Name).Returns("name");
            updateMock.SetupGet(x => x.Value).Returns("value");

            // Act
            Sut.Object.Update(updateMock.Object);

            // Assert
            Sut.VerifySet(x => x.Name = "name", Times.Once);
            Sut.VerifySet(x => x.Value = "value", Times.Once);
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

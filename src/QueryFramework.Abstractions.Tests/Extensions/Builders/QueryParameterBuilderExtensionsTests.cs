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
        [Fact]
        public void Clear_Clears_All_Properties()
        {
            // Arrange
            var sut = new Mock<IQueryParameterBuilder>();

            // Act
            sut.Object.Clear();

            // Assert
            sut.VerifySet(x => x.Name = default, Times.Once);
            sut.VerifySet(x => x.Value = default, Times.Once);
        }

        [Fact]
        public void Update_Updates_All_Properties()
        {
            // Arrange
            var sut = new Mock<IQueryParameterBuilder>();
            var updateMock = new Mock<IQueryParameter>();
            updateMock.SetupGet(x => x.Name).Returns("name");
            updateMock.SetupGet(x => x.Value).Returns("value");

            // Act
            sut.Object.Update(updateMock.Object);

            // Assert
            sut.VerifySet(x => x.Name = "name", Times.Once);
            sut.VerifySet(x => x.Value = "value", Times.Once);
        }

        [Fact]
        public void WithName_Updates_OpenBracket()
        {
            // Arrange
            var sut = new Mock<IQueryParameterBuilder>();

            // Act
            sut.Object.WithName("name");

            // Assert
            sut.VerifySet(x => x.Name = "name", Times.Once);
        }

        [Fact]
        public void WithValue_Updates_OpenBracket()
        {
            // Arrange
            var sut = new Mock<IQueryParameterBuilder>();

            // Act
            sut.Object.WithValue("value");

            // Assert
            sut.VerifySet(x => x.Value = "value", Times.Once);
        }
    }
}

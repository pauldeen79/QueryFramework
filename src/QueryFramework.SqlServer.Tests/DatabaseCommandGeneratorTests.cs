using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions.Queries;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class DatabaseCommandGeneratorTests
    {
        [Fact]
        public void Generate_Throws_On_Null_Query()
        {
            // Arrange
            var sut = new DatabaseCommandGenerator();

            // Act & Assert
            sut.Invoking(x => x.Generate<ISingleEntityQuery>(null, new QueryProcessorSettings(), false))
               .Should().Throw<ArgumentNullException>()
               .WithParameterName("query");
        }

        [Fact]
        public void Generate_Throws_On_Null_Settings()
        {
            // Arrange
            var queryMock = new Mock<ISingleEntityQuery>();
            var sut = new DatabaseCommandGenerator();

            // Act & Assert
            sut.Invoking(x => x.Generate(queryMock.Object, null, false))
               .Should().Throw<ArgumentNullException>()
               .WithParameterName("settings");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Abstractions;
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
            var queryProcessorSettingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            var sut = new DatabaseCommandGenerator();

            // Act & Assert
            sut.Invoking(x => x.Generate<ISingleEntityQuery>(null, queryProcessorSettingsMock.Object, fieldProviderMock.Object, false))
               .Should().Throw<ArgumentNullException>()
               .WithParameterName("query");
        }

        [Fact]
        public void Generate_Throws_On_Null_Settings()
        {
            // Arrange
            var queryMock = new Mock<ISingleEntityQuery>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            var sut = new DatabaseCommandGenerator();

            // Act & Assert
            sut.Invoking(x => x.Generate(queryMock.Object, null, fieldProviderMock.Object, false))
               .Should().Throw<ArgumentNullException>()
               .WithParameterName("settings");
        }

        [Fact]
        public void Generate_Throws_On_Null_FieldProvider()
        {
            // Arrange
            var queryMock = new Mock<ISingleEntityQuery>();
            var queryProcessorSettingsMock = new Mock<IQueryProcessorSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            var sut = new DatabaseCommandGenerator();

            // Act & Assert
            sut.Invoking(x => x.Generate(queryMock.Object, queryProcessorSettingsMock.Object, null, false))
               .Should().Throw<ArgumentNullException>()
               .WithParameterName("fieldProvider");
        }
    }
}

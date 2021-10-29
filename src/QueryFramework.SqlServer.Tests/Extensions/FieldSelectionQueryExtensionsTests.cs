using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using QueryFramework.Core.Queries.Builders;
using QueryFramework.Core.Queries.Builders.Extensions;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Extensions;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class FieldSelectionQueryExtensionsTests
    {
        [Fact]
        public void GetSelectFields_Without_SkipFields_Selects_All_SelectFields()
        {
            // Arrange
            var sut = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);

            // Act
            var selectFields = sut.GetSelectFields(fieldProviderMock.Object);

            // Assert
            selectFields.Should().HaveCount(3);
            selectFields.ElementAt(0).FieldName.Should().Be("Field1");
            selectFields.ElementAt(1).FieldName.Should().Be("Field2");
            selectFields.ElementAt(2).FieldName.Should().Be("Field3");
        }

        [Fact]
        public void GetSelectFields_With_SkipFields_Selects_SelectFields_NotEqualTo_SkipFields()
        {
            // Arrange
            var sut = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input.Where(x => x != "Field1" && x != "Field3"));

            // Act
            var selectFields = sut.GetSelectFields(fieldProviderMock.Object);

            // Assert
            selectFields.Should().HaveCount(1);
            selectFields.ElementAt(0).FieldName.Should().Be("Field2");
        }
    }
}

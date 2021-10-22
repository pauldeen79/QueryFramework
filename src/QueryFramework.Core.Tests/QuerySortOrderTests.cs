using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace QueryFramework.Core.Tests
{
    [ExcludeFromCodeCoverage]
    public class QuerySortOrderTests
    {
        [Fact]
        public void ToString_Gives_A_Good_Indication_Of_Underlying_Values()
        {
            // Arrange
            var sut = new QuerySortOrder("Fieldname");

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("Fieldname Ascending");
        }
    }
}

namespace QueryFramework.Abstractions.Tests.Extensions;

public class SingleEntityQueryExtensionsTests
{
    [Fact]
    public void Validate_Validates_Instance()
    {
        // Arrange
        var sut = new SingleEntityQueryMock
        {
            ValidationResultValue = new[] { new ValidationResult("kaboom") }
        };

        // Act
        sut.Invoking(x => x.Validate())
           .Should().Throw<ValidationException>()
           .And.Message.Should().Be("kaboom");
    }

    [Fact]
    public void GetTableName_Returns_DataObjectName_When_Available_And_Filled()
    {
        // Arrange
        var sut = new Mock<IDataObjectNameQuery>();
        sut.SetupGet(x => x.DataObjectName).Returns("custom");

        // Act
        var actual = sut.Object.GetTableName("default");

        // Assert
        actual.Should().Be("custom");
    }

    [Fact]
    public void GetTableName_Returns_TableName_When_DataObjectName_Is_Available_And_Not_Filled()
    {
        // Arrange
        var sut = new Mock<IDataObjectNameQuery>();
        sut.SetupGet(x => x.DataObjectName).Returns(string.Empty);

        // Act
        var actual = sut.Object.GetTableName("default");

        // Assert
        actual.Should().Be("default");
    }

    [Fact]
    public void GetTableName_Returns_TableName_When_DataObjectName_Is_Not_Available()
    {
        // Arrange
        var sut = new Mock<ISingleEntityQuery>();

        // Act
        var actual = sut.Object.GetTableName("default");

        // Assert
        actual.Should().Be("default");
    }

    private class SingleEntityQueryMock : ISingleEntityQuery, IValidatableObject
    {
        public int? Limit { get; set; }

        public int? Offset { get; set; }

        public IReadOnlyCollection<ICondition> Conditions { get; set; } = new ValueCollection<ICondition>();

        public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; } = new ValueCollection<IQuerySortOrder>();

        public IEnumerable<ValidationResult> ValidationResultValue { get; set; } = Enumerable.Empty<ValidationResult>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => ValidationResultValue;
    }
}

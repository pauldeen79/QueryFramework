namespace QueryFramework.Abstractions.Tests.Extensions;

public class QueryExtensionsTests
{
    [Fact]
    public void Validate_Validates_Instance()
    {
        // Arrange
        var sut = new MyQueryMock
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
        var sut = Substitute.For<IDataObjectNameQuery>();
        sut.DataObjectName.Returns("custom");

        // Act
        var actual = sut.GetTableName("default");

        // Assert
        actual.Should().Be("custom");
    }

    [Fact]
    public void GetTableName_Returns_TableName_When_DataObjectName_Is_Available_And_Not_Filled()
    {
        // Arrange
        var sut = Substitute.For<IDataObjectNameQuery>();
        sut.DataObjectName.Returns(string.Empty);

        // Act
        var actual = sut.GetTableName("default");

        // Assert
        actual.Should().Be("default");
    }

    [Fact]
    public void GetTableName_Returns_TableName_When_DataObjectName_Is_Not_Available()
    {
        // Arrange
        var sut = Substitute.For<IQuery>();

        // Act
        var actual = sut.GetTableName("default");

        // Assert
        actual.Should().Be("default");
    }

    private sealed class MyQueryMock : IQuery, IValidatableObject
    {
        public int? Limit { get; set; }

        public int? Offset { get; set; }

        public ComposedEvaluatable Filter { get; set; } = new(Enumerable.Empty<ComposableEvaluatable>());

        public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; } = new ReadOnlyValueCollection<IQuerySortOrder>();

        public IEnumerable<ValidationResult> ValidationResultValue { get; set; } = Enumerable.Empty<ValidationResult>();

        public IQueryBuilder ToBuilder()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => ValidationResultValue;
    }
}

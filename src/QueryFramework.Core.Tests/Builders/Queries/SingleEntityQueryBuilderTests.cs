namespace QueryFramework.Core.Tests.Builders.Queries;

public class SingleEntityQueryBuilderTests
{
    private SingleEntityQueryBuilder CreateSut() => new SingleEntityQueryBuilder();

    [Fact]
    public void Can_Validate_Recursively()
    {
        // Arrange
        var sut = CreateSut().AddOrderByFields(new QuerySortOrderBuilder());

        // Act
        var validationResults = new List<ValidationResult>();
        var success = sut.TryValidate(validationResults);

        // Assert
        success.ShouldBeFalse();
        validationResults.ShouldHaveSingleItem(); //validation errors of QuerySortOrder should be included
    }
}

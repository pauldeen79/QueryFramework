namespace QueryFramework.Core.Tests.Extensions;

public class GroupingQueryBuilderExtensionsTests
{
    [Fact]
    public void GroupBy_With_ExpressionBuilder_ParamArray_Returns_Correct_Result()
    {
        // Arrange
        var sut = new GroupingQueryBuilder();

        // Act
        var result = sut.GroupBy(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression("MyField"));

        // Assert
        result.GroupByFields.Should().HaveCount(1);
    }

    [Fact]
    public void GroupBy_With_ExpressionBuilder_Enumerable_Returns_Correct_Result()
    {
        // Arrange
        var sut = new GroupingQueryBuilder();

        // Act
        var result = sut.GroupBy(new[] { new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression("MyField") }.AsEnumerable());

        // Assert
        result.GroupByFields.Should().HaveCount(1);
    }

    [Fact]
    public void GroupBy_With_String_ParamArray_Returns_Correct_Result()
    {
        // Arrange
        var sut = new GroupingQueryBuilder();

        // Act
        var result = sut.GroupBy("MyField");

        // Assert
        result.GroupByFields.Should().HaveCount(1);
    }

    [Fact]
    public void GroupBy_With_String_Enumerable_Returns_Correct_Result()
    {
        // Arrange
        var sut = new GroupingQueryBuilder();

        // Act
        var result = sut.GroupBy(new[] { "MyField" }.AsEnumerable());

        // Assert
        result.GroupByFields.Should().HaveCount(1);
    }

    [Fact]
    public void Having_With_ParramArray_Returns_Correct_Result()
    {
        // Arrange
        var sut = new GroupingQueryBuilder();

        // Act
        var result = sut.Having(new ComposableEvaluatableBuilder());

        // Assert
        result.GroupByFilter.Conditions.Should().HaveCount(1);
    }

    [Fact]
    public void Having_With_Enumrable_Returns_Correct_Result()
    {
        // Arrange
        var sut = new GroupingQueryBuilder();

        // Act
        var result = sut.Having(new[] { new ComposableEvaluatableBuilder() }.AsEnumerable());

        // Assert
        result.GroupByFilter.Conditions.Should().HaveCount(1);
    }
}

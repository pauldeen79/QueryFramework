namespace QueryFramework.SqlServer.Tests.Extensions;

public class ExpressionExtensionsTests
{
    [Fact]
    public void Can_Use_Len_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field");

        // Act
        var actual = sut.Len();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LEN(Field)", default);
    }

    [Fact]
    public void Can_Use_Len_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field").Trim();

        // Act
        var actual = sut.Len();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LEN(TRIM(Field))", default);
    }

    [Fact]
    public void Can_Use_Trim_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field");

        // Act
        var actual = sut.Trim();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "TRIM(Field)", default);
    }

    [Fact]
    public void Can_Use_Trim_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field").Upper();

        // Act
        var actual = sut.Trim();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "TRIM(UPPER(Field))", default);
    }

    [Fact]
    public void Can_Use_Upper_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field");

        // Act
        var actual = sut.Upper();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "UPPER(Field)", default);
    }

    [Fact]
    public void Can_Use_Upper_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field").Trim();

        // Act
        var actual = sut.Upper();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "UPPER(TRIM(Field))", default);
    }

    [Fact]
    public void Can_Use_Lower_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field");

        // Act
        var actual = sut.Lower();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LOWER(Field)", default);
    }

    [Fact]
    public void Can_Use_Lower_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field").Trim();

        // Act
        var actual = sut.Lower();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LOWER(TRIM(Field))", default);
    }

    [Fact]
    public void Can_Use_Left_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field");

        // Act
        var actual = sut.Left(2);

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LEFT(Field, 2)", default);
    }

    [Fact]
    public void Can_Use_Left_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field").Trim();

        // Act
        var actual = sut.Left(2);

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LEFT(TRIM(Field), 2)", default);
    }

    [Fact]
    public void Can_Use_Right_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field");

        // Act
        var actual = sut.Right(2);

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "RIGHT(Field, 2)", default);
    }

    [Fact]
    public void Can_Use_Right_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field").Trim();

        // Act
        var actual = sut.Right(2);

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "RIGHT(TRIM(Field), 2)", default);
    }

    [Fact]
    public void Can_Use_Year_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<DateTime>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field");

        // Act
        var actual = sut.Year();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "YEAR(Field)", default);
    }

    [Fact]
    public void Can_Use_Month_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<DateTime>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field");

        // Act
        var actual = sut.Month();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "MONTH(Field)", default);
    }

    [Fact]
    public void Can_Use_Day_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<DateTime>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field");

        // Act
        var actual = sut.Day();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "DAY(Field)", default);
    }

    [Fact]
    public void Can_Use_Count_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new CastExpressionBuilder<IEnumerable>(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field").Build());

        // Act
        var actual = sut.Count();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "COUNT(Field)", default);
    }

    [Fact]
    public void Can_Use_Sum_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new TypedFieldExpressionBuilder<IEnumerable>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field");

        // Act
        var actual = sut.Sum();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "SUM(Field)", default);
    }

    [Fact]
    public void Can_Use_Sum_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new CastExpressionBuilder<IEnumerable>(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field").Trim().Build().ToUntyped());

        // Act
        var actual = sut.Sum();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "SUM(TRIM(Field))", default);
    }
}

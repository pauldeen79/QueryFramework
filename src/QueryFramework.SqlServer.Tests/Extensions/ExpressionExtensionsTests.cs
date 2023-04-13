namespace QueryFramework.SqlServer.Tests.Extensions;

public class ExpressionExtensionsTests
{
    [Fact]
    public void Can_Use_Len_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field"));

        // Act
        var actual = sut.Len();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LEN(Field)");
    }

    [Fact]
    public void Can_Use_Len_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field").Trim());

        // Act
        var actual = sut.Len();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LEN(TRIM(Field))");
    }

    [Fact]
    public void Can_Use_Trim_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field"));

        // Act
        var actual = sut.Trim();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "TRIM(Field)");
    }

    [Fact]
    public void Can_Use_Trim_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field").Upper());

        // Act
        var actual = sut.Trim();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "TRIM(UPPER(Field))");
    }

    [Fact]
    public void Can_Use_Upper_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field"));

        // Act
        var actual = sut.Upper();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "UPPER(Field)");
    }

    [Fact]
    public void Can_Use_Upper_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field").Trim());

        // Act
        var actual = sut.Upper();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "UPPER(TRIM(Field))");
    }

    [Fact]
    public void Can_Use_Lower_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field"));

        // Act
        var actual = sut.Lower();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LOWER(Field)");
    }

    [Fact]
    public void Can_Use_Lower_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field").Trim());

        // Act
        var actual = sut.Lower();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LOWER(TRIM(Field))");
    }

    [Fact]
    public void Can_Use_Left_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field"));

        // Act
        var actual = sut.Left(2);

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LEFT(Field, 2)");
    }

    [Fact]
    public void Can_Use_Left_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field").Trim());

        // Act
        var actual = sut.Left(2);

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LEFT(TRIM(Field), 2)");
    }

    [Fact]
    public void Can_Use_Right_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field"));

        // Act
        var actual = sut.Right(2);

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "RIGHT(Field, 2)");
    }

    [Fact]
    public void Can_Use_Right_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field").Trim());

        // Act
        var actual = sut.Right(2);

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "RIGHT(TRIM(Field), 2)");
    }

    [Fact]
    public void Can_Use_Year_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field"));

        // Act
        var actual = sut.Year();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "YEAR(Field)");
    }

    [Fact]
    public void Can_Use_Month_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field"));

        // Act
        var actual = sut.Month();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "MONTH(Field)");
    }

    [Fact]
    public void Can_Use_Day_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field"));

        // Act
        var actual = sut.Day();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "DAY(Field)");
    }

    [Fact]
    public void Can_Use_Count_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field"));

        // Act
        var actual = sut.Count();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "COUNT(Field)");
    }

    [Fact]
    public void Can_Use_Count_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field").Trim());

        // Act
        var actual = sut.Count();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "COUNT(TRIM(Field))");
    }

    [Fact]
    public void Can_Use_Sum_Function_On_Basic_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field"));

        // Act
        var actual = sut.Sum();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "SUM(Field)");
    }

    [Fact]
    public void Can_Use_Sum_Function_On_Expression_With_Existing_Expression()
    {
        // Arrange
        var sut = new FieldExpressionBuilder().WithExpression(new ConstantExpressionBuilder().WithValue("Field").Trim());

        // Act
        var actual = sut.Sum();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "SUM(TRIM(Field))");
    }
}

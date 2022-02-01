namespace QueryFramework.SqlServer.Tests.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void Can_Use_Len_Function_On_FieldName_String()
    {
        // Arrange
        var sut = "Field";

        // Act
        var actual = sut.Len();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LEN(Field)");
    }

    [Fact]
    public void Can_Use_SqlTrim_Function_On_FieldName_String()
    {
        // Arrange
        var sut = "Field";

        // Act
        var actual = sut.SqlTrim();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "TRIM(Field)");
    }

    [Fact]
    public void Can_Use_Upper_Function_On_FieldName_String()
    {
        // Arrange
        var sut = "Field";

        // Act
        var actual = sut.Upper();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "UPPER(Field)");
    }

    [Fact]
    public void Can_Use_Lower_Function_On_FieldName_String()
    {
        // Arrange
        var sut = "Field";

        // Act
        var actual = sut.Lower();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LOWER(Field)");
    }

    [Fact]
    public void Can_Use_Left_Function_On_FieldName_String()
    {
        // Arrange
        var sut = "Field";

        // Act
        var actual = sut.Left(1);

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "LEFT(Field, 1)");
    }

    [Fact]
    public void Can_Use_Right_Function_On_FieldName_String()
    {
        // Arrange
        var sut = "Field";

        // Act
        var actual = sut.Right(1);

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "RIGHT(Field, 1)");
    }

    [Fact]
    public void Can_Use_Year_Function_On_FieldName_String()
    {
        // Arrange
        var sut = "Field";

        // Act
        var actual = sut.Year();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "YEAR(Field)");
    }

    [Fact]
    public void Can_Use_Month_Function_On_FieldName_String()
    {
        // Arrange
        var sut = "Field";

        // Act
        var actual = sut.Month();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "MONTH(Field)");
    }

    [Fact]
    public void Can_Use_Day_Function_On_FieldName_String()
    {
        // Arrange
        var sut = "Field";

        // Act
        var actual = sut.Day();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "DAY(Field)");
    }

    [Fact]
    public void Can_Use_Coalesce_Function_On_FieldName_String()
    {
        // Arrange
        var sut = "Field";

        // Act
        var actual = sut.Coalesce("default");

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "COALESCE(Field, default)");
    }

    [Fact]
    public void Can_Use_Count_Function_On_FieldName_String()
    {
        // Arrange
        var sut = "Field";

        // Act
        var actual = sut.Count();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "COUNT(Field)");
    }

    [Fact]
    public void Can_Use_Sum_Function_On_FieldName_String()
    {
        // Arrange
        var sut = "Field";

        // Act
        var actual = sut.Sum();

        // Assert
        SqlHelpers.ExpressionSqlShouldBe(actual, "SUM(Field)");
    }
}

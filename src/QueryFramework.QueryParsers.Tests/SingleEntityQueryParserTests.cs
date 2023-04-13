namespace QueryFramework.QueryParsers.Tests;

public class SingleEntityQueryParserTests
{
    [Theory]
    [InlineData("CONTAINS", "MyValue", Operator.Contains)]
    [InlineData("ENDSWITH", "MyValue", Operator.EndsWith)]
    [InlineData("\"ENDS WITH\"", "MyValue", Operator.EndsWith)]
    [InlineData("=", "MyValue", Operator.Equal)]
    [InlineData("==", "MyValue", Operator.Equal)]
    [InlineData(">=", "MyValue", Operator.GreaterOrEqual)]
    [InlineData(">", "MyValue", Operator.Greater)]
    [InlineData("\"IS NOT\"", "NULL", Operator.IsNotNull)]
    [InlineData("IS", "NULL", Operator.IsNull)]
    [InlineData("<=", "MyValue", Operator.SmallerOrEqual)]
    [InlineData("<", "MyValue", Operator.Smaller)]
    [InlineData("NOTCONTAINS", "MyValue", Operator.NotContains)]
    [InlineData("\"NOT CONTAINS\"", "MyValue", Operator.NotContains)]
    [InlineData("NOTENDSWITH", "MyValue", Operator.NotEndsWith)]
    [InlineData("\"NOT ENDS WITH\"", "MyValue", Operator.NotEndsWith)]
    [InlineData("<>", "MyValue", Operator.NotEqual)]
    [InlineData("!=", "MyValue", Operator.NotEqual)]
    [InlineData("#", "MyValue", Operator.NotEqual)]
    [InlineData("NOTSTARTSWITH", "MyValue", Operator.NotStartsWith)]
    [InlineData("\"NOT STARTS WITH\"", "MyValue", Operator.NotStartsWith)]
    [InlineData("STARTSWITH", "MyValue", Operator.StartsWith)]
    [InlineData("\"STARTS WITH\"", "MyValue", Operator.StartsWith)]
    public void Can_Parse_EntityQuery_With_Operator(string @operator, string value, Operator expectedOperator)
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, $"MyFieldName {@operator} {value}");

        // Assert
        actual.Filter.Should().HaveCount(1);
        var conditionField = actual.Filter.First().LeftExpression as FieldExpressionBuilder;
        var conditionValue = (actual.Filter.First().RightExpression as ConstantExpressionBuilder)?.Value;
        conditionField?.FieldName.Should().Be("MyFieldName");
        actual.Filter.First().Operator.Should().Be(expectedOperator);
        conditionValue.Should().Be(value == "NULL" ? null : value);
    }

    [Fact]
    public void Can_Parse_EntityQuery_With_Space_In_Value()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, $"MyFieldName = \"My Value\"");

        // Assert
        actual.Filter.Should().HaveCount(1);
        var conditionField = actual.Filter.First().LeftExpression as FieldExpressionBuilder;
        var conditionValue = (actual.Filter.First().RightExpression as ConstantExpressionBuilder)?.Value;
        conditionField?.FieldName.Should().Be("MyFieldName");
        actual.Filter.First().Operator.Should().Be(Operator.Equal);
        conditionValue.Should().Be("My Value");
    }

    [Fact]
    public void Can_Parse_SimpleQuery_With_Two_Words()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, "First Second");

        // Assert
        actual.Filter.Should().HaveCount(2);
    }

    [Fact]
    public void Can_Parse_SimpleQuery_With_Invalid_Operator()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, "MyFieldName = MyFirstValue AND MyOtherFieldName ? MySecondValue");

        // Assert
        actual.Filter.Should().HaveCount(7);
    }

    [Fact]
    public void Can_Parse_Empty_Query()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, string.Empty);

        // Assert
        actual.Filter.Should().BeEmpty();
    }

    private static SingleEntityQueryParser<ISingleEntityQueryBuilder, FieldExpressionBuilder> CreateSut()
        => new(() => new FieldExpressionBuilder().WithFieldName("PrefilledField"));
}

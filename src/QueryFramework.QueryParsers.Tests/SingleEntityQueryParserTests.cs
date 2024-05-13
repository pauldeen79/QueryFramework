namespace QueryFramework.QueryParsers.Tests;

public class SingleEntityQueryParserTests
{
    [Theory]
    [InlineData("CONTAINS", "MyValue", typeof(StringContainsOperatorBuilder))]
    [InlineData("ENDSWITH", "MyValue", typeof(EndsWithOperatorBuilder))]
    [InlineData("\"ENDS WITH\"", "MyValue", typeof(EndsWithOperatorBuilder))]
    [InlineData("=", "MyValue", typeof(EqualsOperatorBuilder))]
    [InlineData("==", "MyValue", typeof(EqualsOperatorBuilder))]
    [InlineData(">=", "MyValue", typeof(IsGreaterOrEqualOperatorBuilder))]
    [InlineData(">", "MyValue", typeof(IsGreaterOperatorBuilder))]
    [InlineData("\"IS NOT\"", "NULL", typeof(IsNotNullOperatorBuilder))]
    [InlineData("IS", "NULL", typeof(IsNullOperatorBuilder))]
    [InlineData("<=", "MyValue", typeof(IsSmallerOrEqualOperatorBuilder))]
    [InlineData("<", "MyValue", typeof(IsSmallerOperatorBuilder))]
    [InlineData("NOTCONTAINS", "MyValue", typeof(StringNotContainsOperatorBuilder))]
    [InlineData("\"NOT CONTAINS\"", "MyValue", typeof(StringNotContainsOperatorBuilder))]
    [InlineData("NOTENDSWITH", "MyValue", typeof(NotEndsWithOperatorBuilder))]
    [InlineData("\"NOT ENDS WITH\"", "MyValue", typeof(NotEndsWithOperatorBuilder))]
    [InlineData("<>", "MyValue", typeof(NotEqualsOperatorBuilder))]
    [InlineData("!=", "MyValue", typeof(NotEqualsOperatorBuilder))]
    [InlineData("#", "MyValue", typeof(NotEqualsOperatorBuilder))]
    [InlineData("NOTSTARTSWITH", "MyValue", typeof(NotStartsWithOperatorBuilder))]
    [InlineData("\"NOT STARTS WITH\"", "MyValue", typeof(NotStartsWithOperatorBuilder))]
    [InlineData("STARTSWITH", "MyValue", typeof(StartsWithOperatorBuilder))]
    [InlineData("\"STARTS WITH\"", "MyValue", typeof(StartsWithOperatorBuilder))]
    public void Can_Parse_EntityQuery_With_Operator(string @operator, string value, Type expectedOperatorBuilder)
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, $"MyFieldName {@operator} {value}");

        // Assert
        actual.Filter.Conditions.Should().HaveCount(1);
        var conditionField = actual.Filter.Conditions[0].LeftExpression as FieldExpressionBuilder;
        var conditionValue = (actual.Filter.Conditions[0].RightExpression as ConstantExpressionBuilder)?.Value;
        ((TypedConstantExpressionBuilder<string>)conditionField!.FieldNameExpression).Value.Should().Be("MyFieldName");
        actual.Filter.Conditions[0].Operator.Should().BeOfType(expectedOperatorBuilder);
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
        actual.Filter.Conditions.Should().HaveCount(1);
        var conditionField = actual.Filter.Conditions[0].LeftExpression as FieldExpressionBuilder;
        var conditionValue = (actual.Filter.Conditions[0].RightExpression as ConstantExpressionBuilder)?.Value;
        ((TypedConstantExpressionBuilder<string>)conditionField!.FieldNameExpression).Value.Should().Be("MyFieldName");
        actual.Filter.Conditions[0].Operator.Should().BeOfType<EqualsOperatorBuilder>();
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
        actual.Filter.Conditions.Should().HaveCount(2);
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
        actual.Filter.Conditions.Should().HaveCount(7);
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
        actual.Filter.Conditions.Should().BeEmpty();
    }

    private static SingleEntityQueryParser<IQueryBuilder, FieldExpressionBuilder> CreateSut()
        => new(() => new FieldExpressionBuilder()
            .WithExpression(new ContextExpressionBuilder())
            .WithFieldName("PrefilledField"));
}

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
        actual.Conditions.Should().HaveCount(1);
        var conditionField = actual.Conditions.First().LeftExpression as FieldExpressionBuilder;
        var conditionValue = (actual.Conditions.First().RightExpression as ConstantExpressionBuilder)?.Value;
        //actual.Conditions.First().CloseBracket.Should().BeFalse();
        //actual.Conditions.First().Combination.Should().Be(QueryCombination.And);
        conditionField?.FieldName.Should().Be("MyFieldName");
        //actual.Conditions.First().OpenBracket.Should().BeFalse();
        actual.Conditions.First().Operator.Should().Be(expectedOperator);
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
        actual.Conditions.Should().HaveCount(1);
        var conditionField = actual.Conditions.First().LeftExpression as FieldExpressionBuilder;
        var conditionValue = (actual.Conditions.First().RightExpression as ConstantExpressionBuilder)?.Value;
        //actual.Conditions.First().CloseBracket.Should().BeFalse();
        //actual.Conditions.First().Combination.Should().Be(QueryCombination.And);
        conditionField?.FieldName.Should().Be("MyFieldName");
        //actual.Conditions.First().OpenBracket.Should().BeFalse();
        actual.Conditions.First().Operator.Should().Be(Operator.Equal);
        conditionValue.Should().Be("My Value");
    }

    //[Theory]
    //[InlineData("AND", QueryCombination.And)]
    //[InlineData("OR", QueryCombination.Or)]
    //public void Can_Parse_EntityQuery_With_Multiple_Items(string combination, QueryCombination secondQueryCombination)
    //{
    //    // Arrange
    //    var builder = new SingleEntityQueryBuilder();
    //    var sut = CreateSut();

    //    // Act
    //    var actual = sut.Parse(builder, $"MyFieldName = MyFirstValue {combination} MyOtherFieldName != MySecondValue");

    //    // Assert
    //    actual.Conditions.Should().HaveCount(2);
    //    var fistConditionField = actual.Conditions.First().LeftExpression as FieldExpressionBuilder;
    //    var fistConditionValue = (actual.Conditions.First().RightExpression as ConstantExpressionBuilder)?.Value;
    //    var laatConditionField = actual.Conditions.Last().LeftExpression as FieldExpressionBuilder;
    //    var lastConditionValue = (actual.Conditions.Last().RightExpression as ConstantExpressionBuilder)?.Value;
    //    //actual.Conditions.First().CloseBracket.Should().BeFalse();
    //    //actual.Conditions.First().Combination.Should().Be(QueryCombination.And);
    //    fistConditionField?.FieldName.Should().Be("MyFieldName");
    //    //actual.Conditions.First().OpenBracket.Should().BeFalse();
    //    actual.Conditions.First().Operator.Should().Be(Operator.Equal);
    //    fistConditionValue.Should().Be("MyFirstValue");
    //    //actual.Conditions.Last().CloseBracket.Should().BeFalse();
    //    //actual.Conditions.Last().Combination.Should().Be(secondQueryCombination);
    //    laatConditionField?.FieldName.Should().Be("MyOtherFieldName");
    //    //actual.Conditions.Last().OpenBracket.Should().BeFalse();
    //    actual.Conditions.Last().Operator.Should().Be(Operator.NotEqual);
    //    lastConditionValue.Should().Be("MySecondValue");
    //}

    //[Fact]
    //public void Can_Parse_EntityQuery_With_Brackets()
    //{
    //    // Arrange
    //    var builder = new SingleEntityQueryBuilder();
    //    var sut = CreateSut();

    //    // Act
    //    var actual = sut.Parse(builder, "(MyFieldName = MyFirstValue AND MyOtherFieldName != MySecondValue)");

    //    // Assert
    //    actual.Conditions.Should().HaveCount(2);
    //    var fistConditionField = actual.Conditions.First().LeftExpression as FieldExpressionBuilder;
    //    var firstConditionValue = actual.Conditions.First().RightExpression as ConstantExpressionBuilder;
    //    var lastConditionField = actual.Conditions.Last().LeftExpression as FieldExpressionBuilder;
    //    var lastConditionValue = actual.Conditions.Last().RightExpression as ConstantExpressionBuilder;
    //    actual.Conditions.First().CloseBracket.Should().BeFalse();
    //    actual.Conditions.Last().CloseBracket.Should().BeTrue();
    //    actual.Conditions.First().OpenBracket.Should().BeTrue();
    //    actual.Conditions.Last().OpenBracket.Should().BeFalse();
    //    fistConditionField.FieldName.Should().Be("MyFieldName");
    //    firstConditionValue.Should().Be("MyFirstValue");
    //    lastConditionField.FieldName.Should().Be("MyOtherFieldName");
    //    lastConditionValue.Should().Be("MySecondValue");
    //}

    //[Theory]
    //[InlineData("=", "XOR")]
    //[InlineData("?", "OR")]
    //[InlineData("?", "KABOOM")]
    //public void Can_Parse_SimpleQuery(string @operator, string combination)
    //{
    //    // Arrange
    //    var builder = new SingleEntityQueryBuilder();
    //    var sut = CreateSut();

    //    // Act
    //    var actual = sut.Parse(builder, $"MyFieldName {@operator} MyFirstValue {combination} MyOtherFieldName != MySecondValue");

    //    // Assert
    //    actual.Conditions.Should().HaveCount(7);
    //    actual.Conditions.All(x => x.Field.FieldName == "PrefilledField").Should().BeTrue();
    //    actual.Conditions.All(x => x.Combination == QueryCombination.Or).Should().BeTrue();
    //    actual.Conditions.All(x => x.Operator == QueryOperator.Contains).Should().BeTrue();

    //    actual.Conditions.ElementAt(0).Value.Should().Be("MyFieldName");
    //    actual.Conditions.ElementAt(1).Value.Should().Be(@operator);
    //    actual.Conditions.ElementAt(2).Value.Should().Be("MyFirstValue");
    //    actual.Conditions.ElementAt(3).Value.Should().Be(combination);
    //    actual.Conditions.ElementAt(4).Value.Should().Be("MyOtherFieldName");
    //    actual.Conditions.ElementAt(5).Value.Should().Be("!=");
    //    actual.Conditions.ElementAt(6).Value.Should().Be("MySecondValue");

    //    actual.Conditions.ElementAt(0).OpenBracket.Should().BeTrue();
    //    actual.Conditions.ElementAt(1).OpenBracket.Should().BeFalse();
    //    actual.Conditions.ElementAt(2).OpenBracket.Should().BeFalse();
    //    actual.Conditions.ElementAt(3).OpenBracket.Should().BeFalse();
    //    actual.Conditions.ElementAt(4).OpenBracket.Should().BeFalse();
    //    actual.Conditions.ElementAt(5).OpenBracket.Should().BeFalse();
    //    actual.Conditions.ElementAt(6).OpenBracket.Should().BeFalse();

    //    actual.Conditions.ElementAt(0).CloseBracket.Should().BeFalse();
    //    actual.Conditions.ElementAt(1).CloseBracket.Should().BeFalse();
    //    actual.Conditions.ElementAt(2).CloseBracket.Should().BeFalse();
    //    actual.Conditions.ElementAt(3).CloseBracket.Should().BeFalse();
    //    actual.Conditions.ElementAt(4).CloseBracket.Should().BeFalse();
    //    actual.Conditions.ElementAt(5).CloseBracket.Should().BeFalse();
    //    actual.Conditions.ElementAt(6).CloseBracket.Should().BeTrue();
    //}

    //[Theory]
    //[InlineData("-", Operator.NotContains, QueryCombination.And)]
    //[InlineData("+", Operator.Contains, QueryCombination.And)]
    //[InlineData("", Operator.Contains, QueryCombination.Or)]
    //public void Can_Parse_SimpleQuery_With_Sign(string sign, Operator expectedOperator, QueryCombination expectedCombination)
    //{
    //    // Arrange
    //    var builder = new SingleEntityQueryBuilder();
    //    var sut = CreateSut();

    //    // Act
    //    var actual = sut.Parse(builder, $"{sign}First {sign}Second");

    //    // Assert
    //    actual.Conditions.Should().HaveCount(2);
    //    actual.Conditions.All(x => x.Field.FieldName == "PrefilledField").Should().BeTrue();
    //    actual.Conditions.All(x => x.Combination == expectedCombination).Should().BeTrue();
    //    actual.Conditions.All(x => x.Operator == expectedOperator).Should().BeTrue();
    //}

    [Fact]
    public void Can_Parse_SimpleQuery_With_Two_Words()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, "First Second");

        // Assert
        actual.Conditions.Should().HaveCount(2);
    }

    [Fact]
    public void Can_Parse_SimpleQuery_With_Invalid_Combination()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, "MyFieldName = MyFirstValue ? MyOtherFieldName != MySecondValue");

        // Assert
        actual.Conditions.Should().HaveCount(7);
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
        actual.Conditions.Should().HaveCount(7);
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
        actual.Conditions.Should().BeEmpty();
    }

    private static SingleEntityQueryParser<ISingleEntityQueryBuilder, FieldExpressionBuilder> CreateSut()
        => new(() => new FieldExpressionBuilder().WithFieldName("PrefilledField"));
}

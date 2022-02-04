namespace QueryFramework.InMemory.Tests;

public class OrderByWrapperTests
{
    [Fact]
    public void CompareTo_T_Returns_Zero_When_OrderByFields_On_Two_Instances_Are_Both_Null()
    {
        // Arrange
        var data = new MyClass { Property = null };
        var other = new MyClass { Property = null };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), QuerySortOrderDirection.Ascending) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut.CompareTo(new OrderByWrapper<MyClass>(other, orderByFields, valueRetriever));

        // Assert
        actual.Should().Be(0);
    }

    [Theory]
    [InlineData(QuerySortOrderDirection.Ascending, -1)]
    [InlineData(QuerySortOrderDirection.Descending, 1)]
    public void CompareTo_T_Returns_Correct_Value_On_Current_Null_And_Other_Not_Null(QuerySortOrderDirection direction, int expectedOutput)
    {
        // Arrange
        var data = new MyClass { Property = null };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), direction) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut.CompareTo(new OrderByWrapper<MyClass>(other, orderByFields, valueRetriever));

        // Assert
        actual.Should().Be(expectedOutput);
    }

    [Theory]
    [InlineData(QuerySortOrderDirection.Ascending, 1)]
    [InlineData(QuerySortOrderDirection.Descending, -1)]
    public void CompareTo_T_Returns_Correct_Value_On_Current_Not_Null_And_Other_Null(QuerySortOrderDirection direction, int expectedOutput)
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = null };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), direction) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut.CompareTo(new OrderByWrapper<MyClass>(other, orderByFields, valueRetriever));

        // Assert
        actual.Should().Be(expectedOutput);
    }

    [Theory]
    [InlineData(QuerySortOrderDirection.Ascending, 1)]
    [InlineData(QuerySortOrderDirection.Descending, -1)]
    public void CompareTo_T_Returns_Correct_Value_On_Current_Not_Null_And_Other_Not_Null(QuerySortOrderDirection direction, int expectedOutput)
    {
        // Arrange
        var data = new MyClass { Property = "b" };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), direction) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut.CompareTo(new OrderByWrapper<MyClass>(other, orderByFields, valueRetriever));

        // Assert
        actual.Should().Be(expectedOutput);
    }

    [Fact]
    public void CompareTo_T_Returns_Zero_When_OrderByFields_Dont_Give_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), QuerySortOrderDirection.Ascending) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut.CompareTo(new OrderByWrapper<MyClass>(other, orderByFields, valueRetriever));

        // Assert
        actual.Should().Be(0);
    }

    [Fact]
    public void CompareTo_Object_Returns_Correct_Result_When_Argument_Is_OrderByWrapper_Of_T()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "b" };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), QuerySortOrderDirection.Ascending) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut.CompareTo((object)new OrderByWrapper<MyClass>(other, orderByFields, valueRetriever));

        // Assert
        actual.Should().Be(-1);
    }

    [Fact]
    public void CompareTo_Object_Returns_Correct_Result_When_Argument_Is_Not_OrderByWrapper_Of_T()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "b" };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), QuerySortOrderDirection.Ascending) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut.CompareTo(other);

        // Assert
        actual.Should().Be(1);
    }

    [Fact]
    public void Equals_Object_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), QuerySortOrderDirection.Ascending) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut.Equals((object)new OrderByWrapper<MyClass>(other, orderByFields, valueRetriever));

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Equals_T_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), QuerySortOrderDirection.Ascending) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut.Equals(new OrderByWrapper<MyClass>(other, orderByFields, valueRetriever));

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Operator_Equals_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), QuerySortOrderDirection.Ascending) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut == new OrderByWrapper<MyClass>(other, orderByFields, valueRetriever);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Operator_NotEquals_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), QuerySortOrderDirection.Ascending) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut != new OrderByWrapper<MyClass>(other, orderByFields, valueRetriever);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void Operator_Smaller_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "b" };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), QuerySortOrderDirection.Ascending) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut < new OrderByWrapper<MyClass>(other, orderByFields, valueRetriever);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Operator_Greater_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "b" };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), QuerySortOrderDirection.Ascending) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut > new OrderByWrapper<MyClass>(other, orderByFields, valueRetriever);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void Operator_SmallerOrEqual_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "b" };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), QuerySortOrderDirection.Ascending) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut <= new OrderByWrapper<MyClass>(other, orderByFields, valueRetriever);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Operator_GreaterOrEqual_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "b" };
        var orderByFields = new[] { new QuerySortOrder(new QueryExpression(nameof(MyClass.Property), null), QuerySortOrderDirection.Ascending) };
        var valueRetriever = new DefaultExpressionEvaluator(new DefaultValueProvider(), Enumerable.Empty<IFunctionParser>());
        var sut = new OrderByWrapper<MyClass>(data, orderByFields, valueRetriever);

        // Act
        var actual = sut >= new OrderByWrapper<MyClass>(other, orderByFields, valueRetriever);

        // Assert
        actual.Should().BeFalse();
    }

    public class MyClass
    {
        public string? Property { get; set; }
    }
}

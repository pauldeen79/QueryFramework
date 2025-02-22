namespace QueryFramework.InMemory.Tests;

public class OrderByWrapperTests
{
    [Fact]
    public void CompareTo_T_Returns_Zero_When_OrderByFields_On_Two_Instances_Are_Both_Null()
    {
        // Arrange
        var data = new MyClass { Property = null };
        var other = new MyClass { Property = null };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Ascending).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut.CompareTo(new OrderByWrapper(other, orderByFields));

        // Assert
        actual.ShouldBe(0);
    }

    [Theory]
    [InlineData(QuerySortOrderDirection.Ascending, -1)]
    [InlineData(QuerySortOrderDirection.Descending, 1)]
    public void CompareTo_T_Returns_Correct_Value_On_Current_Null_And_Other_Not_Null(QuerySortOrderDirection direction, int expectedOutput)
    {
        // Arrange
        var data = new MyClass { Property = null };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(direction).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut.CompareTo(new OrderByWrapper(other, orderByFields));

        // Assert
        actual.ShouldBe(expectedOutput);
    }

    [Theory]
    [InlineData(QuerySortOrderDirection.Ascending, 1)]
    [InlineData(QuerySortOrderDirection.Descending, -1)]
    public void CompareTo_T_Returns_Correct_Value_On_Current_Not_Null_And_Other_Null(QuerySortOrderDirection direction, int expectedOutput)
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = null };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(direction).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut.CompareTo(new OrderByWrapper(other, orderByFields));

        // Assert
        actual.ShouldBe(expectedOutput);
    }

    [Theory]
    [InlineData(QuerySortOrderDirection.Ascending, 1)]
    [InlineData(QuerySortOrderDirection.Descending, -1)]
    public void CompareTo_T_Returns_Correct_Value_On_Current_Not_Null_And_Other_Not_Null(QuerySortOrderDirection direction, int expectedOutput)
    {
        // Arrange
        var data = new MyClass { Property = "b" };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(direction).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut.CompareTo(new OrderByWrapper(other, orderByFields));

        // Assert
        actual.ShouldBe(expectedOutput);
    }

    [Fact]
    public void CompareTo_T_Returns_Zero_When_OrderByFields_Gives_Empty_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Ascending).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut.CompareTo(new OrderByWrapper(other, orderByFields));

        // Assert
        actual.ShouldBe(0);
    }

    [Fact]
    public void CompareTo_Object_Returns_Correct_Result_When_Argument_Is_OrderByWrapper_Of_T()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "b" };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Ascending).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut.CompareTo((object)new OrderByWrapper(other, orderByFields));

        // Assert
        actual.ShouldBe(-1);
    }

    [Fact]
    public void CompareTo_Object_Returns_Correct_Result_When_Argument_Is_Not_OrderByWrapper_Of_T()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "b" };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Ascending).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut.CompareTo(other);

        // Assert
        actual.ShouldBe(1);
    }

    [Fact]
    public void Equals_Object_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Ascending).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut.Equals((object)new OrderByWrapper(other, orderByFields));

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void Equals_T_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Ascending).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut.Equals(new OrderByWrapper(other, orderByFields));

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void GetHashCode_Returns_Correct_Result_On_Equal_Objects()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Ascending).Build() };
        var sutData = new OrderByWrapper(data, orderByFields);
        var sutOther = new OrderByWrapper(other, orderByFields);

        // Act
        var actualHash = sutData.GetHashCode();
        var otherHash = sutOther.GetHashCode();

        // Assert
        actualHash.ShouldBe(otherHash);
    }

    [Fact]
    public void GetHashCode_Returns_Correct_Result_On_Unequal_Objects()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "a" };
        var orderByFieldsData = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Ascending).Build() };
        var orderByFieldsOther = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Descending).Build() };
        var sutData = new OrderByWrapper(data, orderByFieldsData);
        var sutOther = new OrderByWrapper(other, orderByFieldsOther);

        // Act
        var actualHash = sutData.GetHashCode();
        var otherHash = sutOther.GetHashCode();

        // Assert
        actualHash.ShouldNotBe(otherHash);
    }

    [Fact]
    public void Operator_Equals_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Ascending).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut == new OrderByWrapper(other, orderByFields);

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void Operator_NotEquals_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "a" };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Ascending).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut != new OrderByWrapper(other, orderByFields);

        // Assert
        actual.ShouldBeFalse();
    }

    [Fact]
    public void Operator_Smaller_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "b" };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Ascending).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut < new OrderByWrapper(other, orderByFields);

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void Operator_Greater_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "b" };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Ascending).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut > new OrderByWrapper(other, orderByFields);

        // Assert
        actual.ShouldBeFalse();
    }

    [Fact]
    public void Operator_SmallerOrEqual_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "b" };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Ascending).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut <= new OrderByWrapper(other, orderByFields);

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void Operator_GreaterOrEqual_Returns_Correct_Result()
    {
        // Arrange
        var data = new MyClass { Property = "a" };
        var other = new MyClass { Property = "b" };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)).WithOrder(QuerySortOrderDirection.Ascending).Build() };
        var sut = new OrderByWrapper(data, orderByFields);

        // Act
        var actual = sut >= new OrderByWrapper(other, orderByFields);

        // Assert
        actual.ShouldBeFalse();
    }

    public class MyClass
    {
        public string? Property { get; set; }
    }
}

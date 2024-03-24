namespace QueryFramework.Core.Tests.Extensions;

public class QueryBuilderExtensionsTests
{
    [Fact]
    public void Can_Use_Where_With_ComposableEvaluatableBuilder_To_Add_Condition()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Where(new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("field"))
                                                                 .WithOperator(new IsGreaterOperatorBuilder())
                                                                 .WithRightExpression(new ConstantExpressionBuilder().WithValue("value"))
                                                                 .WithStartGroup()
                                                                 .WithEndGroup()
                                                                 .WithCombination(Combination.Or));

        // Assert
        actual.Filter.Conditions.Should().HaveCount(1);
        var firstCondition = actual.Filter.Conditions.OfType<ComposableEvaluatableBuilder>().First();
        var field = firstCondition.LeftExpression as FieldExpressionBuilder;
        var value = (firstCondition.RightExpression as ConstantExpressionBuilder)?.Value;
        ((TypedConstantExpressionBuilder<string>)field!.FieldNameExpression).Value.Should().Be("field");
        firstCondition.Operator.Should().BeOfType<IsGreaterOperatorBuilder>();
        firstCondition.StartGroup.Should().BeTrue();
        firstCondition.EndGroup.Should().BeTrue();
        firstCondition.Combination.Should().Be(Combination.Or);
        value.Should().Be("value");
    }

    [Fact]
    public void Can_Use_OrderBy_With_FieldName_Strings_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.OrderBy("Field1", "Field2", "Field3");

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields[0].FieldNameExpression.Build().GetFieldName();
        var field1 = actual.OrderByFields[1].FieldNameExpression.Build().GetFieldName();
        var field2 = actual.OrderByFields[2].FieldNameExpression.Build().GetFieldName();
        field0.ToString().Should().Be("Field1");
        actual.OrderByFields[0].Order.Should().Be(QuerySortOrderDirection.Ascending);
        field1.ToString().Should().Be("Field2");
        actual.OrderByFields[1].Order.Should().Be(QuerySortOrderDirection.Ascending);
        field2.ToString().Should().Be("Field3");
        actual.OrderByFields[2].Order.Should().Be(QuerySortOrderDirection.Ascending);
    }

    [Fact]
    public void Can_Use_OrderByDescending_With_FieldName_Strings_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.OrderByDescending("Field1", "Field2", "Field3");

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields[0].FieldNameExpression.Build().GetFieldName();
        var field1 = actual.OrderByFields[1].FieldNameExpression.Build().GetFieldName();
        var field2 = actual.OrderByFields[2].FieldNameExpression.Build().GetFieldName();
        field0.ToString().Should().Be("Field1");
        actual.OrderByFields[0].Order.Should().Be(QuerySortOrderDirection.Descending);
        field1.ToString().Should().Be("Field2");
        actual.OrderByFields[1].Order.Should().Be(QuerySortOrderDirection.Descending);
        field2.ToString().Should().Be("Field3");
        actual.OrderByFields[2].Order.Should().Be(QuerySortOrderDirection.Descending);
    }

    [Fact]
    public void Can_Use_ThenBy_With_FieldName_Strings_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder().OrderBy("Field1");

        // Act
        var actual = sut.ThenBy("Field2", "Field3");

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields[0].FieldNameExpression.Build().GetFieldName();
        var field1 = actual.OrderByFields[1].FieldNameExpression.Build().GetFieldName();
        var field2 = actual.OrderByFields[2].FieldNameExpression.Build().GetFieldName();
        field0.ToString().Should().Be("Field1");
        actual.OrderByFields[0].Order.Should().Be(QuerySortOrderDirection.Ascending);
        field1.ToString().Should().Be("Field2");
        actual.OrderByFields[1].Order.Should().Be(QuerySortOrderDirection.Ascending);
        field2.ToString().Should().Be("Field3");
        actual.OrderByFields[2].Order.Should().Be(QuerySortOrderDirection.Ascending);
    }

    [Fact]
    public void Can_Use_ThenByDescending_With_FieldName_Strings_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder().OrderBy("Field1", "Field2");

        // Act
        var actual = sut.ThenByDescending("Field3");

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields[0].FieldNameExpression.Build().GetFieldName();
        var field1 = actual.OrderByFields[1].FieldNameExpression.Build().GetFieldName();
        var field2 = actual.OrderByFields[2].FieldNameExpression.Build().GetFieldName();
        field0.ToString().Should().Be("Field1");
        actual.OrderByFields[0].Order.Should().Be(QuerySortOrderDirection.Ascending);
        field1.ToString().Should().Be("Field2");
        actual.OrderByFields[1].Order.Should().Be(QuerySortOrderDirection.Ascending);
        field2.ToString().Should().Be("Field3");
        actual.OrderByFields[2].Order.Should().Be(QuerySortOrderDirection.Descending);
    }

    [Fact]
    public void Can_Use_Offset_To_Set_Offset()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Offset(100);

        // Assert
        actual.Offset.Should().Be(100);
    }

    [Fact]
    public void Can_Use_Limit_To_Set_Limit()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Limit(200);

        // Assert
        actual.Limit.Should().Be(200);
    }

    [Fact]
    public void Can_Use_Skip_To_Set_Offset()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Skip(100);

        // Assert
        actual.Offset.Should().Be(100);
    }

    [Fact]
    public void Can_Use_Take_To_Set_Limit()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Take(200);

        // Assert
        actual.Limit.Should().Be(200);
    }

    [Fact]
    public void Can_Use_Where_With_Value_To_Add_Condition()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Where("field").IsEqualTo("value");

        // Assert
        actual.Filter.Conditions.Should().HaveCount(1);
        var firstCondition = actual.Filter.Conditions.OfType<ComposableEvaluatableBuilder>().First();
        var field = firstCondition.LeftExpression as FieldExpressionBuilder;
        var value = (firstCondition.RightExpression as ConstantExpressionBuilder)?.Value;
        ((TypedConstantExpressionBuilder<string>)field!.FieldNameExpression).Value.Should().Be("field");
        firstCondition.Operator.Should().BeOfType<EqualsOperatorBuilder>();
        value.Should().Be("value");
    }

    [Fact]
    public void Can_Use_Where_Without_Value_To_Add_Condition()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Where("field").IsNull();

        // Assert
        actual.Filter.Conditions.Should().HaveCount(1);
        var firstCondition = actual.Filter.Conditions.OfType<ComposableEvaluatableBuilder>().First();
        var field = firstCondition.LeftExpression as FieldExpressionBuilder;
        ((TypedConstantExpressionBuilder<string>)field!.FieldNameExpression).Value.Should().Be("field");
        firstCondition.Operator.Should().BeOfType<IsNullOperatorBuilder>();
        firstCondition.RightExpression.Should().BeOfType<EmptyExpressionBuilder>();
    }
}

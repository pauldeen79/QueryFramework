namespace QueryFramework.Core.Tests.Extensions;

public class SingleEntityQueryBuilderExtensionsTests
{
    [Fact]
    public void Can_Use_Where_With_QueryConditionBuilder_To_Add_Condition()
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
        ((ConstantExpressionBuilder)field!.FieldNameExpression).Value.Should().Be("field");
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
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field0!.FieldNameExpression).Value.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field1!.FieldNameExpression).Value.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field2!.FieldNameExpression).Value.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Ascending);
    }

    [Fact]
    public void Can_Use_OrderBy_With_QueryExpressionBuilders_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.OrderBy(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field1"),
                                 new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field2"),
                                 new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field3"));

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field0!.FieldNameExpression).Value.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field1!.FieldNameExpression).Value.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field2!.FieldNameExpression).Value.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Ascending);
    }

    [Fact]
    public void Can_Use_OrderBy_With_QuerySortOrderBuilders_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.OrderBy(new QuerySortOrderBuilder().WithField("Field1"),
                                 new QuerySortOrderBuilder().WithField("Field2"),
                                 new QuerySortOrderBuilder().WithField("Field3").WithOrder(QuerySortOrderDirection.Descending));

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field0!.FieldNameExpression).Value.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field1!.FieldNameExpression).Value.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field2!.FieldNameExpression).Value.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
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
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field0!.FieldNameExpression).Value.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
        ((ConstantExpressionBuilder)field1!.FieldNameExpression).Value.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Descending);
        ((ConstantExpressionBuilder)field2!.FieldNameExpression).Value.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
    }

    [Fact]
    public void Can_Use_OrderByDescending_With_QueryExpressionBuilders_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.OrderByDescending(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field1"),
                                           new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field2"),
                                           new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field3"));

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field0!.FieldNameExpression).Value.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
        ((ConstantExpressionBuilder)field1!.FieldNameExpression).Value.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Descending);
        ((ConstantExpressionBuilder)field2!.FieldNameExpression).Value.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
    }

    [Fact]
    public void Can_Use_OrderByDescending_With_QuerySortOrderBuilders_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.OrderByDescending(new QuerySortOrderBuilder().WithField("Field1"),
                                           new QuerySortOrderBuilder().WithField("Field2"),
                                           new QuerySortOrderBuilder().WithField("Field3"));

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field0!.FieldNameExpression).Value.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
        ((ConstantExpressionBuilder)field1!.FieldNameExpression).Value.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Descending);
        ((ConstantExpressionBuilder)field2!.FieldNameExpression).Value.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
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
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field0!.FieldNameExpression).Value.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field1!.FieldNameExpression).Value.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field2!.FieldNameExpression).Value.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Ascending);
    }

    [Fact]
    public void Can_Use_ThenBy_With_QueryExpressionBuilders_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder().OrderBy(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field1"));

        // Act
        var actual = sut.ThenBy(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field2"),
                                new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field3"));

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field0!.FieldNameExpression).Value.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field1!.FieldNameExpression).Value.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field2!.FieldNameExpression).Value.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Ascending);
    }

    [Fact]
    public void Can_Use_ThenBy_With_QuerySortOrderBuilders_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder().OrderBy(new QuerySortOrderBuilder().WithField("Field1"));

        // Act
        var actual = sut.ThenBy(new QuerySortOrderBuilder().WithField("Field2"),
                                new QuerySortOrderBuilder().WithField("Field3").WithOrder(QuerySortOrderDirection.Descending));

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field0!.FieldNameExpression).Value.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field1!.FieldNameExpression).Value.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field2!.FieldNameExpression).Value.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
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
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field0!.FieldNameExpression).Value.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field1!.FieldNameExpression).Value.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field2!.FieldNameExpression).Value.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
    }

    [Fact]
    public void Can_Use_ThenByDescending_With_QueryExpressionBuilders_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder().OrderByDescending(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field1"))
                                                .ThenBy(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field2"));

        // Act
        var actual = sut.ThenByDescending(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field3"));

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field0!.FieldNameExpression).Value.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
        ((ConstantExpressionBuilder)field1!.FieldNameExpression).Value.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field2!.FieldNameExpression).Value.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
    }

    [Fact]
    public void Can_Use_ThenByDescending_With_QuerySortOrderBuilders_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder().OrderBy(new QuerySortOrderBuilder().WithField("Field1"),
                                                         new QuerySortOrderBuilder().WithField("Field2"));

        // Act
        var actual = sut.ThenByDescending(new QuerySortOrderBuilder().WithField("Field3"));

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        ((ConstantExpressionBuilder)field0!.FieldNameExpression).Value.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field1!.FieldNameExpression).Value.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        ((ConstantExpressionBuilder)field2!.FieldNameExpression).Value.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
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
}

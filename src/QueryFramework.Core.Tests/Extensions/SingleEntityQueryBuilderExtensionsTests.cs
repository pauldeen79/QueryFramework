namespace QueryFramework.Core.Tests.Extensions;

public class SingleEntityQueryBuilderExtensionsTests
{
    [Fact]
    public void Can_Use_Where_With_QueryConditionBuilder_To_Add_Condition()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Where(new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName("field"))
                                                     .WithOperator(Operator.Greater)
                                                     .WithRightExpression(new ConstantExpressionBuilder().WithValue("value"))
                                                     .WithStartGroup()
                                                     .WithEndGroup()
                                                     .WithCombination(Combination.Or));

        // Assert
        var field = actual.Conditions.First().LeftExpression as FieldExpressionBuilder;
        var value = (actual.Conditions.First().RightExpression as ConstantExpressionBuilder)?.Value;
        actual.Conditions.Should().HaveCount(1);
        field?.FieldName.Should().Be("field");
        actual.Conditions.First().Operator.Should().Be(Operator.Greater);
        actual.Conditions.First().StartGroup.Should().BeTrue();
        actual.Conditions.First().EndGroup.Should().BeTrue();
        actual.Conditions.First().Combination.Should().Be(Combination.Or);
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
        field0?.FieldName.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field1?.FieldName.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field2?.FieldName.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Ascending);
    }

    [Fact]
    public void Can_Use_OrderBy_With_QueryExpressionBuilders_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.OrderBy(new FieldExpressionBuilder().WithFieldName("Field1"),
                                 new FieldExpressionBuilder().WithFieldName("Field2"),
                                 new FieldExpressionBuilder().WithFieldName("Field3"));

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        field0?.FieldName.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field1?.FieldName.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field2?.FieldName.Should().Be("Field3");
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
        field0?.FieldName.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field1?.FieldName.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field2?.FieldName.Should().Be("Field3");
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
        field0?.FieldName.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
        field1?.FieldName.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Descending);
        field2?.FieldName.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
    }

    [Fact]
    public void Can_Use_OrderByDescending_With_QueryExpressionBuilders_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.OrderByDescending(new FieldExpressionBuilder().WithFieldName("Field1"),
                                           new FieldExpressionBuilder().WithFieldName("Field2"),
                                           new FieldExpressionBuilder().WithFieldName("Field3"));

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        field0?.FieldName.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
        field1?.FieldName.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Descending);
        field2?.FieldName.Should().Be("Field3");
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
        field0?.FieldName.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
        field1?.FieldName.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Descending);
        field2?.FieldName.Should().Be("Field3");
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
        field0?.FieldName.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field1?.FieldName.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field2?.FieldName.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Ascending);
    }

    [Fact]
    public void Can_Use_ThenBy_With_QueryExpressionBuilders_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder().OrderBy(new FieldExpressionBuilder().WithFieldName("Field1"));

        // Act
        var actual = sut.ThenBy(new FieldExpressionBuilder().WithFieldName("Field2"),
                                new FieldExpressionBuilder().WithFieldName("Field3"));

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        field0?.FieldName.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field1?.FieldName.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field2?.FieldName.Should().Be("Field3");
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
        field0?.FieldName.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field1?.FieldName.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field2?.FieldName.Should().Be("Field3");
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
        field0?.FieldName.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field1?.FieldName.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field2?.FieldName.Should().Be("Field3");
        actual.OrderByFields.ElementAt(2).Order.Should().Be(QuerySortOrderDirection.Descending);
    }

    [Fact]
    public void Can_Use_ThenByDescending_With_QueryExpressionBuilders_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder().OrderByDescending(new FieldExpressionBuilder().WithFieldName("Field1"))
                                                .ThenBy(new FieldExpressionBuilder().WithFieldName("Field2"));

        // Act
        var actual = sut.ThenByDescending(new FieldExpressionBuilder().WithFieldName("Field3"));

        // Assert
        actual.OrderByFields.Should().HaveCount(3);
        var field0 = actual.OrderByFields.ElementAt(0).Field as FieldExpressionBuilder;
        var field1 = actual.OrderByFields.ElementAt(1).Field as FieldExpressionBuilder;
        var field2 = actual.OrderByFields.ElementAt(2).Field as FieldExpressionBuilder;
        field0?.FieldName.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Descending);
        field1?.FieldName.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field2?.FieldName.Should().Be("Field3");
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
        field0?.FieldName.Should().Be("Field1");
        actual.OrderByFields.ElementAt(0).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field1?.FieldName.Should().Be("Field2");
        actual.OrderByFields.ElementAt(1).Order.Should().Be(QuerySortOrderDirection.Ascending);
        field2?.FieldName.Should().Be("Field3");
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

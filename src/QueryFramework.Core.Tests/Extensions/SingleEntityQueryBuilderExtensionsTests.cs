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
                                                     /*.WithOpenBracket()
                                                     .WithCloseBracket()
                                                     .WithCombination(QueryCombination.Or)*/);

        // Assert
        var field = actual.Conditions.First().LeftExpression as FieldExpressionBuilder;
        var value = (actual.Conditions.First().RightExpression as ConstantExpressionBuilder)?.Value;
        actual.Conditions.Should().HaveCount(1);
        field?.FieldName.Should().Be("field");
        actual.Conditions.First().Operator.Should().Be(Operator.Greater);
        value.Should().Be("value");
        //actual.Conditions.First().OpenBracket.Should().BeTrue();
        //actual.Conditions.First().CloseBracket.Should().BeTrue();
        //actual.Conditions.First().Combination.Should().Be(QueryCombination.Or);
    }

    //[Fact]
    //public void Can_Use_Or_With_QueryConditionBuilder_To_Add_Condition()
    //{
    //    // Arrange
    //    var sut = new SingleEntityQueryBuilder();

    //    // Act
    //    var actual = sut.Or(new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName("field"))
    //                                              .WithOperator(Operator.Greater)
    //                                              .WithRightExpression(new ConstantExpressionBuilder().WithValue("value"))
    //                                              /*.WithOpenBracket()
    //                                              .WithCloseBracket()
    //                                              .WithCombination(QueryCombination.And))*/;

    //    // Assert
    //    actual.Conditions.Should().HaveCount(1);
    //    actual.Conditions.First().Field.FieldName.Should().Be("field");
    //    actual.Conditions.First().Operator.Should().Be(Operator.Greater);
    //    actual.Conditions.First().Value.Should().Be("value");
    //    actual.Conditions.First().OpenBracket.Should().BeTrue();
    //    actual.Conditions.First().CloseBracket.Should().BeTrue();
    //    actual.Conditions.First().Combination.Should().Be(QueryCombination.Or);
    //}

    //[Fact]
    //public void Can_Use_And_With_QueryConditionBuilder_To_Add_Condition()
    //{
    //    // Arrange
    //    var sut = new SingleEntityQueryBuilder();

    //    // Act
    //    var actual = sut.And(new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName("field"))
    //                                                    .WithOperator(Operator.Greater)
    //                                                    .WithRightExpression(new ConstantExpressionBuilder().WithValue("value"))
    //                                                    .WithOpenBracket()
    //                                                    .WithCloseBracket()
    //                                                    .WithCombination(QueryCombination.Or));

    //    // Assert
    //    actual.Conditions.Should().HaveCount(1);
    //    actual.Conditions.First().Field.FieldName.Should().Be("field");
    //    actual.Conditions.First().Operator.Should().Be(Operator.Greater);
    //    actual.Conditions.First().Value.Should().Be("value");
    //    actual.Conditions.First().OpenBracket.Should().BeTrue();
    //    actual.Conditions.First().CloseBracket.Should().BeTrue();
    //    actual.Conditions.First().Combination.Should().Be(QueryCombination.And);
    //}

    //[Fact]
    //public void Can_Use_AndAny_With_QueryConditionBuilders_To_Add_Conditions()
    //{
    //    // Arrange
    //    var sut = new SingleEntityQueryBuilder();

    //    // Act
    //    var actual = sut.AndAny
    //    (
    //        new ConditionBuilder().WithField("field1").WithOperator(Operator.Greater).WithValue("value1"),
    //        new ConditionBuilder().WithField("field2").WithOperator(Operator.Greater).WithValue("value2")
    //    );

    //    // Assert
    //    actual.Conditions.Should().HaveCount(2);
    //    actual.Conditions.First().Field.FieldName.Should().Be("field1");
    //    actual.Conditions.First().Operator.Should().Be(Operator.Greater);
    //    actual.Conditions.First().Value.Should().Be("value1");
    //    actual.Conditions.First().OpenBracket.Should().BeTrue();
    //    actual.Conditions.First().CloseBracket.Should().BeFalse();
    //    actual.Conditions.First().Combination.Should().Be(QueryCombination.And);
    //    actual.Conditions.Last().Field.FieldName.Should().Be("field2");
    //    actual.Conditions.Last().Operator.Should().Be(Operator.Greater);
    //    actual.Conditions.Last().Value.Should().Be("value2");
    //    actual.Conditions.Last().OpenBracket.Should().BeFalse();
    //    actual.Conditions.Last().CloseBracket.Should().BeTrue();
    //    actual.Conditions.Last().Combination.Should().Be(QueryCombination.Or);
    //}

    //[Fact]
    //public void Can_Use_OrAll_With_QueryConditionBuilders_To_Add_Conditions()
    //{
    //    // Arrange
    //    var sut = new SingleEntityQueryBuilder();

    //    // Act
    //    var actual = sut.OrAll
    //    (
    //        new ConditionBuilder().WithField("field1").WithOperator(Operator.Greater).WithValue("value1"),
    //        new ConditionBuilder().WithField("field2").WithOperator(Operator.Greater).WithValue("value2")
    //    );

    //    // Assert
    //    actual.Conditions.Should().HaveCount(2);
    //    actual.Conditions.First().Field.FieldName.Should().Be("field1");
    //    actual.Conditions.First().Operator.Should().Be(Operator.Greater);
    //    actual.Conditions.First().Value.Should().Be("value1");
    //    actual.Conditions.First().OpenBracket.Should().BeTrue();
    //    actual.Conditions.First().CloseBracket.Should().BeFalse();
    //    actual.Conditions.First().Combination.Should().Be(QueryCombination.Or);
    //    actual.Conditions.Last().Field.FieldName.Should().Be("field2");
    //    actual.Conditions.Last().Operator.Should().Be(Operator.Greater);
    //    actual.Conditions.Last().Value.Should().Be("value2");
    //    actual.Conditions.Last().OpenBracket.Should().BeFalse();
    //    actual.Conditions.Last().CloseBracket.Should().BeTrue();
    //    actual.Conditions.Last().Combination.Should().Be(QueryCombination.And);
    //}

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

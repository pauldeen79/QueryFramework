namespace QueryFramework.Core.Tests.Extensions;

public class QueryBuilderExtensionsTests
{
    [Fact]
    public void Can_Use_Where_With_ComposableEvaluatableBuilder_To_Add_Condition_Using_FieldName()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Where("field")
            .WithStartGroup()
            .WithEndGroup()
            .WithCombination(Combination.Or)
            .IsGreaterThan("value");

        // Assert
        actual.Filter.Conditions.Count.ShouldBe(1);
        var firstCondition = actual.Filter.Conditions.OfType<ComposableEvaluatableBuilder>().First();
        var field = firstCondition.LeftExpression as FieldExpressionBuilder;
        var value = (firstCondition.RightExpression as TypedConstantExpressionBuilder<string>)?.Value;
        ((TypedConstantExpressionBuilder<string>)field!.FieldNameExpression).Value.ShouldBe("field");
        firstCondition.Operator.ShouldBeOfType<IsGreaterOperatorBuilder>();
        firstCondition.StartGroup.ShouldBeTrue();
        firstCondition.EndGroup.ShouldBeTrue();
        firstCondition.Combination.ShouldBe(Combination.Or);
        value.ShouldBe("value");
    }

    [Fact]
    public void Can_Use_Where_With_ComposableEvaluatableBuilder_To_Add_Condition_Using_Expression()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Where(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("field"))
            .WithStartGroup()
            .WithEndGroup()
            .WithCombination(Combination.Or)
            .IsGreaterThan("value");

        // Assert
        actual.Filter.Conditions.Count.ShouldBe(1);
        var firstCondition = actual.Filter.Conditions.OfType<ComposableEvaluatableBuilder>().First();
        var field = firstCondition.LeftExpression as FieldExpressionBuilder;
        var value = (firstCondition.RightExpression as TypedConstantExpressionBuilder<string>)?.Value;
        ((TypedConstantExpressionBuilder<string>)field!.FieldNameExpression).Value.ShouldBe("field");
        firstCondition.Operator.ShouldBeOfType<IsGreaterOperatorBuilder>();
        firstCondition.StartGroup.ShouldBeTrue();
        firstCondition.EndGroup.ShouldBeTrue();
        firstCondition.Combination.ShouldBe(Combination.Or);
        value.ShouldBe("value");
    }

    [Fact]
    public void Can_Use_OrderBy_With_FieldName_Strings_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.OrderBy("Field1", "Field2", "Field3");

        // Assert
        AssertOrderBy(actual);
    }

    [Fact]
    public void Can_Use_Result_Of_OrderBy_To_Convert_To_Entity_And_Back_To_Builder()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.OrderBy("Field1", "Field2", "Field3").BuildTyped().ToTypedBuilder();

        // Assert
        AssertOrderBy(actual);
    }

    [Fact]
    public void Can_Use_OrderByDescending_With_FieldName_Strings_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.OrderByDescending("Field1", "Field2", "Field3");

        // Assert
        actual.OrderByFields.Count.ShouldBe(3);
        var field0 = actual.OrderByFields[0].FieldNameExpression.Build().GetFieldName();
        var field1 = actual.OrderByFields[1].FieldNameExpression.Build().GetFieldName();
        var field2 = actual.OrderByFields[2].FieldNameExpression.Build().GetFieldName();
        field0.ToString().ShouldBe("Field1");
        actual.OrderByFields[0].Order.ShouldBe(QuerySortOrderDirection.Descending);
        field1.ToString().ShouldBe("Field2");
        actual.OrderByFields[1].Order.ShouldBe(QuerySortOrderDirection.Descending);
        field2.ToString().ShouldBe("Field3");
        actual.OrderByFields[2].Order.ShouldBe(QuerySortOrderDirection.Descending);
    }

    [Fact]
    public void Can_Use_ThenBy_With_FieldName_Strings_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder().OrderBy("Field1");

        // Act
        var actual = sut.ThenBy("Field2", "Field3");

        // Assert
        actual.OrderByFields.Count.ShouldBe(3);
        var field0 = actual.OrderByFields[0].FieldNameExpression.Build().GetFieldName();
        var field1 = actual.OrderByFields[1].FieldNameExpression.Build().GetFieldName();
        var field2 = actual.OrderByFields[2].FieldNameExpression.Build().GetFieldName();
        field0.ToString().ShouldBe("Field1");
        actual.OrderByFields[0].Order.ShouldBe(QuerySortOrderDirection.Ascending);
        field1.ToString().ShouldBe("Field2");
        actual.OrderByFields[1].Order.ShouldBe(QuerySortOrderDirection.Ascending);
        field2.ToString().ShouldBe("Field3");
        actual.OrderByFields[2].Order.ShouldBe(QuerySortOrderDirection.Ascending);
    }

    [Fact]
    public void Can_Use_ThenByDescending_With_FieldName_Strings_To_Add_OrderByClauses()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder().OrderBy("Field1", "Field2");

        // Act
        var actual = sut.ThenByDescending("Field3");

        // Assert
        actual.OrderByFields.Count.ShouldBe(3);
        var field0 = actual.OrderByFields[0].FieldNameExpression.Build().GetFieldName();
        var field1 = actual.OrderByFields[1].FieldNameExpression.Build().GetFieldName();
        var field2 = actual.OrderByFields[2].FieldNameExpression.Build().GetFieldName();
        field0.ToString().ShouldBe("Field1");
        actual.OrderByFields[0].Order.ShouldBe(QuerySortOrderDirection.Ascending);
        field1.ToString().ShouldBe("Field2");
        actual.OrderByFields[1].Order.ShouldBe(QuerySortOrderDirection.Ascending);
        field2.ToString().ShouldBe("Field3");
        actual.OrderByFields[2].Order.ShouldBe(QuerySortOrderDirection.Descending);
    }

    [Fact]
    public void Can_Use_Offset_To_Set_Offset()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Offset(100);

        // Assert
        actual.Offset.ShouldBe(100);
    }

    [Fact]
    public void Can_Use_Limit_To_Set_Limit()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Limit(200);

        // Assert
        actual.Limit.ShouldBe(200);
    }

    [Fact]
    public void Can_Use_Skip_To_Set_Offset()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Skip(100);

        // Assert
        actual.Offset.ShouldBe(100);
    }

    [Fact]
    public void Can_Use_Take_To_Set_Limit()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Take(200);

        // Assert
        actual.Limit.ShouldBe(200);
    }

    [Fact]
    public void Can_Use_Where_With_Value_To_Add_Condition()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Where("field").IsEqualTo("value");

        // Assert
        actual.Filter.Conditions.Count.ShouldBe(1);
        var firstCondition = actual.Filter.Conditions.OfType<ComposableEvaluatableBuilder>().First();
        var field = firstCondition.LeftExpression as FieldExpressionBuilder;
        var value = (firstCondition.RightExpression as TypedConstantExpressionBuilder<string>)?.Value;
        ((TypedConstantExpressionBuilder<string>)field!.FieldNameExpression).Value.ShouldBe("field");
        firstCondition.Operator.ShouldBeOfType<EqualsOperatorBuilder>();
        value.ShouldBe("value");
    }

    [Fact]
    public void Can_Use_Where_Without_Value_To_Add_Condition()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();

        // Act
        var actual = sut.Where("field").IsNull();

        // Assert
        actual.Filter.Conditions.Count.ShouldBe(1);
        var firstCondition = actual.Filter.Conditions.OfType<ComposableEvaluatableBuilder>().First();
        var field = firstCondition.LeftExpression as FieldExpressionBuilder;
        ((TypedConstantExpressionBuilder<string>)field!.FieldNameExpression).Value.ShouldBe("field");
        firstCondition.Operator.ShouldBeOfType<IsNullOperatorBuilder>();
        firstCondition.RightExpression.ShouldBeOfType<EmptyExpressionBuilder>();
    }

    private static void AssertOrderBy(SingleEntityQueryBuilder actual)
    {
        actual.OrderByFields.Count.ShouldBe(3);
        var field0 = actual.OrderByFields[0].FieldNameExpression.Build().GetFieldName();
        var field1 = actual.OrderByFields[1].FieldNameExpression.Build().GetFieldName();
        var field2 = actual.OrderByFields[2].FieldNameExpression.Build().GetFieldName();
        field0.ToString().ShouldBe("Field1");
        actual.OrderByFields[0].Order.ShouldBe(QuerySortOrderDirection.Ascending);
        field1.ToString().ShouldBe("Field2");
        actual.OrderByFields[1].Order.ShouldBe(QuerySortOrderDirection.Ascending);
        field2.ToString().ShouldBe("Field3");
        actual.OrderByFields[2].Order.ShouldBe(QuerySortOrderDirection.Ascending);
    }
}

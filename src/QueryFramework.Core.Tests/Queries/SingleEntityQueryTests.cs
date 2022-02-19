namespace QueryFramework.Core.Tests.Queries;

public class SingleEntityQueryTests
{
    [Fact]
    public void Can_Construct_SingleEntityQuery_With_Default_Values()
    {
        // Act
        var sut = new SingleEntityQuery();

        // Assert
        sut.Conditions.Should().BeEmpty();
        sut.Limit.Should().BeNull();
        sut.Offset.Should().BeNull();
        sut.OrderByFields.Should().BeEmpty();
    }

    [Fact]
    public void Can_Construct_SingleEntityQuery_With_Custom_Values()
    {
        // Arrange
        var conditions = new[] { new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName("field"))
                                                       .WithOperator(Operator.Equal)
                                                       .WithRightExpression(new ConstantExpressionBuilder().WithValue("value"))
                                                       .Build() };
        var orderByFields = new[] { new QuerySortOrderBuilder().WithField(new FieldExpressionBuilder().WithFieldName("field"))
                                                               .WithOrder(QuerySortOrderDirection.Ascending)
                                                               .Build() };
        var limit = 1;
        var offset = 2;

        // Act
        var sut = new SingleEntityQuery(limit, offset, conditions, orderByFields);

        // Assert
        sut.Conditions.Should().BeEquivalentTo(conditions);
        sut.Limit.Should().Be(limit);
        sut.Offset.Should().Be(offset);
        sut.OrderByFields.Should().BeEquivalentTo(orderByFields);
    }

    //[Fact]
    //public void Constructing_SingleEntityQuery_With_ValidationError_Leads_To_Exception()
    //{
    //    // Arrange
    //    var conditions = new[]
    //    {
    //        new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName("field"))
    //                              .WithOperator(Operator.Equal)
    //                              .Build()
    //    };
    //    var action = new Action(() => _ = new FieldSelectionQuery(null, null, false, true, conditions, Enumerable.Empty<IQuerySortOrder>(), Enumerable.Empty<IExpression>()));

    //    // Act
    //    action.Should().Throw<ValidationException>();
    //}

    [Fact]
    public void Can_Compare_SingleEntityQuery_With_Equal_Values()
    {
        // Arrange
        var q1 = new SingleEntityQuery(5, 64, new[] { new ConditionBuilder()
                                                        .WithLeftExpression(new FieldExpressionBuilder().WithFieldName("Field"))
                                                        .WithOperator(Operator.Equal)
                                                        .WithRightExpression(new ConstantExpressionBuilder().WithValue("A"))
                                                        .Build() }, Enumerable.Empty<IQuerySortOrder>());
        var q2 = new SingleEntityQuery(5, 64, new[] { new ConditionBuilder()
                                                        .WithLeftExpression(new FieldExpressionBuilder().WithFieldName("Field"))
                                                        .WithOperator(Operator.Equal)
                                                        .WithRightExpression(new ConstantExpressionBuilder().WithValue("A"))
                                                        .Build() }, Enumerable.Empty<IQuerySortOrder>());

        // Act
        var actual = q1.Equals(q2);

        // Asset
        actual.Should().BeTrue();
    }
}

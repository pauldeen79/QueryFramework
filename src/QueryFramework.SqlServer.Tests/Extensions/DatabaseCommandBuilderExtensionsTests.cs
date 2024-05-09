namespace QueryFramework.SqlServer.Tests.Extensions;

public class DatabaseCommandBuilderExtensionsTests
{
    private readonly PagedSelectCommandBuilder _builder;
    private readonly IPagedDatabaseEntityRetrieverSettings _settingsMock;
    private readonly IQueryFieldInfo _fieldInfoMock;
    private readonly IGroupingQuery _queryMock;
    private readonly ISqlExpressionEvaluator _evaluatorMock;
    private readonly ParameterBag _parameterBag;

    public DatabaseCommandBuilderExtensionsTests()
    {
        _builder = new PagedSelectCommandBuilder();
        _settingsMock = Substitute.For<IPagedDatabaseEntityRetrieverSettings>();
        _settingsMock.Fields
                     .Returns(string.Empty);
        _fieldInfoMock = Substitute.For<IQueryFieldInfo>();
        _fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>())
                      .Returns(x => x.ArgAt<string>(0));
        _queryMock = Substitute.For<IGroupingQuery>();
        _queryMock.GroupByFields
                  .Returns(new ReadOnlyValueCollection<Expression>());
        _queryMock.GroupByFilter
                  .Returns(new ComposedEvaluatable(new ReadOnlyValueCollection<ComposableEvaluatable>()));
        _evaluatorMock = Substitute.For<ISqlExpressionEvaluator>();
        _parameterBag = new ParameterBag();
        DefaultSqlExpressionEvaluatorHelper.UseRealSqlExpressionEvaluator(_evaluatorMock, _parameterBag);
    }

    [Fact]
    public void Select_Uses_Star_When_Fields_Is_Empty_And_GetAllFields_Returns_Null_And_SelectAll_Is_True()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().SelectAll().BuildTyped();
        _fieldInfoMock.GetAllFields()
                      .Returns(Enumerable.Empty<string>());
        _builder.From("MyTable");

        // Act
        var actual = _builder.Select(_settingsMock, _fieldInfoMock, query, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void Select_Uses_GetAllFields_When_Provided()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().SelectAll().BuildTyped();
        _fieldInfoMock.GetAllFields()
                      .Returns(new[] { "Field1", "Field2", "Field3" });
        _builder.From("MyTable");

        // Act
        var actual = _builder.Select(_settingsMock, _fieldInfoMock, query, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT Field1, Field2, Field3 FROM MyTable");
    }

    [Fact]
    public void Select_Uses_Fields_From_Query_When_GetAllFields_Is_False()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").BuildTyped();
        _builder.From("MyTable");

        // Act
        var actual = _builder.Select(_settingsMock, _fieldInfoMock, query, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT Field1, Field2, Field3 FROM MyTable");
    }

    [Fact]
    public void Select_Uses_GetFieldDelegate_When_Result_Is_Not_Null()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").BuildTyped();
        _fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>())
                      .Returns(x => x.ArgAt<string>(0) + "A");
        _builder.From("MyTable");

        // Act
        var actual = _builder.Select(_settingsMock, _fieldInfoMock, query, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT Field1A, Field2A, Field3A FROM MyTable");
    }

    [Fact]
    public void Where_Does_Not_Append_Anything_When_Conditions_And_DefaultWhere_Are_Both_Empty()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().BuildTyped();
        _builder.From("MyTable");

        // Act
        var actual = _builder.Where(query, _settingsMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void Where_Adds_Single_Condition_Without_Default_Where_Clause()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Where("Field").IsEqualTo("value").BuildTyped();
        _builder.From("MyTable");

        // Act
        var actual = _builder.Where(query, _settingsMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field = @p0");
    }

    [Fact]
    public void Where_Adds_Single_Condition_With_Default_Where_Clause()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Where("Field").IsEqualTo("value").BuildTyped();
        _settingsMock.DefaultWhere.Returns("Field IS NOT NULL");
        _builder.From("MyTable");

        // Act
        var actual = _builder.Where(query, _settingsMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field IS NOT NULL AND Field = @p0");
    }

    [Fact]
    public void Where_Adds_Multiple_Conditions_Without_Default_Where_Clause()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Where("Field").IsEqualTo("value")
                                                    .And("Field2").IsNotNull()
                                                    .BuildTyped();
        _builder.From("MyTable");

        // Act
        var actual = _builder.Where(query, _settingsMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field = @p0 AND Field2 IS NOT NULL");
    }

    [Fact]
    public void Where_Adds_Multiple_Conditions_With_Default_Where_Clause()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Where("Field").IsEqualTo("value").And("Field2").IsNotNull().BuildTyped();
        _settingsMock.DefaultWhere.Returns("Field IS NOT NULL");
        _builder.From("MyTable");

        // Act
        var actual = _builder.Where(query, _settingsMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field IS NOT NULL AND Field = @p0 AND Field2 IS NOT NULL");
    }

    [Fact]
    public void OrderBy_Does_Not_Append_Anything_When_Limit_And_Offset_Are_Both_Filled()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().OrderBy("Field").Limit(10).Offset(20).BuildTyped();
        _settingsMock.DefaultOrderBy.Returns("Ignored");
        _builder.From("MyTable");

        // Act
        var actual = _builder.OrderBy(query, _settingsMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void OrderBy_Does_Not_Append_Anything_When_Query_OrderByFields_Is_Empty()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().BuildTyped();
        _builder.From("MyTable");

        // Act
        var actual = _builder.OrderBy(query, _settingsMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void OrderBy_Appends_Single_OrderBy_Clause()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().OrderBy("Field").BuildTyped();
        _builder.From("MyTable");

        // Act
        var actual = _builder.OrderBy(query, _settingsMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY Field ASC");
    }

    [Fact]
    public void OrderBy_Appends_Multiple_OrderBy_Clauses()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().OrderBy("Field1").ThenByDescending("Field2").BuildTyped();
        _builder.From("MyTable");

        // Act
        var actual = _builder.OrderBy(query, _settingsMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY Field1 ASC, Field2 DESC");
    }

    [Fact]
    public void OrderBy_Appends_Default_OrderBy_Clause()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().BuildTyped();
        _settingsMock.DefaultOrderBy.Returns("Field ASC");
        _builder.From("MyTable");

        // Act
        var actual = _builder.OrderBy(query, _settingsMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY Field ASC");
    }

    [Fact]
    public void OrderBy_Does_Not_Append_DefaultOrderBy_When_OrderBy_Clause_Is_Present_On_Query()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().OrderBy("Field").BuildTyped();
        _settingsMock.DefaultOrderBy.Returns("Ignored");
        _builder.From("MyTable");

        // Act
        var actual = _builder.OrderBy(query, _settingsMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY Field ASC");
    }

    [Fact]
    public void OrderBy_Appends_Single_OrderBy_Clause_With_GetDatabaseFieldName()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().OrderBy("Field").BuildTyped();
        _fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>())
                      .Returns(x => x.ArgAt<string>(0) + "A");
        _builder.From("MyTable");

        // Act
        var actual = _builder.OrderBy(query, _settingsMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY FieldA ASC");
    }

    [Fact]
    public void OrderBy_Throws_When_GetDatabaseFieldName_Returns_Null()
    {
        // Arrange
        _fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>()).Returns(default(string));
        var query = new FieldSelectionQueryBuilder().OrderBy("Field").BuildTyped();

        // Act & Assert
        _builder.Invoking(x => x.OrderBy(query, _settingsMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default))
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().StartWith("Expression contains unknown field [Field]");
    }

    [Fact]
    public void GroupBy_Does_Not_Append_Anything_When_GroupByFields_Is_Null()
    {
        // Arrange
        _settingsMock.TableName.Returns("MyTable");
        _builder.From("MyTable");

        // Act
        var actual = _builder.GroupBy(null, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void GroupBy_Does_Not_Append_Anything_When_GroupByFields_Is_Empty()
    {
        // Arrange
        _builder.From("MyTable");

        // Act
        var actual = _builder.GroupBy(_queryMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void GroupBy_Appends_Single_GroupBy_Clause()
    {
        // Arrange
        _queryMock.GroupByFields
                  .Returns(new ReadOnlyValueCollection<Expression>([new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field").Build()]));
        _builder.From("MyTable");

        // Act
        var actual = _builder.GroupBy(_queryMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable GROUP BY Field");
    }

    [Fact]
    public void GroupBy_Appends_Multiple_GroupBy_Clauses()
    {
        // Arrange
        _queryMock.GroupByFields
                  .Returns(new ReadOnlyValueCollection<Expression>([ new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field1").Build(),
                                                                           new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field2").Build() ]));
        _builder.From("MyTable");

        // Act
        var actual = _builder.GroupBy(_queryMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable GROUP BY Field1, Field2");
    }

    [Fact]
    public void GroupBy_Appends_GroupBy_Clause_With_GetDatabaseFieldName()
    {
        // Arrange
        _queryMock.GroupByFields
                  .Returns(new ReadOnlyValueCollection<Expression>([new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field").Build()]));
        _builder.From("MyTable");
        _fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>())
                      .Returns(x => x.ArgAt<string>(0) + "A");

        // Act
        var actual = _builder.GroupBy(_queryMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable GROUP BY FieldA");
    }

    [Fact]
    public void Having_Adds_Single_Condition()
    {
        // Arrange
        _queryMock.GroupByFilter
                  .Returns(new ComposedEvaluatable([ new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field"))
                                                                                       .WithOperator(new EqualsOperatorBuilder())
                                                                                       .WithRightExpression(new ConstantExpressionBuilder().WithValue("value"))
                                                                                       .BuildTyped() ]));
        _builder.From("MyTable");

        // Act
        var actual = _builder.Having(_queryMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable HAVING Field = @p0");
    }

    [Fact]
    public void Having_Adds_Multiple_Conditions()
    {
        // Arrange
        _queryMock.GroupByFilter
                  .Returns(new ComposedEvaluatable(
                  [
                      new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field1")).WithOperator(new EqualsOperatorBuilder()).WithRightExpression(new ConstantExpressionBuilder().WithValue("value1")).BuildTyped(),
                      new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field2")).WithOperator(new EqualsOperatorBuilder()).WithRightExpression(new ConstantExpressionBuilder().WithValue("value2")).BuildTyped()
                  ]));
        _builder.From("MyTable");

        // Act
        var actual = _builder.Having(_queryMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable HAVING Field1 = @p0 AND Field2 = @p1");
    }

    [Fact]
    public void Having_Does_Not_Append_Anything_When_GroupByFilter_Is_Null()
    {
        // Arrange
        _builder.From("MyTable");

        // Act
        var actual = _builder.Having(null, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void Having_Does_Not_Append_Anything_When_GroupByFilter_Is_Empty()
    {
        // Arrange
        _builder.From("MyTable");

        // Act
        var actual = _builder.Having(_queryMock, _fieldInfoMock, _evaluatorMock, _parameterBag, default);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void AppendQueryCondition_Adds_Combination_And_Increases_ParamCountner_When_ParamCounter_Is()
    {
        // Arrange
        _builder.From("MyTable");

        // Act
        _  = _builder.AppendQueryCondition(new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field"))
                                                                             .WithOperator(new IsGreaterOperatorBuilder())
                                                                             .WithRightExpression(new ConstantExpressionBuilder().WithValue("value"))
                                                                             .BuildTyped(),
                                           _fieldInfoMock,
                                           _evaluatorMock,
                                           _parameterBag,
                                           default,
                                           _builder.Where);

        // Assert
        _builder.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field > @p0");
    }

    [Fact]
    public void AppendQueryCondition_Gets_CustomFieldName_When_Possible()
    {
        // Arrange
        _fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>())
                      .Returns(x => x.ArgAt<string>(0) == "Field" ? "CustomField" : x.ArgAt<string>(0));
        _builder.From("MyTable");

        // Act
        _builder.AppendQueryCondition(new ComposableEvaluatableBuilder()
                                        .WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field"))
                                        .WithOperator(new IsGreaterOperatorBuilder())
                                        .WithRightExpression(new ConstantExpressionBuilder().WithValue("value"))
                                        .BuildTyped(),
                                      _fieldInfoMock,
                                      _evaluatorMock,
                                      _parameterBag,
                                      default,
                                      _builder.Where);

        // Assert
        _builder.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE CustomField > @p0");
    }

    [Fact]
    public void AppendQueryCondition_Throws_On_Invalid_CustomFieldName()
    {
        // Arrange
        _fieldInfoMock.GetDatabaseFieldName(Arg.Any<string>())
                      .Returns(x => x.ArgAt<string>(0) == "Field" ? null : x.ArgAt<string>(0));
        _builder.From("MyTable");

        // Act
        _builder.Invoking(x => x.AppendQueryCondition(new ComposableEvaluatableBuilder()
                                                        .WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field"))
                                                        .WithOperator(new IsGreaterOperatorBuilder())
                                                        .WithRightExpression(new ConstantExpressionBuilder().WithValue("value"))
                                                        .BuildTyped(),
                                                      _fieldInfoMock,
                                                      _evaluatorMock,
                                                      _parameterBag,
                                                      default,
                                                      _builder.Where))
                .Should().Throw<InvalidOperationException>()
                .And.Message.Should().StartWith("Expression contains unknown field [Field]");
    }

    [Theory]
    [InlineData(typeof(IsNullOrEmptyOperatorBuilder), "COALESCE(Field,'') = ''")]
    [InlineData(typeof(IsNotNullOrEmptyOperatorBuilder), "COALESCE(Field,'') <> ''")]
    [InlineData(typeof(IsNullOrWhiteSpaceOperatorBuilder), "COALESCE(TRIM(Field),'') = ''")]
    [InlineData(typeof(IsNotNullOrWhiteSpaceOperatorBuilder), "COALESCE(TRIM(Field),'') <> ''")]
    [InlineData(typeof(IsNullOperatorBuilder), "Field IS NULL")]
    [InlineData(typeof(IsNotNullOperatorBuilder), "Field IS NOT NULL")]
    public void AppendQueryCondition_Fills_CommandText_Correctly_For_QueryOperator_Without_Value(Type operatorBuilderType, string expectedCommandText)
    {
        // Arrange
        _builder.From("MyTable");

        // Act
        _builder.AppendQueryCondition(new ComposableEvaluatableBuilder()
                                        .WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field"))
                                        .WithOperator((OperatorBuilder)Activator.CreateInstance(operatorBuilderType)!)
                                        .WithRightExpression(new EmptyExpressionBuilder())
                                        .BuildTyped(),
                                      _fieldInfoMock,
                                      _evaluatorMock,
                                      _parameterBag,
                                      default,
                                      _builder.Where);

        // Assert
        _builder.Build().DataCommand.CommandText.Should().Be($"SELECT * FROM MyTable WHERE {expectedCommandText}");
    }

    [Theory]
    [InlineData(typeof(StringContainsOperatorBuilder), "CHARINDEX(@p0, Field) > 0")]
    [InlineData(typeof(StringNotContainsOperatorBuilder), "CHARINDEX(@p0, Field) = 0")]
    [InlineData(typeof(StartsWithOperatorBuilder), "LEFT(Field, 4) = @p0")]
    [InlineData(typeof(NotStartsWithOperatorBuilder), "LEFT(Field, 4) <> @p0")]
    [InlineData(typeof(EndsWithOperatorBuilder), "RIGHT(Field, 4) = @p0")]
    [InlineData(typeof(NotEndsWithOperatorBuilder), "RIGHT(Field, 4) <> @p0")]
    [InlineData(typeof(EqualsOperatorBuilder), "Field = @p0")]
    [InlineData(typeof(IsGreaterOrEqualOperatorBuilder), "Field >= @p0")]
    [InlineData(typeof(IsGreaterOperatorBuilder), "Field > @p0")]
    [InlineData(typeof(IsSmallerOrEqualOperatorBuilder), "Field <= @p0")]
    [InlineData(typeof(IsSmallerOperatorBuilder), "Field < @p0")]
    [InlineData(typeof(NotEqualsOperatorBuilder), "Field <> @p0")]
    public void AppendQueryCondition_Fills_CommandText_Correctly_For_QueryOperator_With_Value(Type operatorBuilderType, string expectedCommandText)
    {
        // Arrange
        _builder.From("MyTable");

        // Act
        _builder.AppendQueryCondition(new ComposableEvaluatableBuilder()
                                        .WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("Field"))
                                        .WithOperator((OperatorBuilder)Activator.CreateInstance(operatorBuilderType)!)
                                        .WithRightExpression(new ConstantExpressionBuilder().WithValue("test"))
                                        .BuildTyped(),
                                      _fieldInfoMock,
                                      _evaluatorMock,
                                      _parameterBag,
                                      default,
                                      _builder.Where);
        var actual = _builder.Build().DataCommand;

        // Assert
        actual.CommandText.Should().Be($"SELECT * FROM MyTable WHERE {expectedCommandText}");
    }

    [Fact]
    public void WithParameters_Adds_QueryParameters_When_Found()
    {
        // Arrange
        var query = new ParameterizedQueryBuilder()
            .AddParameter("name", "Value1")
            .BuildTyped();
        var parameterBag = new ParameterBag();
        parameterBag.CreateQueryParameterName("Value2");

        // Act
        var actual = _builder.WithParameters(query, parameterBag);

        // Assert
        actual.CommandParameters.Should().HaveCount(2);
        actual.CommandParameters.First().Key.Should().Be("name");
        actual.CommandParameters.First().Value.Should().Be("Value1");
        actual.CommandParameters.Last().Key.Should().Be("@p0");
        actual.CommandParameters.Last().Value.Should().Be("Value2");
    }
}

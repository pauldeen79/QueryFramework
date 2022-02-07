namespace QueryFramework.SqlServer.Tests.Extensions;

public class DatabaseCommandBuilderExtensionsTests
{
    private readonly PagedSelectCommandBuilder _builder;
    private readonly Mock<IPagedDatabaseEntityRetrieverSettings> _settingsMock;
    private readonly Mock<IQueryFieldInfo> _fieldInfoMock;
    private readonly Mock<IGroupingQuery> _queryMock;
    private readonly Mock<IQueryExpressionEvaluator> _evaluatorMock;

    public DatabaseCommandBuilderExtensionsTests()
    {
        _builder = new PagedSelectCommandBuilder();
        _settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
        _settingsMock.SetupGet(x => x.Fields).Returns(string.Empty);
        _fieldInfoMock = new Mock<IQueryFieldInfo>();
        _fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
        _queryMock = new Mock<IGroupingQuery>();
        _queryMock.SetupGet(x => x.GroupByFields).Returns(new ValueCollection<IQueryExpression>());
        _queryMock.SetupGet(x => x.HavingFields).Returns(new ValueCollection<IQueryCondition>());
        _evaluatorMock = new Mock<IQueryExpressionEvaluator>();
        // Use real query expression evaluator
        _evaluatorMock.Setup(x => x.GetSqlExpression(It.IsAny<IQueryExpression>()))
                     .Returns<IQueryExpression>(x => new DefaultQueryExpressionEvaluator(Enumerable.Empty<IFunctionParser>()).GetSqlExpression(x));
    }

    [Fact]
    public void Select_Uses_Star_When_Fields_Is_Empty_And_GetAllFields_Returns_Null_And_SelectAll_Is_True()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().SelectAll().Build();
        _fieldInfoMock.Setup(x => x.GetAllFields())
                         .Returns(Enumerable.Empty<string>());
        _builder.From("MyTable");

        // Act
        var actual = _builder.Select(_settingsMock.Object, _fieldInfoMock.Object, query, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void Select_Uses_GetAllFields_When_Provided()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().SelectAll().Build();
        _fieldInfoMock.Setup(x => x.GetAllFields())
                         .Returns(new[] { "Field1", "Field2", "Field3" });
        _builder.From("MyTable");

        // Act
        var actual = _builder.Select(_settingsMock.Object, _fieldInfoMock.Object, query, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT Field1, Field2, Field3 FROM MyTable");
    }

    [Fact]
    public void Select_Uses_Fields_From_Query_When_GetAllFields_Is_False()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
        _builder.From("MyTable");

        // Act
        var actual = _builder.Select(_settingsMock.Object, _fieldInfoMock.Object, query, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT Field1, Field2, Field3 FROM MyTable");
    }

    [Fact]
    public void Select_Uses_GetFieldDelegate_When_Result_Is_Not_Null()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
        _fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                         .Returns<string>(x => x + "A");
        _builder.From("MyTable");

        // Act
        var actual = _builder.Select(_settingsMock.Object, _fieldInfoMock.Object, query, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT Field1A, Field2A, Field3A FROM MyTable");
    }

    [Fact]
    public void Select_Throws_When_GetDatabaseFieldName_Returns_Null()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
        _fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                         .Returns(default(string));

        // Act
        _builder.Invoking(x => x.Select(_settingsMock.Object, _fieldInfoMock.Object, query, _evaluatorMock.Object))
               .Should().Throw<InvalidOperationException>()
               .And.Message.Should().StartWith("Query fields contains unknown field in expression [QueryExpression { FieldName = Field1, Function =  }]");
    }

    [Fact]
    public void Where_Does_Not_Append_Anything_When_Conditions_And_DefaultWhere_Are_Both_Empty()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Build();
        _builder.From("MyTable");

        // Act
        var actual = _builder.Where(query, _settingsMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object, out _);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void Where_Adds_Single_Condition_Without_Default_Where_Clause()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).Build();
        _builder.From("MyTable");

        // Act
        var actual = _builder.Where(query, _settingsMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object, out _);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field = @p0");
    }

    [Fact]
    public void Where_Adds_Single_Condition_With_Default_Where_Clause()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).Build();
        _settingsMock.SetupGet(x => x.DefaultWhere).Returns("Field IS NOT NULL");
        _builder.From("MyTable");

        // Act
        var actual = _builder.Where(query, _settingsMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object, out _);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field IS NOT NULL AND Field = @p0");
    }

    [Fact]
    public void Where_Adds_Multiple_Conditions_Without_Default_Where_Clause()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value"))
                                                    .And("Field2".IsNotNull())
                                                    .Build();
        _builder.From("MyTable");

        // Act
        var actual = _builder.Where(query, _settingsMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object, out _);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field = @p0 AND Field2 IS NOT NULL");
    }

    [Fact]
    public void Where_Adds_Multiple_Conditions_With_Default_Where_Clause()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).And("Field2".IsNotNull()).Build();
        _settingsMock.SetupGet(x => x.DefaultWhere).Returns("Field IS NOT NULL");
        _builder.From("MyTable");

        // Act
        var actual = _builder.Where(query, _settingsMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object, out _);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field IS NOT NULL AND Field = @p0 AND Field2 IS NOT NULL");
    }

    [Fact]
    public void OrderBy_Does_Not_Append_Anything_When_Limit_And_Offset_Are_Both_Filled()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().OrderBy("Field").Limit(10).Offset(20).Build();
        _settingsMock.SetupGet(x => x.DefaultOrderBy).Returns("Ignored");
        _builder.From("MyTable");

        // Act
        var actual = _builder.OrderBy(query, _settingsMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void OrderBy_Does_Not_Append_Anything_When_Query_OrderByFields_Is_Empty()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Build();
        _builder.From("MyTable");

        // Act
        var actual = _builder.OrderBy(query, _settingsMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void OrderBy_Appends_Single_OrderBy_Clause()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
        _builder.From("MyTable");

        // Act
        var actual = _builder.OrderBy(query, _settingsMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY Field ASC");
    }

    [Fact]
    public void OrderBy_Appends_Multiple_OrderBy_Clauses()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().OrderBy("Field1").ThenByDescending("Field2").Build();
        _builder.From("MyTable");

        // Act
        var actual = _builder.OrderBy(query, _settingsMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY Field1 ASC, Field2 DESC");
    }

    [Fact]
    public void OrderBy_Appends_Default_OrderBy_Clause()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().Build();
        _settingsMock.SetupGet(x => x.DefaultOrderBy).Returns("Field ASC");
        _builder.From("MyTable");

        // Act
        var actual = _builder.OrderBy(query, _settingsMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY Field ASC");
    }

    [Fact]
    public void OrderBy_Does_Not_Append_DefaultOrderBy_When_OrderBy_Clause_Is_Present_On_Query()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
        _settingsMock.SetupGet(x => x.DefaultOrderBy).Returns("Ignored");
        _builder.From("MyTable");

        // Act
        var actual = _builder.OrderBy(query, _settingsMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY Field ASC");
    }

    [Fact]
    public void OrderBy_Appends_Single_OrderBy_Clause_With_GetDatabaseFieldName()
    {
        // Arrange
        var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
        _fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                         .Returns<string>(x => x + "A");
        _builder.From("MyTable");

        // Act
        var actual = _builder.OrderBy(query, _settingsMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY FieldA ASC");
    }

    [Fact]
    public void OrderBy_Throws_When_GetDatabaseFieldName_Returns_Null()
    {
        // Arrange
        _fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns(default(string));
        var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();

        // Act & Assert
        _builder.Invoking(x => x.OrderBy(query, _settingsMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object))
               .Should().Throw<InvalidOperationException>()
               .And.Message.Should().StartWith("Query order by fields contains unknown field [Field]");
    }

    [Fact]
    public void GroupBy_Does_Not_Append_Anything_When_GroupByFields_Is_Null()
    {
        // Arrange
        _settingsMock.SetupGet(x => x.TableName).Returns("MyTable");
        _builder.From("MyTable");

        // Act
        var actual = _builder.GroupBy(null, _fieldInfoMock.Object, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void GroupBy_Does_Not_Append_Anything_When_GroupByFields_Is_Empty()
    {
        // Arrange
        _builder.From("MyTable");

        // Act
        var actual = _builder.GroupBy(_queryMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void GroupBy_Appends_Single_GroupBy_Clause()
    {
        // Arrange
        _queryMock.SetupGet(x => x.GroupByFields)
                 .Returns(new ValueCollection<IQueryExpression>(new[] { new QueryExpression("Field", null) }));
        _builder.From("MyTable");

        // Act
        var actual = _builder.GroupBy(_queryMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable GROUP BY Field");
    }

    [Fact]
    public void GroupBy_Appends_Multiple_GroupBy_Clauses()
    {
        // Arrange
        _queryMock.SetupGet(x => x.GroupByFields)
                 .Returns(new ValueCollection<IQueryExpression>(new[] { new QueryExpression("Field1", null),
                                                                            new QueryExpression("Field2", null) }));
        _builder.From("MyTable");

        // Act
        var actual = _builder.GroupBy(_queryMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable GROUP BY Field1, Field2");
    }

    [Fact]
    public void GroupBy_Appends_GroupBy_Clause_With_GetDatabaseFieldName()
    {
        // Arrange
        _queryMock.SetupGet(x => x.GroupByFields)
                 .Returns(new ValueCollection<IQueryExpression>(new[] { new QueryExpression("Field", null) }));
        _builder.From("MyTable");
        _fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                         .Returns<string>(x => x + "A");

        // Act
        var actual = _builder.GroupBy(_queryMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable GROUP BY FieldA");
    }

    [Fact]
    public void GroupBy_Throws_When_GetDatabaseFieldName_Returns_Null()
    {
        // Arrange
        _queryMock.SetupGet(x => x.GroupByFields)
                 .Returns(new ValueCollection<IQueryExpression>(new[] { new QueryExpression("Field", null) }));
        _settingsMock.SetupGet(x => x.TableName).Returns("MyTable");
        _fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                         .Returns(default(string));

        // Act & Assert
        _builder.Invoking(x => x.GroupBy(_queryMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object))
               .Should().Throw<InvalidOperationException>()
               .And.Message.Should().StartWith("Query group by fields contains unknown field [Field]");
    }

    [Fact]
    public void Having_Adds_Single_Condition()
    {
        // Arrange
        _queryMock.SetupGet(x => x.HavingFields)
                 .Returns(new ValueCollection<IQueryCondition>(new[] { new QueryCondition(false, false, new QueryExpression("Field", null), QueryOperator.Equal, "value", QueryCombination.And) }));
        int paramCounter = 0;
        _builder.From("MyTable");

        // Act
        var actual = _builder.Having(_queryMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object, ref paramCounter);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable HAVING Field = @p0");
    }

    [Fact]
    public void Having_Adds_Multiple_Conditions()
    {
        // Arrange
        _queryMock.SetupGet(x => x.HavingFields)
                 .Returns(new ValueCollection<IQueryCondition>(new[]
                 {
                     new QueryCondition(false, false, new QueryExpression("Field1", null), QueryOperator.Equal, "value1", QueryCombination.And),
                     new QueryCondition(false, false, new QueryExpression("Field2", null), QueryOperator.Equal, "value2", QueryCombination.And)
                 }));
        int paramCounter = 0;
        _builder.From("MyTable");

        // Act
        var actual = _builder.Having(_queryMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object, ref paramCounter);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable HAVING Field1 = @p0 AND Field2 = @p1");
    }

    [Fact]
    public void Having_Does_Not_Append_Anything_When_HavingFields_Is_Null()
    {
        // Arrange
        int paramCounter = 0;
        _builder.From("MyTable");

        // Act
        var actual = _builder.Having(null, _fieldInfoMock.Object, _evaluatorMock.Object, ref paramCounter);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Fact]
    public void Having_Does_Not_Append_Anything_When_HavingFields_Is_Empty()
    {
        // Arrange
        int paramCounter = 0;
        _builder.From("MyTable");

        // Act
        var actual = _builder.Having(_queryMock.Object, _fieldInfoMock.Object, _evaluatorMock.Object, ref paramCounter);

        // Assert
        actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(99, true)]
    public void AppendQueryCondition_Adds_Combination_Conditionally_But_Always_Increases_ParamCountner_When_ParamCounter_Is(int paramCounter, bool shouldAddCombination)
    {
        // Arrange
        _builder.From("MyTable");
        if (shouldAddCombination)
        {
            _builder.Where("Field IS NOT NULL");
        }

        // Act
        var actual = _builder.AppendQueryCondition(paramCounter,
                                                  new QueryCondition(false, false, new QueryExpression("Field", null), QueryOperator.Greater, "value", QueryCombination.And),
                                                  _fieldInfoMock.Object,
                                                  _evaluatorMock.Object,
                                                  _builder.Where);

        // Assert
        if (shouldAddCombination)
        {
            _builder.Build().DataCommand.CommandText.Should().Be($"SELECT * FROM MyTable WHERE Field IS NOT NULL AND Field > @p{paramCounter}");
        }
        else
        {
            _builder.Build().DataCommand.CommandText.Should().Be($"SELECT * FROM MyTable WHERE Field > @p{paramCounter}");
        }
        actual.Should().Be(paramCounter + 1);
    }

    [Fact]
    public void AppendQueryCondition_Adds_Brackets_When_Necessary()
    {
        // Arrange
        _builder.From("MyTable");

        // Act
        _builder.AppendQueryCondition(0,
                                     new QueryCondition(true, true, new QueryExpression("Field", null), QueryOperator.Greater, "value", QueryCombination.And),
                                     _fieldInfoMock.Object,
                                     _evaluatorMock.Object,
                                     _builder.Where);

        // Assert
        _builder.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE (Field > @p0)");
    }

    [Fact]
    public void AppendQueryCondition_Gets_CustomFieldName_When_Possible()
    {
        // Arrange
        _fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                         .Returns<string>(x => x == "Field" ? "CustomField" : x);
        _builder.From("MyTable");

        // Act
        _builder.AppendQueryCondition(0,
                                     new QueryCondition(false, false, new QueryExpression("Field", null), QueryOperator.Greater, "value", QueryCombination.And),
                                     _fieldInfoMock.Object,
                                     _evaluatorMock.Object,
                                     _builder.Where);

        // Assert
        _builder.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE CustomField > @p0");
    }

    [Fact]
    public void AppendQueryCondition_Throws_On_Invalid_CustomFieldName()
    {
        // Arrange
        _fieldInfoMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                         .Returns<string>(x => x == "Field" ? null : x);
        _builder.From("MyTable");

        // Act
        _builder.Invoking(x => x.AppendQueryCondition(0,
                                                     new QueryCondition(false, false, new QueryExpression("Field", null), QueryOperator.Greater, "value", QueryCombination.And),
                                                     _fieldInfoMock.Object,
                                                     _evaluatorMock.Object,
                                                     _builder.Where))
               .Should().Throw<InvalidOperationException>()
               .And.Message.Should().StartWith("Query conditions contains unknown field [Field]");
    }

    [Theory]
    [InlineData(QueryOperator.IsNullOrEmpty, "COALESCE(Field,'') = ''")]
    [InlineData(QueryOperator.IsNotNullOrEmpty, "COALESCE(Field,'') <> ''")]
    [InlineData(QueryOperator.IsNull, "Field IS NULL")]
    [InlineData(QueryOperator.IsNotNull, "Field IS NOT NULL")]
    public void AppendQueryCondition_Fills_CommandText_Correctly_For_QueryOperator_Without_Value(QueryOperator queryOperator, string expectedCommandText)
    {
        // Arrange
        _builder.From("MyTable");

        // Act
        _builder.AppendQueryCondition(0,
                                     new QueryCondition(false, false, new QueryExpression("Field", null), queryOperator, null, QueryCombination.And),
                                     _fieldInfoMock.Object,
                                     _evaluatorMock.Object,
                                     _builder.Where);

        // Assert
        _builder.Build().DataCommand.CommandText.Should().Be($"SELECT * FROM MyTable WHERE {expectedCommandText}");
    }

    [Theory]
    [InlineData(QueryOperator.Contains, "CHARINDEX(@p0, Field) > 0")]
    [InlineData(QueryOperator.NotContains, "CHARINDEX(@p0, Field) = 0")]
    [InlineData(QueryOperator.StartsWith, "LEFT(Field, 4) = @p0")]
    [InlineData(QueryOperator.NotStartsWith, "LEFT(Field, 4) <> @p0")]
    [InlineData(QueryOperator.EndsWith, "RIGHT(Field, 4) = @p0")]
    [InlineData(QueryOperator.NotEndsWith, "RIGHT(Field, 4) <> @p0")]
    [InlineData(QueryOperator.Equal, "Field = @p0")]
    [InlineData(QueryOperator.GreaterOrEqual, "Field >= @p0")]
    [InlineData(QueryOperator.Greater, "Field > @p0")]
    [InlineData(QueryOperator.LowerOrEqual, "Field <= @p0")]
    [InlineData(QueryOperator.Lower, "Field < @p0")]
    [InlineData(QueryOperator.NotEqual, "Field <> @p0")]
    public void AppendQueryCondition_Fills_CommandText_And_Parameters_Correctly_For_QueryOperator_With_Value(QueryOperator queryOperator, string expectedCommandText)
    {
        // Arrange
        _builder.From("MyTable");

        // Act
        _builder.AppendQueryCondition(0,
                                     new QueryCondition(false, false, new QueryExpression("Field", null), queryOperator, "test", QueryCombination.And),
                                     _fieldInfoMock.Object,
                                     _evaluatorMock.Object,
                                     _builder.Where);
        var actual = _builder.Build().DataCommand;

        // Assert
        actual.CommandText.Should().Be($"SELECT * FROM MyTable WHERE {expectedCommandText}");
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        parameters.Should().HaveCount(1);
        if (parameters?.Count == 1)
        {
            parameters.First().Key.Should().Be("p0");
            parameters.First().Value.Should().Be("test");
        }
    }

    [Fact]
    public void WithParameters_Adds_QueryParameters_When_Found()
    {
        // Arrange
        var query = new ParameterizedQueryMock(new[] { new QueryParameter("name", "Value") });

        // Act
        var actual = _builder.WithParameters(query);

        // Assert
        actual.CommandParameters.Should().HaveCount(1);
        actual.CommandParameters.First().Key.Should().Be("name");
        actual.CommandParameters.First().Value.Should().Be("Value");
    }
}

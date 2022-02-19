namespace QueryFramework.InMemory.Tests;

public sealed class QueryProcessorTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly DataProviderMock _dataProviderMock = new DataProviderMock();

    public QueryProcessorTests()
        => _serviceProvider = new ServiceCollection()
            .AddExpressionFramework()
            .AddQueryFrameworkInMemory()
            .AddSingleton<IDataProvider>(_dataProviderMock)
            .BuildServiceProvider();

    [Fact]
    public void Unsupported_Query_Operator_Throws_On_FindPAged()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(new ConditionBuilder
            {
                LeftExpression = new FieldExpressionBuilder().WithFieldName(nameof(MyClass.Property)),
                Operator = (Operator)99
            }).Build();

        // Act & Assert
        sut.Invoking(x => x.FindPaged<MyClass>(query))
           .Should().Throw<ArgumentOutOfRangeException>()
           .And.Message.Should().StartWith("Unsupported operator: 99");
    }

    [Fact]
    public void Unknown_FieldName_Throws_On_FindPaged()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where("UnknownField".IsEqualTo("something"))
            .Build();

        // Act & Assert
        sut.Invoking(x => x.FindPaged<MyClass>(query))
           .Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName("fieldName")
           .And.Message.Should().StartWith("Fieldname [UnknownField] is not found on type [QueryFramework.InMemory.Tests.QueryProcessorTests+MyClass]");
    }

    [Fact]
    public void Unsupported_Function_Throws_On_FindPaged()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var functionMock = new Mock<IExpressionFunction>();
        var functionBuilderMock = new Mock<IExpressionFunctionBuilder>();
        functionBuilderMock.Setup(x => x.Build()).Returns(functionMock.Object);
        functionMock.Setup(x => x.ToBuilder()).Returns(functionBuilderMock.Object);

        var query = new SingleEntityQueryBuilder()
            .Where(new ConditionBuilder()
                .WithLeftExpression(new FieldExpressionBuilder()
                    .WithFieldName(nameof(MyClass.Property))
                    .WithFunction(functionBuilderMock.Object))
                .WithOperator(Operator.Equal)
                .WithRightExpression(new ConstantExpressionBuilder().WithValue("something")))
            .Build();

        // Act & Assert
        sut.Invoking(x => x.FindPaged<MyClass>(query))
           .Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName("expression")
           .And.Message.Should().StartWith("Unsupported function: [IExpressionFunctionProxy]");
    }

    [Fact]
    public void Can_FindOne_On_InMemoryList_With_Zero_Conditions()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQuery();

        // Act
        var actual = sut.FindOne<MyClass>(query);

        // Assert
        actual.Should().NotBeNull();
        actual?.Property.Should().Be("A");
    }

    [Fact]
    public void Can_FindMany_On_InMemoryList_With_Zero_Conditions()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQuery();

        // Act
        var actual = sut.FindMany<MyClass>(query);

        // Assert
        actual.Should().HaveCount(2);
        actual.First().Property.Should().Be("A");
        actual.Last().Property.Should().Be("B");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_Zero_Conditions()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQuery();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(2);
        actual.First().Property.Should().Be("A");
        actual.Last().Property.Should().Be("B");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_Equals_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).IsEqualTo("B"))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("B");
    }

    //[Fact]
    //public void Can_FindPaged_On_InMemoryList_With_Two_Equals_Conditions_Using_Or_Combination()
    //{
    //    // Arrange
    //    var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
    //    var sut = CreateSut(items);
    //    var query = new SingleEntityQueryBuilder()
    //        .Where(nameof(MyClass.Property).IsEqualTo("B"))
    //        .Or(nameof(MyClass.Property).IsEqualTo("A"))
    //        .Build();

    //    // Act
    //    var actual = sut.FindPaged<MyClass>(query);

    //    // Assert
    //    actual.Should().HaveCount(2);
    //    actual.First().Property.Should().Be("A");
    //    actual.Last().Property.Should().Be("B");
    //}

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_Two_Equals_Conditions_Using_And_Combination()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).IsEqualTo("B"))
            .And(nameof(MyClass.Property).IsEqualTo("A"))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(0);
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_NotEquals_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).IsNotEqualTo("B"))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("A");
    }

    //[Fact]
    //public void Can_FindPaged_On_InMemoryList_With_One_NotEquals_Condition_And_Brackets()
    //{
    //    // Arrange
    //    var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
    //    var sut = CreateSut(items);
    //    var query = new SingleEntityQueryBuilder()
    //        .OrAll(nameof(MyClass.Property).IsNotEqualTo("B"))
    //        .Build();

    //    // Act
    //    var actual = sut.FindPaged<MyClass>(query);

    //    // Assert
    //    actual.Should().HaveCount(1);
    //    actual.First().Property.Should().Be("A");
    //}

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_Contains_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).DoesContain("zz"))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Pizza");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_NotContains_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).DoesNotContain("zz"))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Beer");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_EndsWith_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).DoesEndWith("er"))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Beer");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_NotEndsWith_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).DoesNotEndWith("er"))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Pizza");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_StartsWith_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).DoesStartWith("Be"))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Beer");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_NotStartsWith_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).DoesNotStartWith("Be"))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Pizza");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_IsNull_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).IsNull())
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(0);
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_IsNotNull_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).IsNotNull())
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(2);
        actual.First().Property.Should().Be("Pizza");
        actual.Last().Property.Should().Be("Beer");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_IsNullOrEmpty_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).IsNullOrEmpty())
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(0);
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_IsNotNullOrEmpty_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).IsNotNullOrEmpty())
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(2);
        actual.First().Property.Should().Be("Pizza");
        actual.Last().Property.Should().Be("Beer");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_SmallerThan_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).IsSmallerThan("Coconut"))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Beer");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_SmallerOrEqualThan_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).IsSmallerOrEqualThan("Beer"))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Beer");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_GreaterThan_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).IsGreaterThan("Coconut"))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Pizza");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_GreaterOrEqualThan_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).IsGreaterOrEqualThan("Pizza"))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("Pizza");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_Equals_Condition_Case_Insensitive()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property).IsEqualTo("b"))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("B");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_Skip_And_Take()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" }, new MyClass { Property = "C" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Skip(1)
            .Take(1)
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("B");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_OrderByAscending()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" }, new MyClass { Property = "C" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .OrderBy(nameof(MyClass.Property))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(3);
        actual.ElementAt(0).Property.Should().Be("A");
        actual.ElementAt(1).Property.Should().Be("B");
        actual.ElementAt(2).Property.Should().Be("C");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_OrderByDescending()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" }, new MyClass { Property = "C" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .OrderByDescending(nameof(MyClass.Property))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(3);
        actual.ElementAt(0).Property.Should().Be("C");
        actual.ElementAt(1).Property.Should().Be("B");
        actual.ElementAt(2).Property.Should().Be("A");
    }

    [Fact]
    public void Can_FindOne_On_InMemoryList_With_OrderByDescending()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" }, new MyClass { Property = "C" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .OrderByDescending(nameof(MyClass.Property))
            .Build();

        // Act
        var actual = sut.FindOne<MyClass>(query);

        // Assert
        actual.Should().NotBeNull();
        actual?.Property.Should().Be("C");
    }

    [Fact]
    public void Can_FindMany_On_InMemoryList_With_OrderByDescending()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" }, new MyClass { Property = "C" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .OrderByDescending(nameof(MyClass.Property))
            .Build();

        // Act
        var actual = sut.FindMany<MyClass>(query);

        // Assert
        actual.Should().HaveCount(3);
        actual.ElementAt(0).Property.Should().Be("C");
        actual.ElementAt(1).Property.Should().Be("B");
        actual.ElementAt(2).Property.Should().Be("A");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_Multiple_OrderBy_Clauses()
    {
        // Arrange
        var items = new[]
        {
            new MyClass { Property = "A", Property2 = "Z" },
            new MyClass { Property = "B", Property2 = "X" },
            new MyClass { Property = "C", Property2 = "Z" }
        };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .OrderBy(nameof(MyClass.Property2))
            .ThenByDescending(nameof(MyClass.Property))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(3);
        actual.ElementAt(0).Property.Should().Be("B");
        actual.ElementAt(1).Property.Should().Be("C");
        actual.ElementAt(2).Property.Should().Be("A");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_Len_Expression()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A2" }, new MyClass { Property = "B23" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(new ConditionBuilder()
                .WithLeftExpression(new FieldExpressionBuilder().WithFieldName(nameof(MyClass.Property))
                                                                .WithFunction(new LengthFunctionBuilder()))
                .WithOperator(Operator.Equal)
                .WithRightExpression(new ConstantExpressionBuilder().WithValue(2)))
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("A2");
    }

    [Fact]
    public void FindOne_On_InMemoryList_Without_DataProvider_Throws()
    {
        // Arrange
        var query = new SingleEntityQuery();
        _dataProviderMock.ResultDelegate = new Func<ISingleEntityQuery, IEnumerable?>(_ => default(IEnumerable<MyClass>));
        _dataProviderMock.ReturnValue = false;
        var sut = _serviceProvider.GetRequiredService<IQueryProcessor>();

        // Act & Assert
        sut.Invoking(x => x.FindOne<MyClass>(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Query type [QueryFramework.Core.Queries.SingleEntityQuery] for data type [QueryFramework.InMemory.Tests.QueryProcessorTests+MyClass] does not have a data provider");
    }

    [Fact]
    public void FindMany_On_InMemoryList_Without_DataProvider_Throws()
    {
        // Arrange
        var query = new SingleEntityQuery();
        _dataProviderMock.ResultDelegate = new Func<ISingleEntityQuery, IEnumerable?>(_ => default(IEnumerable<MyClass>));
        _dataProviderMock.ReturnValue = false;
        var sut = _serviceProvider.GetRequiredService<IQueryProcessor>();

        // Act & Assert
        sut.Invoking(x => x.FindMany<MyClass>(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Query type [QueryFramework.Core.Queries.SingleEntityQuery] for data type [QueryFramework.InMemory.Tests.QueryProcessorTests+MyClass] does not have a data provider");
    }

    [Fact]
    public void FindPaged_On_InMemoryList_Without_DataProvider_Throws()
    {
        // Arrange
        var query = new SingleEntityQuery();
        _dataProviderMock.ResultDelegate = new Func<ISingleEntityQuery, IEnumerable?>(_ => default(IEnumerable<MyClass>));
        _dataProviderMock.ReturnValue = false;
        var sut = _serviceProvider.GetRequiredService<IQueryProcessor>();

        // Act & Assert
        sut.Invoking(x => x.FindPaged<MyClass>(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Query type [QueryFramework.Core.Queries.SingleEntityQuery] for data type [QueryFramework.InMemory.Tests.QueryProcessorTests+MyClass] does not have a data provider");
    }

    [Fact]
    public void FindOne_On_InMemoryList_With_DataProvider_That_Returns_Null_Throws()
    {
        // Arrange
        var query = new SingleEntityQuery();
        _dataProviderMock.ResultDelegate = new Func<ISingleEntityQuery, IEnumerable?>(_ => default(IEnumerable<MyClass>));
        _dataProviderMock.ReturnValue = true;
        var sut = _serviceProvider.GetRequiredService<IQueryProcessor>();

        // Act & Assert
        sut.Invoking(x => x.FindOne<MyClass>(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Data provider of type [QueryFramework.InMemory.Tests.TestHelpers.DataProviderMock] for data type [QueryFramework.InMemory.Tests.QueryProcessorTests+MyClass] provided an empty result");
    }

    private IQueryProcessor CreateSut(MyClass[] items)
    {
        var expressionEvaluator = _serviceProvider.GetRequiredService<IExpressionEvaluator>();
        _dataProviderMock.ResultDelegate = new Func<ISingleEntityQuery, IEnumerable?>
        (
            query => items.Where
            (
                item => query.Conditions
                    .Select(x => new DelegateExpressionBuilder()
                        .WithValueDelegate((item, expression, evaluator) => item)
                        .WithFunction(new ConditionFunctionBuilder().WithCondition(new ConditionBuilder(x)))
                        .Build())
                    .All(y => Convert.ToBoolean(expressionEvaluator.Evaluate(item, y)))
            )
        );
        _dataProviderMock.ReturnValue = true;
        return _serviceProvider.GetRequiredService<IQueryProcessor>();
    }

    public void Dispose() => _serviceProvider.Dispose();

    public class MyClass
    {
        public string? Property { get; set; }
        public string? Property2 { get; set; }
    }
}

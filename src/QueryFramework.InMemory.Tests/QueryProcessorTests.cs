namespace QueryFramework.InMemory.Tests;

public sealed class QueryProcessorTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly DataProviderMock _dataProviderMock = new();
    private readonly ContextDataProviderMock _contextDataProviderMock = new();

    public QueryProcessorTests()
        => _serviceProvider = new ServiceCollection()
            .AddQueryFrameworkInMemory()
            .AddSingleton<IDataProvider>(_dataProviderMock)
            .AddSingleton<IContextDataProvider>(_contextDataProviderMock)
            .BuildServiceProvider();

    [Fact]
    public void Unknown_FieldName_Throws_On_FindPaged()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where("UnknownField").IsEqualTo("something")
            .Build();

        // Act & Assert
        sut.Invoking(x => x.FindPaged<MyClass>(query))
           .Should().Throw<InvalidOperationException>()
           .WithMessage("Evaluation failed");
    }

    [Fact]
    public void Unknown_FieldName_Throws_On_FindPagedAsync()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where("UnknownField").IsEqualTo("something")
            .Build();

        // Act & Assert
        sut.Awaiting(x => x.FindPagedAsync<MyClass>(query))
           .Should().ThrowAsync<InvalidOperationException>()
           .WithMessage("Evaluation failed");
    }

    [Fact]
    public void Can_FindOne_On_InMemoryList_With_Zero_Conditions()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder().Build();

        // Act
        var actual = sut.FindOne<MyClass>(query);

        // Assert
        actual.Should().NotBeNull();
        actual?.Property.Should().Be("A");
    }

    [Fact]
    public async Task Can_FindOne_On_InMemoryList_With_Zero_Conditions_Async()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder().Build();

        // Act
        var actual = await sut.FindOneAsync<MyClass>(query);

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
        var query = new SingleEntityQueryBuilder().Build();

        // Act
        var actual = sut.FindMany<MyClass>(query);

        // Assert
        actual.Should().HaveCount(2);
        actual.First().Property.Should().Be("A");
        actual.Last().Property.Should().Be("B");
    }

    [Fact]
    public async Task Can_FindMany_On_InMemoryList_With_Zero_Conditions_Async()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder().Build();

        // Act
        var actual = await sut.FindManyAsync<MyClass>(query);

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
        var query = new SingleEntityQueryBuilder().Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(2);
        actual.First().Property.Should().Be("A");
        actual.Last().Property.Should().Be("B");
    }

    [Fact]
    public async Task Can_FindPaged_On_InMemoryList_With_Zero_Conditions_Async()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder().Build();

        // Act
        var actual = await sut.FindPagedAsync<MyClass>(query);

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
            .Where(nameof(MyClass.Property)).IsEqualTo("B")
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("B");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_Equals_Condition_Using_Context()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateContextSut(items);
        var builder = new SingleEntityQueryBuilder()
            .Where($"{nameof(SurrogateContext.Item)}.{nameof(MyClass.Property)}").IsEqualTo("##IGNORE##");
        builder.Filter.Conditions
            .Single()
            .WithRightExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(nameof(SurrogateContext.Context)));
        var query = builder.Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query, "B");

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("B");
    }

    [Fact]
    public async Task Can_FindPaged_On_InMemoryList_With_One_Equals_Condition_Using_Context_Async()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateContextSut(items);
        var builder = new SingleEntityQueryBuilder()
            .Where($"{nameof(SurrogateContext.Item)}.{nameof(MyClass.Property)}").IsEqualTo("##IGNORE##");
        builder.Filter.Conditions
            .Single()
            .WithRightExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(nameof(SurrogateContext.Context)));
        var query = builder.Build();

        // Act
        var actual = await sut.FindPagedAsync<MyClass>(query, "B");

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("B");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_Two_Equals_Conditions_Using_And_Combination()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property)).IsEqualTo("B")
            .And(nameof(MyClass.Property)).IsEqualTo("A")
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
            .Where(nameof(MyClass.Property)).IsNotEqualTo("B")
            .Build();

        // Act
        var actual = sut.FindPaged<MyClass>(query);

        // Assert
        actual.Should().HaveCount(1);
        actual.First().Property.Should().Be("A");
    }

    [Fact]
    public void Can_FindPaged_On_InMemoryList_With_One_Contains_Condition()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where(nameof(MyClass.Property)).Contains("zz")
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
            .Where(nameof(MyClass.Property)).DoesNotContain("zz")
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
            .Where(nameof(MyClass.Property)).EndsWith("er")
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
            .Where(nameof(MyClass.Property)).DoesNotEndWith("er")
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
            .Where(nameof(MyClass.Property)).StartsWith("Be")
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
            .Where(nameof(MyClass.Property)).DoesNotStartWith("Be")
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
            .Where(nameof(MyClass.Property)).IsNull()
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
            .Where(nameof(MyClass.Property)).IsNotNull()
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
            .Where(nameof(MyClass.Property)).IsNullOrEmpty()
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
            .Where(nameof(MyClass.Property)).IsNotNullOrEmpty()
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
            .Where(nameof(MyClass.Property)).IsSmallerThan("Coconut")
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
            .Where(nameof(MyClass.Property)).IsSmallerOrEqualThan("Beer")
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
            .Where(nameof(MyClass.Property)).IsGreaterThan("Coconut")
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
            .Where(nameof(MyClass.Property)).IsGreaterOrEqualThan("Pizza")
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
            .Where(nameof(MyClass.Property)).IsEqualTo("b")
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
    public void Can_FindOne_On_InMemoryList_With_OrderByDescending_Using_String_Extension()
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
    public void Can_FindOne_On_InMemoryList_With_OrderByDescending_Using_QuerySortOrderBuilder_Extension()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" }, new MyClass { Property = "C" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .OrderByDescending(new QuerySortOrderBuilder().WithFieldName(nameof(MyClass.Property)))
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
            .Where(new ComposableEvaluatableBuilder()
                .WithLeftExpression(new StringLengthExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName(nameof(MyClass.Property))))
                .WithOperator(new EqualsOperatorBuilder())
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
        var query = new SingleEntityQueryBuilder().Build();
        _dataProviderMock.ResultDelegate = new Func<IQuery, IEnumerable?>(_ => default(IEnumerable<MyClass>));
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
        var query = new SingleEntityQueryBuilder().Build();
        _dataProviderMock.ResultDelegate = new Func<IQuery, IEnumerable?>(_ => default(IEnumerable<MyClass>));
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
        var query = new SingleEntityQueryBuilder().Build();
        _dataProviderMock.ResultDelegate = new Func<IQuery, IEnumerable?>(_ => default(IEnumerable<MyClass>));
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
        var query = new SingleEntityQueryBuilder().Build();
        _dataProviderMock.ResultDelegate = new Func<IQuery, IEnumerable?>(_ => default(IEnumerable<MyClass>));
        _dataProviderMock.ReturnValue = true;
        var sut = _serviceProvider.GetRequiredService<IQueryProcessor>();

        // Act & Assert
        sut.Invoking(x => x.FindOne<MyClass>(query))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Data provider of type [QueryFramework.InMemory.Tests.TestHelpers.DataProviderMock] for data type [QueryFramework.InMemory.Tests.QueryProcessorTests+MyClass] provided an empty result");
    }

    private IQueryProcessor CreateSut(MyClass[] items)
    {
        _dataProviderMock.ResultDelegate = new Func<IQuery, IEnumerable?>
        (
            query => items.Where
            (
                item => query.Filter.Evaluate(item).GetValueOrThrow("Evaluation failed")
            )
        );
        _dataProviderMock.ReturnValue = true;
        return _serviceProvider.GetRequiredService<IQueryProcessor>();
    }

    private IContextQueryProcessor CreateContextSut(MyClass[] items)
    {
        _contextDataProviderMock.ContextResultDelegate = new Func<IQuery, object?, IEnumerable?>
        (
            (query, ctx) => items.Where
            (
                item =>query.Filter.Evaluate(new SurrogateContext(item, ctx)).GetValueOrThrow("Evaluation failed")
            )
        );
        _contextDataProviderMock.ReturnValue = true;
        return _serviceProvider.GetRequiredService<IContextQueryProcessor>();
    }

    public void Dispose() => _serviceProvider.Dispose();

    public sealed class MyClass
    {
        public string? Property { get; set; }
        public string? Property2 { get; set; }
    }

    public sealed class UnsupportedOperatorBuilder : OperatorBuilder
    {
        public override Operator Build() => new UnsupportedOperator();
    }

    public sealed record UnsupportedOperator : Operator
    {
        public override OperatorBuilder ToBuilder()
        {
            throw new NotImplementedException();
        }

        protected override Result<bool> Evaluate(object? leftValue, object? rightValue)
        {
            throw new NotImplementedException();
        }
    }

    private sealed record SurrogateContext
    {
        public SurrogateContext(MyClass item, object? context)
        {
            Item = item;
            Context = context;
        }

        public MyClass Item { get; }
        public object? Context { get; }
    }
}

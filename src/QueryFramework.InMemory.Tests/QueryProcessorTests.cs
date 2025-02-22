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
        Action a = () => sut.FindPaged<MyClass>(query);
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("Evaluation failed");
    }

    [Fact(Skip = "Not working with Shouldly, don't know why")]
    public async Task Unknown_FieldName_Throws_On_FindPagedAsync()
    {
        // Arrange
        var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
        var sut = CreateSut(items);
        var query = new SingleEntityQueryBuilder()
            .Where("UnknownField").IsEqualTo("something")
            .Build();

        // Act & Assert
        Task t = sut.FindPagedAsync<MyClass>(query);
        //Action a = async () => await sut.FindPagedAsync<MyClass>(query);
        (await t.ShouldThrowAsync<InvalidOperationException>())
        //a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("Evaluation failed");
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
        actual.ShouldNotBeNull();
        actual?.Property.ShouldBe("A");
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
        actual.ShouldNotBeNull();
        actual?.Property.ShouldBe("A");
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
        actual.Count.ShouldBe(2);
        actual.First().Property.ShouldBe("A");
        actual.Last().Property.ShouldBe("B");
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
        actual.Count.ShouldBe(2);
        actual.First().Property.ShouldBe("A");
        actual.Last().Property.ShouldBe("B");
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
        actual.Count.ShouldBe(2);
        actual.First().Property.ShouldBe("A");
        actual.Last().Property.ShouldBe("B");
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
        actual.Count.ShouldBe(2);
        actual.First().Property.ShouldBe("A");
        actual.Last().Property.ShouldBe("B");
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("B");
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
        actual.Count.ShouldBe(0);
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("A");
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("Pizza");
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("Beer");
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("Beer");
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("Pizza");
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("Beer");
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("Pizza");
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
        actual.Count.ShouldBe(0);
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
        actual.Count.ShouldBe(2);
        actual.First().Property.ShouldBe("Pizza");
        actual.Last().Property.ShouldBe("Beer");
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
        actual.Count.ShouldBe(0);
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
        actual.Count.ShouldBe(2);
        actual.First().Property.ShouldBe("Pizza");
        actual.Last().Property.ShouldBe("Beer");
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("Beer");
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("Beer");
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("Pizza");
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("Pizza");
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("B");
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("B");
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
        actual.Count.ShouldBe(3);
        actual.ElementAt(0).Property.ShouldBe("A");
        actual.ElementAt(1).Property.ShouldBe("B");
        actual.ElementAt(2).Property.ShouldBe("C");
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
        actual.Count.ShouldBe(3);
        actual.ElementAt(0).Property.ShouldBe("C");
        actual.ElementAt(1).Property.ShouldBe("B");
        actual.ElementAt(2).Property.ShouldBe("A");
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
        actual.ShouldNotBeNull();
        actual?.Property.ShouldBe("C");
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
        actual.ShouldNotBeNull();
        actual?.Property.ShouldBe("C");
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
        actual.Count.ShouldBe(3);
        actual.ElementAt(0).Property.ShouldBe("C");
        actual.ElementAt(1).Property.ShouldBe("B");
        actual.ElementAt(2).Property.ShouldBe("A");
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
        actual.Count.ShouldBe(3);
        actual.ElementAt(0).Property.ShouldBe("B");
        actual.ElementAt(1).Property.ShouldBe("C");
        actual.ElementAt(2).Property.ShouldBe("A");
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
        actual.Count.ShouldBe(1);
        actual.First().Property.ShouldBe("A2");
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
        Action a = () => sut.FindOne<MyClass>(query);
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("Query type [QueryFramework.Core.Queries.SingleEntityQuery] for data type [QueryFramework.InMemory.Tests.QueryProcessorTests+MyClass] does not have a data provider");
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
        Action a = () => sut.FindMany<MyClass>(query);
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("Query type [QueryFramework.Core.Queries.SingleEntityQuery] for data type [QueryFramework.InMemory.Tests.QueryProcessorTests+MyClass] does not have a data provider");
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
        Action a = () => sut.FindPaged<MyClass>(query);
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("Query type [QueryFramework.Core.Queries.SingleEntityQuery] for data type [QueryFramework.InMemory.Tests.QueryProcessorTests+MyClass] does not have a data provider");
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
        Action a = () => sut.FindOne<MyClass>(query);
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("Data provider of type [QueryFramework.InMemory.Tests.TestHelpers.DataProviderMock] for data type [QueryFramework.InMemory.Tests.QueryProcessorTests+MyClass] provided an empty result");
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
}

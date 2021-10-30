using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Extensions;
using QueryFramework.Core.Queries.Builders;
using QueryFramework.Core.Queries.Builders.Extensions;
using Xunit;

namespace QueryFramework.InMemory.Tests
{
    [ExcludeFromCodeCoverage]
    public class QueryProcessorTests
    {
        [Fact]
        public void Unsupported_Query_Operator_Throws_On_FindPAged()
        {
            // Arrange
            var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
            var sut = CreateSut(items);
            var query = new SingleEntityQueryBuilder()
                .Where(new QueryConditionBuilder
                {
                    Field = new QueryExpressionBuilder { FieldName = nameof(MyClass.Property) },
                    Operator = (QueryOperator)99
                }).Build();

            // Act & Assert
            sut.Invoking(x => x.FindPaged(query))
               .Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().StartWith("Unsupported query operator: 99");
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
            sut.Invoking(x => x.FindPaged(query))
               .Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().StartWith("Fieldname [UnknownField] is not found on type [QueryFramework.InMemory.Tests.QueryProcessorTests+MyClass]");
        }

        [Fact]
        public void Unsupported_Expression_Throws_On_FindPaged()
        {
            // Arrange
            var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
            var sut = CreateSut(items);
            var query = new SingleEntityQueryBuilder()
                .Where(new QueryExpression(nameof(MyClass.Property), "UNKNOWN({0})").IsEqualTo("something"))
                .Build();

            // Act & Assert
            sut.Invoking(x => x.FindPaged(query))
               .Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().StartWith("Expression [UNKNOWN(Property)] is not supported");
        }

        [Fact]
        public void Can_FindOne_On_InMemoryList_With_Zero_Conditions()
        {
            // Arrange
            var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
            var sut = CreateSut(items);
            var query = new SingleEntityQueryBuilder().Build();

            // Act
            var actual = sut.FindOne(query);

            // Assert
            actual.Should().NotBeNull();
            actual.Property.Should().Be("A");
        }

        [Fact]
        public void Can_FindMany_On_InMemoryList_With_Zero_Conditions()
        {
            // Arrange
            var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
            var sut = CreateSut(items);
            var query = new SingleEntityQueryBuilder().Build();

            // Act
            var actual = sut.FindMany(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("B");
        }

        [Fact]
        public void Can_FindPaged_On_InMemoryList_With_Two_Equals_Conditions_Using_Or_Combination()
        {
            // Arrange
            var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
            var sut = CreateSut(items);
            var query = new SingleEntityQueryBuilder()
                .Where(nameof(MyClass.Property).IsEqualTo("B"))
                .Or(nameof(MyClass.Property).IsEqualTo("A"))
                .Build();

            // Act
            var actual = sut.FindPaged(query);

            // Assert
            actual.Should().HaveCount(2);
            actual.First().Property.Should().Be("A");
            actual.Last().Property.Should().Be("B");
        }

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("A");
        }

        [Fact]
        public void Can_FindPaged_On_InMemoryList_With_One_NotEquals_Condition_And_Brackets()
        {
            // Arrange
            var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
            var sut = CreateSut(items);
            var query = new SingleEntityQueryBuilder()
                .OrAll(nameof(MyClass.Property).IsNotEqualTo("B"))
                .Build();

            // Act
            var actual = sut.FindPaged(query);

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
                .Where(nameof(MyClass.Property).DoesContain("zz"))
                .Build();

            // Act
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

            // Assert
            actual.Should().HaveCount(2);
            actual.First().Property.Should().Be("Pizza");
            actual.Last().Property.Should().Be("Beer");
        }

        [Fact]
        public void Can_FindPaged_On_InMemoryList_With_One_LowerThan_Condition()
        {
            // Arrange
            var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
            var sut = CreateSut(items);
            var query = new SingleEntityQueryBuilder()
                .Where(nameof(MyClass.Property).IsLowerThan("Coconut"))
                .Build();

            // Act
            var actual = sut.FindPaged(query);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("Beer");
        }

        [Fact]
        public void Can_FindPaged_On_InMemoryList_With_One_LowerOrEqualThan_Condition()
        {
            // Arrange
            var items = new[] { new MyClass { Property = "Pizza" }, new MyClass { Property = "Beer" } };
            var sut = CreateSut(items);
            var query = new SingleEntityQueryBuilder()
                .Where(nameof(MyClass.Property).IsLowerOrEqualThan("Beer"))
                .Build();

            // Act
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
            var actual = sut.FindPaged(query);

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
                .Where(new QueryExpression(nameof(MyClass.Property), "LEN({0})").IsEqualTo(2))
                .Build();

            // Act
            var actual = sut.FindPaged(query);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("A2");
        }

        [Fact]
        public void Can_FindPaged_On_InMemoryList_With_Left_Expression()
        {
            // Arrange
            var items = new[] { new MyClass { Property = "A2" }, new MyClass { Property = "B23" } };
            var sut = CreateSut(items);
            var query = new SingleEntityQueryBuilder()
                .Where(new QueryExpression(nameof(MyClass.Property), "LEFT({0},1)").IsEqualTo("B"))
                .Build();

            // Act
            var actual = sut.FindPaged(query);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("B23");
        }

        [Fact]
        public void Can_FindPaged_On_InMemoryList_With_Right_Expression()
        {
            // Arrange
            var items = new[] { new MyClass { Property = "A2" }, new MyClass { Property = "B23" } };
            var sut = CreateSut(items);
            var query = new SingleEntityQueryBuilder()
                .Where(new QueryExpression(nameof(MyClass.Property), "RIGHT({0},1)").IsEqualTo("2"))
                .Build();

            // Act
            var actual = sut.FindPaged(query);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("A2");
        }

        [Fact]
        public void Can_FindPaged_On_InMemoryList_With_Upper_Expression()
        {
            // Arrange
            var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "b" } };
            var sut = CreateSut(items);
            var query = new SingleEntityQueryBuilder()
                .Where(new QueryExpression(nameof(MyClass.Property), "UPPER({0})").IsEqualTo("B"))
                .Build();

            // Act
            var actual = sut.FindPaged(query);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("b");
        }

        [Fact]
        public void Can_FindPaged_On_InMemoryList_With_Lower_Expression()
        {
            // Arrange
            var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" } };
            var sut = CreateSut(items);
            var query = new SingleEntityQueryBuilder()
                .Where(new QueryExpression(nameof(MyClass.Property), "LOWER({0})").IsEqualTo("b"))
                .Build();

            // Act
            var actual = sut.FindPaged(query);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("B");
        }

        [Fact]
        public void Can_FindPaged_On_InMemoryList_With_Trim_Expression()
        {
            // Arrange
            var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B " } };
            var sut = CreateSut(items);
            var query = new SingleEntityQueryBuilder()
                .Where(new QueryExpression(nameof(MyClass.Property), "TRIM({0})").IsEqualTo("B"))
                .Build();

            // Act
            var actual = sut.FindPaged(query);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("B ");
        }

        [Fact]
        public void Query_With_Null_Properties_Results_In_Same_Result_As_Input()
        {
            // Arrange
            var items = new[] { new MyClass { Property = "A" }, new MyClass { Property = "B" }, new MyClass { Property = "C" } };
            var sut = CreateSut(items);
            var query = new Mock<ISingleEntityQuery>().Object;

            // Act
            var actual = sut.FindMany(query);

            // Assert
            actual.Should().BeEquivalentTo(items);
        }

        private static QueryProcessor<ISingleEntityQuery, MyClass> CreateSut(MyClass[] items)
            => new QueryProcessor<ISingleEntityQuery, MyClass>(items, new ExpressionEvaluator<MyClass>(new ValueProvider()));

        // Expressions: coalesce?

        [ExcludeFromCodeCoverage]
        private class MyClass
        {
            public string Property { get; set; }
            public string Property2 { get; set; }
        }
    }
}

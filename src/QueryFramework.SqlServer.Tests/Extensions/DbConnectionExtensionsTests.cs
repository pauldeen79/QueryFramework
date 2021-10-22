using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Stub;
using System.Data.Stub.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Extensions;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class DbConnectionExtensionsTests
    {
        [Fact]
        public void Query_Throws_On_Null_Query()
        {
            // Arrange
            using var sut = new DbConnection();

            // Act
            sut.Invoking(x => x.Query(null, Map))
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("query");
        }

        [Fact]
        public void Query_Throws_On_Null_MapFunction()
        {
            // Arrange
            using var sut = new DbConnection();

            // Act
            sut.Invoking(x => x.Query<MyEntity>(new Mock<ISingleEntityQuery>().Object, null, tableName: "Table"))
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("mapFunction");
        }

        [Fact]
        public void Query_Returns_MappedEntities()
        {
            // Arrange
            using var sut = new DbConnection();
            sut.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });

            // Act
            var actual = sut.Query(new Mock<ISingleEntityQuery>().Object, Map, tableName: "Table");

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("Value");
            actual.TotalRecordCount.Should().Be(1);
        }

        [Fact]
        public void Query_Returns_MappedEntities_With_FinalizeDelegate()
        {
            // Arrange
            using var sut = new DbConnection();
            sut.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });
            var finalizeDelegateIsCalled = false;

            // Act
            var actual = sut.Query(new Mock<ISingleEntityQuery>().Object,
                                   Map,
                                   tableName: "Table",
                                   finalizeAction: (result, exception) => { finalizeDelegateIsCalled = true; return result; });

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("Value");
            actual.TotalRecordCount.Should().Be(1);
            finalizeDelegateIsCalled.Should().BeTrue();
        }

        [Fact]
        public void Query_Fills_TotalRecordCount_On_Paged_Query()
        {
            // Arrange
            using var sut = new DbConnection();
            sut.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });
            sut.AddResultForScalarCommand(10);
            var queryMock = new Mock<ISingleEntityQuery>();
            queryMock.SetupGet(x => x.Limit).Returns(1);

            // Act
            var actual = sut.Query(queryMock.Object, Map, tableName: "Table");

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("Value");
            actual.TotalRecordCount.Should().Be(10);
        }

        [Fact]
        public void Query_Invokes_FinalizeDelegate_On_Exception()
        {
            // Arrange
            using var sut = new DbConnection();
            sut.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });
            var finalizeDelegateIsCalled = false;

            // Act
            sut.Invoking(x => x.Query<MyEntity>(new Mock<ISingleEntityQuery>().Object,
                                                _ => throw new InvalidOperationException("Kaboom"),
                                                tableName: "Table",
                                                finalizeAction: (result, exception) => { finalizeDelegateIsCalled = true; return result; }))
               .Should().Throw<InvalidOperationException>()
               .And.Message.Should().Be("Kaboom");

            // Assert
            finalizeDelegateIsCalled.Should().BeTrue();
        }

        [Fact]
        public void FindOne_Throws_On_Null_Query()
        {
            // Arrange
            using var sut = new DbConnection();

            // Act
            sut.Invoking(x => x.FindOne(null, Map))
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("query");
        }

        [Fact]
        public void FindOne_Throws_On_Null_MapFunction()
        {
            // Arrange
            using var sut = new DbConnection();

            // Act
            sut.Invoking(x => x.FindOne<MyEntity>(new Mock<ISingleEntityQuery>().Object, null, tableName: "Table"))
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("mapFunction");
        }

        [Fact]
        public void FindOne_Returns_MappedEntities()
        {
            // Arrange
            using var sut = new DbConnection();
            sut.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });

            // Act
            var actual = sut.FindOne(new Mock<ISingleEntityQuery>().Object, Map, tableName: "Table");

            // Assert
            actual.Should().NotBeNull();
            actual.Property.Should().Be("Value");
        }

        [Fact]
        public void FindOne_Returns_MappedEntities_With_FinalizeDelegate()
        {
            // Arrange
            using var sut = new DbConnection();
            sut.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });
            var finalizeDelegateIsCalled = false;

            // Act
            var actual = sut.FindOne(new Mock<ISingleEntityQuery>().Object,
                                     Map,
                                     tableName: "Table",
                                     finalizeAction: (result, exception) => { finalizeDelegateIsCalled = true; return result; });

            // Assert
            actual.Should().NotBeNull();
            actual.Property.Should().Be("Value");
            finalizeDelegateIsCalled.Should().BeTrue();
        }

        [Fact]
        public void FindOne_Invokes_FinalizeDelegate_On_Exception()
        {
            // Arrange
            using var sut = new DbConnection();
            sut.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });
            var finalizeDelegateIsCalled = false;

            // Act
            sut.Invoking(x => x.FindOne<MyEntity>(new Mock<ISingleEntityQuery>().Object,
                                                  _ => throw new InvalidOperationException("Kaboom"),
                                                  tableName: "Table",
                                                  finalizeAction: (result, exception) => { finalizeDelegateIsCalled = true; return result; }))
               .Should().Throw<InvalidOperationException>()
               .And.Message.Should().Be("Kaboom");

            // Assert
            finalizeDelegateIsCalled.Should().BeTrue();
        }

        [Fact]
        public void FindMany_Throws_On_Null_Query()
        {
            // Arrange
            using var sut = new DbConnection();

            // Act
            sut.Invoking(x => x.FindMany(null, Map))
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("query");
        }

        [Fact]
        public void FindMany_Throws_On_Null_MapFunction()
        {
            // Arrange
            using var sut = new DbConnection();

            // Act
            sut.Invoking(x => x.FindMany<MyEntity>(new Mock<ISingleEntityQuery>().Object, null, tableName: "Table"))
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("mapFunction");
        }

        [Fact]
        public void FindMany_Returns_MappedEntities()
        {
            // Arrange
            using var sut = new DbConnection();
            sut.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });

            // Act
            var actual = sut.FindMany(new Mock<ISingleEntityQuery>().Object, Map, tableName: "Table");

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("Value");
        }

        [Fact]
        public void FindMany_Returns_MappedEntities_With_FinalizeDelegate()
        {
            // Arrange
            using var sut = new DbConnection();
            sut.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });
            var finalizeDelegateIsCalled = false;

            // Act
            var actual = sut.FindMany(new Mock<ISingleEntityQuery>().Object,
                                      Map,
                                      tableName: "Table",
                                      finalizeAction: (result, exception) => { finalizeDelegateIsCalled = true; return result; });

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("Value");
            finalizeDelegateIsCalled.Should().BeTrue();
        }

        [Fact]
        public void FindMany_Invokes_FinalizeDelegate_On_Exception()
        {
            // Arrange
            using var sut = new DbConnection();
            sut.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });
            var finalizeDelegateIsCalled = false;

            // Act
            sut.Invoking(x => x.FindMany<MyEntity>(new Mock<ISingleEntityQuery>().Object,
                                                   _ => throw new InvalidOperationException("Kaboom"),
                                                   tableName: "Table",
                                                   finalizeAction: (result, exception) => { finalizeDelegateIsCalled = true; return result; }))
               .Should().Throw<InvalidOperationException>()
               .And.Message.Should().Be("Kaboom");

            // Assert
            finalizeDelegateIsCalled.Should().BeTrue();
        }

        private static MyEntity Map(IDataReader reader) => new MyEntity { Property = reader.GetString(0) };

        [ExcludeFromCodeCoverage]
        private class MyEntity
        {
            public string Property { get; set; }
        }
    }
}

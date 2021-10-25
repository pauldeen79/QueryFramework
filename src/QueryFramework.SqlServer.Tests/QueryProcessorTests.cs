using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Stub;
using System.Data.Stub.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Data.Core;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;
using QueryFramework.Core.Extensions;
using QueryFramework.Core.Queries.Builders;
using QueryFramework.Core.Queries.Builders.Extensions;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Tests.Fixtures;
using Xunit;

namespace QueryFramework.SqlServer.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class QueryProcessorTests : IDisposable
    {
        private DbConnection Connection { get; }
        private DbConnectionCallback Callback { get; }
        private Mock<IDataReaderMapper<MyEntity>> MapperMock { get; }
        private Mock<IDatabaseCommandGenerator> DatabaseCommandGeneratorMock { get; }

        public QueryProcessorTests()
        {
            Callback = new DbConnectionCallback();
            Connection = new DbConnection().AddCallback(Callback);
            MapperMock = new Mock<IDataReaderMapper<MyEntity>>();
            MapperMock.Setup(x => x.Map(It.IsAny<IDataReader>()))
                      .Returns<IDataReader>(reader => new MyEntity { Property = reader.GetString(0) });
            DatabaseCommandGeneratorMock = new Mock<IDatabaseCommandGenerator>();
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        [Fact]
        public void FindPaged_Throws_On_Null_Query()
        {
            // Arrange
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(), DatabaseCommandGeneratorMock.Object);

            // Act
            sut.Invoking(x => x.FindPaged(null))
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("query");
        }

        [Fact]
        public void FindPaged_Returns_MappedEntities()
        {
            // Arrange
            SetupDatabaseCommandGenerator("SELECT * From UnitTest"); // Doesn't matter what command text we use, as long as it's not empty
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(), DatabaseCommandGeneratorMock.Object);
            Connection.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });

            // Act
            var actual = sut.FindPaged(new Mock<ISingleEntityQuery>().Object);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("Value");
            actual.TotalRecordCount.Should().Be(1);
        }

        [Fact]
        public void FindPaged_Fills_TotalRecordCount_On_Paged_Query()
        {
            // Arrange
            SetupDatabaseCommandGenerator("SELECT * From UnitTest"); // Doesn't matter what command text we use, as long as it's not empty
            SetupDatabaseCommandGeneratorForCountQuery("SELECT COUNT(*) From UnitTest"); // Doesn't matter what command text we use, as long as it's not empty
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(), DatabaseCommandGeneratorMock.Object);
            Connection.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });
            Connection.AddResultForScalarCommand(10);
            var queryMock = new Mock<ISingleEntityQuery>();
            queryMock.SetupGet(x => x.Limit).Returns(1);

            // Act
            var actual = sut.FindPaged(queryMock.Object);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("Value");
            actual.TotalRecordCount.Should().Be(10);
        }

        [Fact]
        public void FindOne_Throws_On_Null_Query()
        {
            // Arrange
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(), DatabaseCommandGeneratorMock.Object);

            // Act
            sut.Invoking(x => x.FindOne(null))
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("query");
        }

        [Fact]
        public void FindOne_Returns_MappedEntity()
        {
            // Arrange
            SetupDatabaseCommandGenerator("SELECT * From UnitTest"); // Doesn't matter what command text we use, as long as it's not empty
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(), DatabaseCommandGeneratorMock.Object);
            Connection.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });

            // Act
            var actual = sut.FindOne(new Mock<ISingleEntityQuery>().Object);

            // Assert
            actual.Should().NotBeNull();
            actual.Property.Should().Be("Value");
        }

        [Fact]
        public void FindMany_Throws_On_Null_Query()
        {
            // Arrange
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(), DatabaseCommandGeneratorMock.Object);

            // Act
            sut.Invoking(x => x.FindMany(null))
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("query");
        }

        [Fact]
        public void FindMany_Returns_MappedEntities()
        {
            // Arrange
            SetupDatabaseCommandGenerator("SELECT * From UnitTest"); // Doesn't matter what command text we use, as long as it's not empty
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(), DatabaseCommandGeneratorMock.Object);
            Connection.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });

            // Act
            var actual = sut.FindMany(new Mock<ISingleEntityQuery>().Object);

            // Assert
            actual.Should().HaveCount(1);
            actual.First().Property.Should().Be("Value");
        }

        [Fact]
        public void FindMany_Uses_DynamicQuery_Result_When_Query_Implements_IDynamicQuery()
        {
            // Arrange
            const string Sql = "SELECT Field1, Field2 FROM (SELECT TOP 10 Field1, Field2, ROW_NUMBER() OVER (ORDER BY (SELECT 0)) as sq_row_number FROM MyEntity) sq WHERE sq.sq_row_number BETWEEN 11 and 20;";
            SetupDatabaseCommandGenerator(Sql);
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(fields: "Field1, Field2"), DatabaseCommandGeneratorMock.Object);
            var query = new DynamicQueryMock();

            // Act
            sut.FindMany(query);

            // Assert
            Callback.Commands.Should().HaveCount(1);
            Callback.Commands.First().CommandText.Should().Be(Sql);
        }

        [Fact]
        public void FindMany_Validates_Query_When_ValidateFieldNames_Is_Set_To_True()
        {
            // Arrange
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(), DatabaseCommandGeneratorMock.Object);
            var query = new ValidatableQueryMock();

            // Act & Assert
            sut.Invoking(x => x.FindMany(query))
               .Should().Throw<ValidationException>();
        }

        [Fact]
        public void FindMany_Does_Not_Validate_Query_When_ValidateFieldNames_Is_Set_To_False()
        {
            // Arrange
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(validateFieldNames: false), DatabaseCommandGeneratorMock.Object);
            var query = new ValidatableQueryMock();

            // Act & Assert
            sut.Invoking(x => x.FindMany(query))
               .Should().NotThrow<ValidationException>();
        }

        [Fact]
        public void FindMany_Fills_CommandText_From_Query()
        {
            // Arrange
            const string Sql = "SELECT Field1, Field2 FROM (SELECT Field1, Field2, ROW_NUMBER() OVER (ORDER BY Field2 ASC) as sq_row_number FROM MyEntity WHERE Field1 = @p0 OR Field1 = @p1) sq WHERE sq.sq_row_number BETWEEN 21 and 30;";
            SetupDatabaseCommandGenerator(Sql);
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(), DatabaseCommandGeneratorMock.Object);
            var query = new AdvancedSingleDataObjectQueryBuilder()
                .Select("Field1", "Field2")
                .Limit(10)
                .Offset(20)
                .From("MyEntity")
                .Where("Field1".IsEqualTo("value1"))
                .Or("Field1".IsEqualTo("value2"))
                .OrderBy("Field2")
                .Build();

            // Act
            sut.FindMany(query);

            // Assert
            Callback.Commands.Should().HaveCount(1);
            Callback.Commands.First().CommandText.Should().Be(Sql);
        }

        [Fact]
        public void FindMany_Fills_CommandText_From_Query_With_Distinct_Clause()
        {
            // Arrange
            const string Sql = "SELECT DISTINCT Field1, Field2 FROM MyEntity";
            SetupDatabaseCommandGenerator(Sql);
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(), DatabaseCommandGeneratorMock.Object);
            var query = new AdvancedSingleDataObjectQueryBuilder()
                .SelectDistinct("Field1", "Field2")
                .From("MyEntity")
                .Build();

            // Act
            sut.FindMany(query);

            // Assert
            Callback.Commands.Should().HaveCount(1);
            Callback.Commands.First().CommandText.Should().Be(Sql);
        }

        [Fact]
        public void FindMany_Adds_QueryParameters_When_Available()
        {
            // Arrange
            const string Sql = "SELECT * FROM MyEntity WHERE field = @myparameter";
            SetupDatabaseCommandGenerator(Sql, new { myparameter = "value" });
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(), DatabaseCommandGeneratorMock.Object);
            var query = new ParameterizedQueryMock
            {
                Conditions = new List<IQueryCondition> { new QueryCondition("field", QueryOperator.Equal, new QueryParameterValue("myparameter")) },
                Parameters = new List<IQueryParameter> { new QueryParameter("myparameter", "value") }
            };

            // Act
            sut.FindMany(query);

            // Assert
            Callback.Commands.Should().HaveCount(1);
            Callback.Commands.First().CommandText.Should().Be(Sql);
            Callback.Commands.First().Parameters.Cast<IDbDataParameter>().Should().HaveCount(1);
            Callback.Commands.First().Parameters.Cast<IDbDataParameter>().First().ParameterName.Should().Be("myparameter");
            Callback.Commands.First().Parameters.Cast<IDbDataParameter>().First().Value.Should().Be("value");
        }

        [Fact]
        public void FindMany_Uses_Default_OrderBy_Clause_When_Available()
        {
            // Arrange
            const string Sql = "SELECT TOP 100 Field1, Field2 FROM MyEntity WHERE Field1 = @p0 ORDER BY Field2 ASC";
            SetupDatabaseCommandGenerator(Sql);
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(defaultOrderBy: "Field2 ASC"), DatabaseCommandGeneratorMock.Object);
            var query = new AdvancedSingleDataObjectQueryBuilder()
                .Select("Field1", "Field2")
                .Limit(100)
                .From("MyEntity")
                .Where("Field1".IsEqualTo("value"))
                .Build();

            // Act
            sut.FindMany(query);

            // Assert
            Callback.Commands.Should().HaveCount(1);
            Callback.Commands.First().CommandText.Should().Be(Sql);
        }

        [Fact]
        public void FindMany_Uses_Default_Where_Clause_When_Available()
        {
            // Arrange
            const string Sql = "SELECT TOP 100 Field1, Field2 FROM MyEntity WHERE Field1 = 'Value' ORDER BY Field2 ASC";
            SetupDatabaseCommandGenerator(Sql);
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(defaultWhere: "Field1 = 'Value'"), DatabaseCommandGeneratorMock.Object);
            var query = new AdvancedSingleDataObjectQueryBuilder()
                .Select("Field1", "Field2")
                .Limit(100)
                .From("MyEntity")
                .OrderBy("Field2")
                .Build();

            // Act
            sut.FindMany(query);

            // Assert
            Callback.Commands.Should().HaveCount(1);
            Callback.Commands.First().CommandText.Should().Be(Sql);
        }

        [Fact]
        public void FindMany_Uses_TableName_When_Available()
        {
            // Arrange
            const string Sql = "SELECT TOP 100 Field1, Field2 FROM Table WHERE Field1 = @p0 ORDER BY Field2 ASC";
            SetupDatabaseCommandGenerator(Sql);
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(tableName: "Table"), DatabaseCommandGeneratorMock.Object);
            var query = new AdvancedSingleDataObjectQueryBuilder()
                .Select("Field1", "Field2")
                .Limit(100)
                .Where("Field1".IsEqualTo("value"))
                .OrderBy("Field2")
                .Build();

            // Act
            sut.FindMany(query);

            // Assert
            DatabaseCommandGeneratorMock.Verify(x => x.Generate(It.IsAny<ISingleEntityQuery>(), It.Is<IQueryProcessorSettings>(x => x.TableName == "Table"), false), Times.Once);
            Callback.Commands.Should().HaveCount(1);
            Callback.Commands.First().CommandText.Should().Be(Sql);
        }

        [Fact]
        public void FindMany_Uses_OverrideLimit_When_Available()
        {
            // Arrange
            const string Sql = "SELECT TOP 10 Field1, Field2 FROM MyEntity WHERE Field1 = @p0 ORDER BY Field2 ASC";
            SetupDatabaseCommandGenerator(Sql);
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(overrideLimit: 10), DatabaseCommandGeneratorMock.Object);
            var query = new AdvancedSingleDataObjectQueryBuilder()
                .Select("Field1", "Field2")
                .Limit(100)
                .From("MyEntity")
                .Where("Field1".IsEqualTo("value"))
                .OrderBy("Field2")
                .Build();

            // Act
            sut.FindMany(query);

            // Assert
            Callback.Commands.Should().HaveCount(1);
            Callback.Commands.First().CommandText.Should().Be(Sql);
        }

        [Fact]
        public void FindMany_Uses_AllFieldsDelegate_When_Available()
        {
            // Arrange
            const string Sql = "SELECT TOP 100 Field1, Field2 FROM MyEntity WHERE Field1 = @p0 ORDER BY Field2 ASC";
            SetupDatabaseCommandGenerator(Sql);
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, new QueryProcessorSettings(getAllFieldsDelegate: () => new[] { "Field1", "Field2" }), DatabaseCommandGeneratorMock.Object);
            var query = new AdvancedSingleDataObjectQueryBuilder()
                .SelectAll()
                .Limit(100)
                .From("MyEntity")
                .Where("Field1".IsEqualTo("value"))
                .OrderBy("Field2")
                .Build();

            // Act
            sut.FindMany(query);

            // Assert
            Callback.Commands.Should().HaveCount(1);
            Callback.Commands.First().CommandText.Should().Be(Sql);
        }

        private void SetupDatabaseCommandGenerator(string sql, object parameters = null)
            => DatabaseCommandGeneratorMock.Setup(x => x.Generate(It.IsAny<ISingleEntityQuery>(), It.IsAny<IQueryProcessorSettings>(), false))
                                           .Returns(new SqlTextCommand(sql, parameters));

        private void SetupDatabaseCommandGeneratorForCountQuery(string sql, object parameters = null)
            => DatabaseCommandGeneratorMock.Setup(x => x.Generate(It.IsAny<ISingleEntityQuery>(), It.IsAny<IQueryProcessorSettings>(), true))
                                   .Returns(new SqlTextCommand(sql, parameters));

    }
}

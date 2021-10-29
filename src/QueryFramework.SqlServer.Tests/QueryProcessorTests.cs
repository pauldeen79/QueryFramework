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
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Tests.TestHelpers;
using Xunit;

namespace QueryFramework.SqlServer.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class QueryProcessorTests : IDisposable
    {
        private DbConnection Connection { get; }
        private DbConnectionCallback Callback { get; }
        private Mock<IDataReaderMapper<MyEntity>> MapperMock { get; }
        private Mock<IQueryProcessorSettings> QueryProcessorSettingsMock { get; }
        private Mock<IDatabaseCommandGenerator> DatabaseCommandGeneratorMock { get; }
        private Mock<IQueryFieldProvider> FieldProviderMock { get; }

        public QueryProcessorTests()
        {
            Callback = new DbConnectionCallback();
            Connection = new DbConnection().AddCallback(Callback);
            MapperMock = new Mock<IDataReaderMapper<MyEntity>>();
            MapperMock.Setup(x => x.Map(It.IsAny<IDataReader>()))
                      .Returns<IDataReader>(reader => new MyEntity { Property = reader.GetString(0) });
            QueryProcessorSettingsMock = new Mock<IQueryProcessorSettings>();
            DatabaseCommandGeneratorMock = new Mock<IDatabaseCommandGenerator>();
            FieldProviderMock = new Mock<IQueryFieldProvider>();
            FieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        [Fact]
        public void FindPaged_Throws_On_Null_Query()
        {
            // Arrange
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, QueryProcessorSettingsMock.Object, DatabaseCommandGeneratorMock.Object, FieldProviderMock.Object);

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
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, QueryProcessorSettingsMock.Object, DatabaseCommandGeneratorMock.Object, FieldProviderMock.Object);
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
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, QueryProcessorSettingsMock.Object, DatabaseCommandGeneratorMock.Object, FieldProviderMock.Object);
            Connection.AddResultForDataReader(new[] { new MyEntity { Property = "Value" } });
            Connection.AddResultForScalarCommand(10);
            var queryMock = new Mock<ISingleEntityQuery>();
            queryMock.SetupGet(x => x.Limit)
                     .Returns(1);

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
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, QueryProcessorSettingsMock.Object, DatabaseCommandGeneratorMock.Object, FieldProviderMock.Object);

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
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, QueryProcessorSettingsMock.Object, DatabaseCommandGeneratorMock.Object, FieldProviderMock.Object);
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
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, QueryProcessorSettingsMock.Object, DatabaseCommandGeneratorMock.Object, FieldProviderMock.Object);

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
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, QueryProcessorSettingsMock.Object, DatabaseCommandGeneratorMock.Object, FieldProviderMock.Object);
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
            QueryProcessorSettingsMock.SetupGet(x => x.Fields)
                                      .Returns("Field1, Field2");
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, QueryProcessorSettingsMock.Object, DatabaseCommandGeneratorMock.Object, FieldProviderMock.Object);
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
            QueryProcessorSettingsMock.SetupGet(x => x.ValidateFieldNames)
                                      .Returns(true);
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, QueryProcessorSettingsMock.Object, DatabaseCommandGeneratorMock.Object, FieldProviderMock.Object);
            var query = new ValidatableQueryMock();

            // Act & Assert
            sut.Invoking(x => x.FindMany(query))
               .Should().Throw<ValidationException>();
        }

        [Fact]
        public void FindMany_Does_Not_Validate_Query_When_ValidateFieldNames_Is_Set_To_False()
        {
            // Arrange
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(Connection, MapperMock.Object, QueryProcessorSettingsMock.Object, DatabaseCommandGeneratorMock.Object, FieldProviderMock.Object);
            var query = new ValidatableQueryMock();

            // Act & Assert
            sut.Invoking(x => x.FindMany(query))
               .Should().NotThrow<ValidationException>();
        }

        private void SetupDatabaseCommandGenerator(string sql, object parameters = null)
            => DatabaseCommandGeneratorMock.Setup(x => x.Generate(It.IsAny<ISingleEntityQuery>(), It.IsAny<IQueryProcessorSettings>(), It.IsAny<IQueryFieldProvider>(), false))
                                           .Returns(new SqlTextCommand(sql, parameters));

        private void SetupDatabaseCommandGeneratorForCountQuery(string sql, object parameters = null)
            => DatabaseCommandGeneratorMock.Setup(x => x.Generate(It.IsAny<ISingleEntityQuery>(), It.IsAny<IQueryProcessorSettings>(), It.IsAny<IQueryFieldProvider>(), true))
                                   .Returns(new SqlTextCommand(sql, parameters));
    }
}

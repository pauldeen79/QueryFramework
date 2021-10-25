using System.Collections.Generic;
using System.Data;
using System.Data.Stub;
using System.Data.Stub.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;
using QueryFramework.Core.Queries;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Extensions;
using QueryFramework.SqlServer.QueryExpressions;
using QueryFramework.SqlServer.Tests.Fixtures;
using Xunit;

namespace QueryFramework.SqlServer.Tests.QueryExpressions
{
    [ExcludeFromCodeCoverage]
    public class DateTests
    {
        [Fact]
        public void Can_Use_SqlDate_In_Coalesce_Expression()
        {
            // Arrange
            var expression = new QueryExpression("Field");

            // Act
            var actual = expression.Coalesce(new Date(new System.DateTime(2000, 1, 1)));

            // Assert
            ExpressionSqlShouldBe(actual, "COALESCE(Field, '2000-01-01 00:00:00')");
        }

        private static void ExpressionSqlShouldBe(IQueryExpression expression, string expectedSqlForExpression)
        {
            var callback = new DbConnectionCallback();
            using var connection = new DbConnection().AddCallback(callback);
            var mapperMock = new Mock<IDataReaderMapper<MyEntity>>();
            mapperMock.Setup(x => x.Map(It.IsAny<IDataReader>()))
                      .Returns<IDataReader>(reader => new MyEntity { Property = reader.GetString(0) });
            var fieldNameProviderMock = new Mock<IQueryFieldNameProvider>();
            fieldNameProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                                 .Returns<IEnumerable<string>>(input => input);
            var sut = new QueryProcessor<ISingleEntityQuery, MyEntity>(connection, mapperMock.Object, new QueryProcessorSettings(), new DatabaseCommandGenerator(), fieldNameProviderMock.Object);
            var query = new SingleEntityQuery(new[] { new QueryCondition(expression, QueryOperator.Equal, "test") });
            sut.FindMany(query);

            // Assert
            callback.Commands.Should().HaveCount(1);
            callback.Commands.First().CommandText.Should().Be($"SELECT * FROM MyEntity WHERE {expectedSqlForExpression} = @p0");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Common;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql.Builders;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;
using QueryFramework.Core;
using QueryFramework.Core.Extensions;
using QueryFramework.Core.Queries.Builders;
using QueryFramework.Core.Queries.Builders.Extensions;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Extensions;
using QueryFramework.SqlServer.Tests.TestHelpers;
using Xunit;

namespace QueryFramework.SqlServer.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class DatabaseCommandBuilderExtensionsTests
    {
        private PagedSelectCommandBuilder Builder { get; }

        public DatabaseCommandBuilderExtensionsTests()
        {
            Builder = new PagedSelectCommandBuilder();
        }

        [Fact]
        public void Select_Skips_Fields_That_Are_Returned_As_Null_In_GetDatabaseFieldName()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>())).Returns<IEnumerable<string>>(input => input.Where(x => x != "Field2"));
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            Builder.From("MyTable");

            // Act
            var actual = Builder.Select(query, settingsMock.Object, fieldProviderMock.Object, query);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT Field1, Field3 FROM MyTable");
        }

        [Fact]
        public void Select_Uses_Star_When_Fields_Is_Empty_And_GetAllFields_Returns_Null_And_SelectAll_Is_True()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().SelectAll().Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            fieldProviderMock.Setup(x => x.GetAllFields())
                             .Returns(Enumerable.Empty<string>());
            Builder.From("MyTable");

            // Act
            var actual = Builder.Select(query, settingsMock.Object, fieldProviderMock.Object, query);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
        }

        [Fact]
        public void Select_Uses_GetAllFields_When_Provided()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().SelectAll().Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            settingsMock.SetupGet(x => x.Fields)
                                      .Returns(string.Empty);
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            fieldProviderMock.Setup(x => x.GetAllFields())
                             .Returns(new[] { "Field1", "Field2", "Field3" });
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x);
            Builder.From("MyTable");

            // Act
            var actual = Builder.Select(query, settingsMock.Object, fieldProviderMock.Object, query);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT Field1, Field2, Field3 FROM MyTable");
        }

        [Fact]
        public void Select_Uses_Fields_From_Query_When_GetAllFields_Is_False()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>())).Returns<IEnumerable<string>>(input => input);
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            Builder.From("MyTable");

            // Act
            var actual = Builder.Select(query, settingsMock.Object, fieldProviderMock.Object, query);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT Field1, Field2, Field3 FROM MyTable");
        }

        [Fact]
        public void Select_Uses_GetFieldDelegate_When_Result_Is_Not_Null()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x + "A");
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);
            Builder.From("MyTable");

            // Act
            var actual = Builder.Select(query, settingsMock.Object, fieldProviderMock.Object, query);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT Field1A, Field2A, Field3A FROM MyTable");
        }

        [Fact]
        public void Select_Throws_When_GetDatabaseFieldName_Returns_Null()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>()))
                             .Returns<IEnumerable<string>>(input => input);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns(default(string));
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            Builder.Invoking(x => x.Select(query, settingsMock.Object, fieldProviderMock.Object, query))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query fields contains unknown field in expression [Field1]");
        }

        [Fact]
        public void Select_Throws_When_ValidateExpression_Returns_False()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Select("Field1", "Field2", "Field3").Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetSelectFields(It.IsAny<IEnumerable<string>>())).Returns<IEnumerable<string>>(input => input);
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(false);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);

            // Act
            Builder.Invoking(x => x.Select(query, settingsMock.Object, fieldProviderMock.Object, query))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query fields contains invalid expression [Field1]");
        }

        [Fact]
        public void Where_Does_Not_Append_Anything_When_Conditions_And_DefaultWhere_Are_Both_Empty()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            Builder.From("MyTable");

            // Act
            var actual = Builder.Where(query, settingsMock.Object, fieldProviderMock.Object, out _);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
        }

        [Fact]
        public void Where_Adds_Single_Condition_Without_Default_Where_Clause()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            Builder.From("MyTable");

            // Act
            var actual = Builder.Where(query, settingsMock.Object, fieldProviderMock.Object, out _);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field = @p0");
        }

        [Fact]
        public void Where_Adds_Single_Condition_With_Default_Where_Clause()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            settingsMock.SetupGet(x => x.DefaultWhere)
                        .Returns("Field IS NOT NULL");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            Builder.From("MyTable");

            // Act
            var actual = Builder.Where(query, settingsMock.Object, fieldProviderMock.Object, out _);

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
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            Builder.From("MyTable");

            // Act
            var actual = Builder.Where(query, settingsMock.Object, fieldProviderMock.Object, out _);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field = @p0 AND Field2 IS NOT NULL");
        }

        [Fact]
        public void Where_Adds_Multiple_Conditions_With_Default_Where_Clause()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Where("Field".IsEqualTo("value")).And("Field2".IsNotNull()).Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            settingsMock.SetupGet(x => x.DefaultWhere).Returns("Field IS NOT NULL");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            Builder.From("MyTable");

            // Act
            var actual = Builder.Where(query, settingsMock.Object, fieldProviderMock.Object, out _);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE Field IS NOT NULL AND Field = @p0 AND Field2 IS NOT NULL");
        }

        [Fact]
        public void OrderBy_Does_Not_Append_Anything_When_Limit_And_Offset_Are_Both_Filled()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Limit(10).Offset(20).Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            settingsMock.SetupGet(x => x.DefaultOrderBy).Returns("Ignored");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            Builder.From("MyTable");

            // Act
            var actual = Builder.OrderBy(query, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
        }

        [Fact]
        public void OrderBy_Does_Not_Append_Anything_When_Query_OrderByFields_Is_Empty()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            Builder.From("MyTable");

            // Act
            var actual = Builder.OrderBy(query, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
        }

        [Fact]
        public void OrderBy_Appends_Single_OrderBy_Clause()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            Builder.From("MyTable");

            // Act
            var actual = Builder.OrderBy(query, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY Field ASC");
        }

        [Fact]
        public void OrderBy_Appends_Multiple_OrderBy_Clauses()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field1").ThenByDescending("Field2").Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            Builder.From("MyTable");

            // Act
            var actual = Builder.OrderBy(query, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY Field1 ASC, Field2 DESC");
        }

        [Fact]
        public void OrderBy_Appends_Default_OrderBy_Clause()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            settingsMock.SetupGet(x => x.DefaultOrderBy).Returns("Field ASC");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            Builder.From("MyTable");

            // Act
            var actual = Builder.OrderBy(query, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY Field ASC");
        }

        [Fact]
        public void OrderBy_Does_Not_Append_DefaultOrderBy_When_OrderBy_Clause_Is_Present_On_Query()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            settingsMock.SetupGet(x => x.DefaultOrderBy).Returns("Ignored");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            Builder.From("MyTable");

            // Act
            var actual = Builder.OrderBy(query, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY Field ASC");
        }

        [Fact]
        public void OrderBy_Appends_Single_OrderBy_Clause_With_GetDatabaseFieldName()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x + "A");
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);
            Builder.From("MyTable");

            // Act
            var actual = Builder.OrderBy(query, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable ORDER BY FieldA ASC");
        }

        [Fact]
        public void OrderBy_Throws_When_GetDatabaseFieldName_Returns_Null()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();

            // Act & Assert
            Builder.Invoking(x => x.OrderBy(query, settingsMock.Object, fieldProviderMock.Object))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query order by fields contains unknown field [Field]");
        }

        [Fact]
        public void OrderBy_Throws_When_ValidateExpression_Returns_False_And_GetFieldName_Returns_Null()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns(default(string));
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(false);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);

            // Act & Assert
            Builder.Invoking(x => x.OrderBy(query, settingsMock.Object, fieldProviderMock.Object))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query order by fields contains invalid expression [Field]");
        }

        [Fact]
        public void OrderBy_Throws_When_ValidateExpression_Returns_False_And_GetFieldName_Returns_NonNullValue()
        {
            // Arrange
            var query = new FieldSelectionQueryBuilder().OrderBy("Field").Build();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x);
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(false);

            // Act & Assert
            Builder.Invoking(x => x.OrderBy(query, settingsMock.Object, fieldProviderMock.Object))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query order by fields contains invalid expression [Field]");
        }

        [Fact]
        public void GroupBy_Does_Not_Append_Anything_When_GroupByFields_Is_Null()
        {
            // Arrange
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            settingsMock.SetupGet(x => x.TableName).Returns("MyTable");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            Builder.From("MyTable");

            // Act
            var actual = Builder.GroupBy(null, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
        }

        [Fact]
        public void GroupBy_Does_Not_Append_Anything_When_GroupByFields_Is_Empty()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            Builder.From("MyTable");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();

            // Act
            var actual = Builder.GroupBy(queryMock.Object, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
        }

        [Fact]
        public void GroupBy_Appends_Single_GroupBy_Clause()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            queryMock.SetupGet(x => x.GroupByFields)
                     .Returns(new ValueCollection<IQueryExpression>(new[] { new QueryExpression("Field") }));
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            Builder.From("MyTable");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);

            // Act
            var actual = Builder.GroupBy(queryMock.Object, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable GROUP BY Field");
        }

        [Fact]
        public void GroupBy_Appends_Multiple_GroupBy_Clauses()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            queryMock.SetupGet(x => x.GroupByFields)
                     .Returns(new ValueCollection<IQueryExpression>(new[] { new QueryExpression("Field1"), new QueryExpression("Field2") }));
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            Builder.From("MyTable");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);

            // Act
            var actual = Builder.GroupBy(queryMock.Object, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable GROUP BY Field1, Field2");
        }

        [Fact]
        public void GroupBy_Appends_GroupBy_Clause_With_GetDatabaseFieldName()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            queryMock.SetupGet(x => x.GroupByFields)
                     .Returns(new ValueCollection<IQueryExpression>(new[] { new QueryExpression("Field") }));
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            Builder.From("MyTable");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x + "A");
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act
            var actual = Builder.GroupBy(queryMock.Object, settingsMock.Object, fieldProviderMock.Object);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable GROUP BY FieldA");
        }

        [Fact]
        public void GroupBy_Throws_When_GetDatabaseFieldName_Returns_Null()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            queryMock.SetupGet(x => x.GroupByFields)
                     .Returns(new ValueCollection<IQueryExpression>(new[] { new QueryExpression("Field") }));
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            settingsMock.SetupGet(x => x.TableName).Returns("MyTable");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns(default(string));
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);

            // Act & Assert
            Builder.Invoking(x => x.GroupBy(queryMock.Object, settingsMock.Object, fieldProviderMock.Object))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query group by fields contains unknown field [Field]");
        }

        [Fact]
        public void GroupBy_Throws_When_ValidateExpression_Returns_False()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            queryMock.SetupGet(x => x.GroupByFields)
                     .Returns(new ValueCollection<IQueryExpression>(new[] { new QueryExpression("Field") }));
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            settingsMock.SetupGet(x => x.TableName).Returns("MyTable");
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x);
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(false);

            // Act & Assert
            Builder.Invoking(x => x.GroupBy(queryMock.Object, settingsMock.Object, fieldProviderMock.Object))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query group by fields contains invalid expression [Field]");
        }

        [Fact]
        public void Having_Adds_Single_Condition()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            queryMock.SetupGet(x => x.HavingFields)
                     .Returns(new ValueCollection<IQueryCondition>(new[] { new QueryCondition("Field", QueryOperator.Equal, "value") }));
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            int paramCounter = 0;
            Builder.From("MyTable");

            // Act
            var actual = Builder.Having(queryMock.Object, settingsMock.Object, fieldProviderMock.Object, ref paramCounter);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable HAVING Field = @p0");
        }

        [Fact]
        public void Having_Adds_Multiple_Conditions()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            queryMock.SetupGet(x => x.HavingFields)
                     .Returns(new ValueCollection<IQueryCondition>(new[]
                     {
                         new QueryCondition("Field1", QueryOperator.Equal, "value1"),
                         new QueryCondition("Field2", QueryOperator.Equal, "value2")
                     }));
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            int paramCounter = 0;
            Builder.From("MyTable");

            // Act
            var actual = Builder.Having(queryMock.Object, settingsMock.Object, fieldProviderMock.Object, ref paramCounter);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable HAVING Field1 = @p0 AND Field2 = @p1");
        }

        [Fact]
        public void Having_Does_Not_Append_Anything_When_HavingFields_Is_Null()
        {
            // Arrange
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            int paramCounter = 0;
            Builder.From("MyTable");

            // Act
            var actual = Builder.Having(null, settingsMock.Object, fieldProviderMock.Object, ref paramCounter);

            // Assert
            actual.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable");
        }

        [Fact]
        public void Having_Does_Not_Append_Anything_When_HavingFields_Is_Empty()
        {
            // Arrange
            var queryMock = new Mock<IGroupingQuery>();
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            int paramCounter = 0;
            Builder.From("MyTable");

            // Act
            var actual = Builder.Having(queryMock.Object, settingsMock.Object, fieldProviderMock.Object, ref paramCounter);

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
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            Builder.From("MyTable");
            if (shouldAddCombination)
            {
                Builder.Where("Field IS NOT NULL");
            }

            // Act
            var actual = Builder.AppendQueryCondition(paramCounter,
                                                      new QueryCondition("Field", QueryOperator.Greater, "value"),
                                                      settingsMock.Object,
                                                      fieldProviderMock.Object,
                                                      Builder.Where);

            // Assert
            if (shouldAddCombination)
            {
                Builder.Build().DataCommand.CommandText.Should().Be($"SELECT * FROM MyTable WHERE Field IS NOT NULL AND Field > @p{paramCounter}");
            }
            else
            {
                Builder.Build().DataCommand.CommandText.Should().Be($"SELECT * FROM MyTable WHERE Field > @p{paramCounter}");
            }
            actual.Should().Be(paramCounter + 1);
        }

        [Fact]
        public void AppendQueryCondition_Adds_Brackets_When_Necessary()
        {
            // Arrange
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            Builder.From("MyTable");

            // Act
            Builder.AppendQueryCondition(0,
                                         new QueryCondition("Field", QueryOperator.Greater, "value", true, true),
                                         settingsMock.Object,
                                         fieldProviderMock.Object,
                                         Builder.Where);

            // Assert
            Builder.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE (Field > @p0)");
        }

        [Fact]
        public void AppendQueryCondition_Gets_CustomFieldName_When_Possible()
        {
            // Arrange
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x == "Field" ? "CustomField" : x);
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);
            Builder.From("MyTable");

            // Act
            Builder.AppendQueryCondition(0,
                                         new QueryCondition("Field", QueryOperator.Greater, "value"),
                                         settingsMock.Object,
                                         fieldProviderMock.Object,
                                         Builder.Where);

            // Assert
            Builder.Build().DataCommand.CommandText.Should().Be("SELECT * FROM MyTable WHERE CustomField > @p0");
        }

        [Fact]
        public void AppendQueryCondition_Throws_On_Invalid_CustomFieldName()
        {
            // Arrange
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>()))
                             .Returns<string>(x => x == "Field" ? null : x);
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>()))
                             .Returns(true);
            Builder.From("MyTable");

            // Act
            Builder.Invoking(x => x.AppendQueryCondition(0,
                                                         new QueryCondition("Field", QueryOperator.Greater, "value"),
                                                         settingsMock.Object,
                                                         fieldProviderMock.Object,
                                                         Builder.Where))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query conditions contains unknown field [Field]");
        }

        [Fact]
        public void AppendQueryCondition_Throws_On_Invalid_Expression_When_ValidateExpression_Returns_False()
        {
            // Arrange
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(false);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            Builder.From("MyTable");

            // Act
            Builder.Invoking(x => x.AppendQueryCondition(0,
                                                         new QueryCondition(new QueryExpression("Field", "SUM({0})"), QueryOperator.Greater, "value"),
                                                         settingsMock.Object,
                                                         fieldProviderMock.Object,
                                                         Builder.Where))
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().StartWith("Query conditions contains invalid expression [SUM(Field)]");
        }

        [Theory]
        [InlineData(QueryOperator.IsNullOrEmpty, "COALESCE(Field,'') = ''")]
        [InlineData(QueryOperator.IsNotNullOrEmpty, "COALESCE(Field,'') <> ''")]
        [InlineData(QueryOperator.IsNull, "Field IS NULL")]
        [InlineData(QueryOperator.IsNotNull, "Field IS NOT NULL")]
        public void AppendQueryCondition_Fills_CommandText_Correctly_For_QueryOperator_Without_Value(QueryOperator queryOperator, string expectedCommandText)
        {
            // Arrange
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            Builder.From("MyTable");

            // Act
            Builder.AppendQueryCondition(0,
                                         new QueryCondition("Field", queryOperator),
                                         settingsMock.Object,
                                         fieldProviderMock.Object,
                                         Builder.Where);

            // Assert
            Builder.Build().DataCommand.CommandText.Should().Be($"SELECT * FROM MyTable WHERE {expectedCommandText}");
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
            var settingsMock = new Mock<IPagedDatabaseEntityRetrieverSettings>();
            var fieldProviderMock = new Mock<IQueryFieldProvider>();
            fieldProviderMock.Setup(x => x.ValidateExpression(It.IsAny<IQueryExpression>())).Returns(true);
            fieldProviderMock.Setup(x => x.GetDatabaseFieldName(It.IsAny<string>())).Returns<string>(x => x);
            Builder.From("MyTable");

            // Act
            Builder.AppendQueryCondition(0,
                                         new QueryCondition("Field", queryOperator, "test"),
                                         settingsMock.Object,
                                         fieldProviderMock.Object,
                                         Builder.Where);
            var actual = Builder.Build().DataCommand;

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
            var actual = Builder.WithParameters(query);

            // Assert
            actual.CommandParameters.Should().HaveCount(1);
            actual.CommandParameters.First().Key.Should().Be("name");
            actual.CommandParameters.First().Value.Should().Be("Value");
        }
    }
}

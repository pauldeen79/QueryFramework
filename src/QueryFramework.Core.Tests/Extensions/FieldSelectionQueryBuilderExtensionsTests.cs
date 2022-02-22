namespace QueryFramework.Core.Tests.Extensions;

public class FieldSelectionQueryBuilderExtensionsTests
{
    [Fact]
    public void Can_Use_Select_With_QueryExpressionBuilder_To_Add_Field()
    {
        // Arrange
        var functionBuilder = new Mock<IExpressionFunctionBuilder>().Object;
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.Select(new FieldExpressionBuilder().WithFieldName("FieldName").WithFunction(functionBuilder));

        // Assert
        actual.Fields.Should().HaveCount(1);
        var field = actual.Fields.First() as FieldExpressionBuilder;
        field?.FieldName.Should().Be("FieldName");
        actual.Fields.First().Function.Should().NotBeNull();
        actual.Distinct.Should().BeFalse();
    }

    [Fact]
    public void Can_Use_Select_With_FieldName_String_To_Add_Field()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.Select("FieldName");

        // Assert
        actual.Fields.Should().HaveCount(1);
        var field = actual.Fields.First() as FieldExpressionBuilder;
        field?.FieldName.Should().Be("FieldName");
        actual.Fields.First().Function.Should().BeNull();
        actual.Distinct.Should().BeFalse();
    }

    [Fact]
    public void Can_Use_Select_With_FieldName_String_To_Add_Multiple_Fields()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.Select("FieldName1", "FieldName2");

        // Assert
        actual.Fields.Should().HaveCount(2);
        var firstField = actual.Fields.First() as FieldExpressionBuilder;
        firstField?.FieldName.Should().Be("FieldName1");
        actual.Fields.ElementAt(0).Function.Should().BeNull();
        var lastField = actual.Fields.Last() as FieldExpressionBuilder;
        lastField?.FieldName.Should().Be("FieldName2");
        actual.Fields.ElementAt(1).Function.Should().BeNull();
        actual.Distinct.Should().BeFalse();
    }

    [Fact]
    public void Can_Use_SelectAll_To_Clear_Fields_And_Set_GetAllFields_To_True()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder().Select("FieldName1", "FieldName2");

        // Act
        var actual = sut.SelectAll();

        // Assert
        actual.Fields.Should().BeEmpty();
        actual.GetAllFields.Should().BeTrue();
    }

    [Fact]
    public void Can_Use_SelectDistinct_With_QueryExpressionBuilder_To_Add_Field()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();
        var functionBuilder = new Mock<IExpressionFunctionBuilder>().Object;

        // Act
        var actual = sut.SelectDistinct(new FieldExpressionBuilder().WithFieldName("FieldName").WithFunction(functionBuilder));

        // Assert
        actual.Fields.Should().HaveCount(1);
        var field = actual.Fields.First() as FieldExpressionBuilder;
        field?.FieldName.Should().Be("FieldName");
        actual.Fields.First().Function.Should().NotBeNull();
        actual.Distinct.Should().BeTrue();
    }

    [Fact]
    public void Can_Use_SelectDistinct_With_FieldName_String_To_Add_Field()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.SelectDistinct("FieldName");

        // Assert
        actual.Fields.Should().HaveCount(1);
        var field = actual.Fields.First() as FieldExpressionBuilder;
        field?.FieldName.Should().Be("FieldName");
        actual.Fields.First().Function.Should().BeNull();
        actual.Distinct.Should().BeTrue();
    }

    [Fact]
    public void Can_Use_SelectDistinct_With_FieldName_Strings_To_Add_Multiple_Fields()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.SelectDistinct("FieldName1", "FieldName2");

        // Assert
        actual.Fields.Should().HaveCount(2);
        var firstField = actual.Fields.First() as FieldExpressionBuilder;
        firstField?.FieldName.Should().Be("FieldName1");
        actual.Fields.ElementAt(0).Function.Should().BeNull();
        var lastField = actual.Fields.Last() as FieldExpressionBuilder;
        lastField?.FieldName.Should().Be("FieldName2");
        actual.Fields.ElementAt(1).Function.Should().BeNull();
        actual.Distinct.Should().BeTrue();
    }

    [Fact]
    public void Can_Use_Distinct_To_Set_Distinct()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.Distinct();

        // Assert
        actual.Distinct.Should().BeTrue();
    }

    [Fact]
    public void Can_Use_GetAllFields_To_Set_GetAllFields()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.GetAllFields();

        // Assert
        actual.GetAllFields.Should().BeTrue();
    }
}

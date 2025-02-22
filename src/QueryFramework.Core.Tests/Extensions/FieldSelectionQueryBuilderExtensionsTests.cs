namespace QueryFramework.Core.Tests.Extensions;

public class FieldSelectionQueryBuilderExtensionsTests
{
    [Fact]
    public void Can_Use_Select_With_FieldName_String_To_Add_Field()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.Select("FieldName");

        // Assert
        actual.FieldNames.Count.ShouldBe(1);
        var field = actual.FieldNames[0];
        field.ShouldBe("FieldName");
        actual.Distinct.ShouldBeFalse();
    }

    [Fact]
    public void Can_Use_Select_With_FieldName_String_To_Add_Multiple_Fields()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.Select("FieldName1", "FieldName2");

        // Assert
        actual.FieldNames.Count.ShouldBe(2);
        var firstField = actual.FieldNames[0];
        firstField.ShouldBe("FieldName1");
        var lastField = actual.FieldNames[actual.FieldNames.Count - 1];
        lastField.ShouldBe("FieldName2");
        actual.Distinct.ShouldBeFalse();
    }

    [Fact]
    public void Can_Use_SelectAll_To_Clear_Fields_And_Set_GetAllFields_To_True()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder().Select("FieldName1", "FieldName2");

        // Act
        var actual = sut.SelectAll();

        // Assert
        actual.FieldNames.ShouldBeEmpty();
        actual.GetAllFields.ShouldBeTrue();
    }

    [Fact]
    public void Can_Use_SelectDistinct_With_FieldName_String_To_Add_Field()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.SelectDistinct("FieldName");

        // Assert
        actual.FieldNames.Count.ShouldBe(1);
        var field = actual.FieldNames[0];
        field.ShouldBe("FieldName");
        actual.Distinct.ShouldBeTrue();
    }

    [Fact]
    public void Can_Use_SelectDistinct_With_FieldName_Strings_To_Add_Multiple_Fields()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.SelectDistinct("FieldName1", "FieldName2");

        // Assert
        actual.FieldNames.Count.ShouldBe(2);
        var firstField = actual.FieldNames[0];
        firstField.ShouldBe("FieldName1");
        var lastField = actual.FieldNames[actual.FieldNames.Count - 1];
        lastField.ShouldBe("FieldName2");
        actual.Distinct.ShouldBeTrue();
    }

    [Fact]
    public void Can_Use_Distinct_To_Set_Distinct()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.Distinct();

        // Assert
        actual.Distinct.ShouldBeTrue();
    }

    [Fact]
    public void Can_Use_GetAllFields_To_Set_GetAllFields()
    {
        // Arrange
        var sut = new FieldSelectionQueryBuilder();

        // Act
        var actual = sut.GetAllFields();

        // Assert
        actual.GetAllFields.ShouldBeTrue();
    }
}

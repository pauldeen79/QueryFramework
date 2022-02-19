namespace ExpressionFramework.CodeGeneration.Tests;

public class ModelGenerationTests
{
    private static readonly CodeGenerationSettings Settings = new CodeGenerationSettings
    (
        basePath: Path.Combine(Directory.GetCurrentDirectory(), @"../../../../"),
        generateMultipleFiles: false,
        dryRun: true
    );

    [Fact]
    public void Can_Generate_Everything()
    {
        Verify(GenerateCode.For<AbstractionsBuildersInterfaces>(Settings));
        Verify(GenerateCode.For<AbstractionsExtensionsBuilders>(Settings));
        Verify(GenerateCode.For<CoreEntities>(Settings));
        Verify(GenerateCode.For<CoreBuilders>(Settings));
    }

    [Fact]
    public void FixTypeName_Returns_Correct_Result_For_NonGeneric_Type()
    {
        // Arrange
        var input = typeof(int).FullName;

        // Act
        var actual = input.FixTypeName();

        // Assert
        actual.Should().Be("System.Int32");
    }

    [Fact]
    public void FixTypeName_Returns_Correct_Result_For_Nullable_Type()
    {
        // Arrange
        var input = typeof(int?).FullName;

        // Act
        var actual = input.FixTypeName();

        // Assert
        actual.Should().Be("System.Nullable<System.Int32>");
    }

    [Fact]
    public void FixTypeName_Returns_Correct_Result_For_Generic_Func()
    {
        // Arrange
        var input = typeof(Func<int>).FullName;

        // Act
        var actual = input.FixTypeName();

        // Assert
        actual.Should().Be("System.Func<System.Int32>");
    }

    [Fact]
    public void FixTypeName_Returns_Correct_Result_For_Nullable_Generic_Func()
    {
        // Arrange
        var input = typeof(Func<int?>).FullName;

        // Act
        var actual = input.FixTypeName();

        // Assert
        actual.Should().Be("System.Func<System.Nullable<System.Int32>>");
    }

    [Fact]
    public void FixTypeName_Returns_Correct_Result_For_Generic_Enumerable()
    {
        // Arrange
        var input = typeof(IEnumerable<int>).FullName;

        // Act
        var actual = input.FixTypeName();

        // Assert
        actual.Should().Be("System.Collections.Generic.IEnumerable<System.Int32>");
    }

    [Fact]
    public void FixTypeName_Returns_Correct_Result_For_Nullable_Generic_Enumerable()
    {
        // Arrange
        var input = typeof(IEnumerable<int?>).FullName;

        // Act
        var actual = input.FixTypeName();

        // Assert
        actual.Should().Be("System.Collections.Generic.IEnumerable<System.Nullable<System.Int32>>");
    }

    [Fact]
    public void FixTypeName_Returns_Correct_Result_For_Generics_With_Multiple_Generic_Parameters()
    {
        // Arrange
        var input = typeof(Func<object?, IExpression, IExpressionEvaluator, object?>).FullName;

        // Act
        var actual = input.FixTypeName();

        // Assert
        //Note that nullable generic argument types are not recognized
        actual.Should().Be("System.Func<System.Object,ExpressionFramework.Abstractions.DomainModel.IExpression,ExpressionFramework.Abstractions.IExpressionEvaluator,System.Object>");
    }
    
    private void Verify(GenerateCode generatedCode)
    {
        if (Settings.DryRun)
        {
            var actual = generatedCode.GenerationEnvironment.ToString();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }
    }
}

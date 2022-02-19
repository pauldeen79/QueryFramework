namespace ExpressionFramework.Core.Tests.Default;

public class ValueProviderTests
{
    [Fact]
    public void GetValue_Returns_Null_When_Context_Is_Null()
    {
        // Arrange
        var sut = new ValueProvider();

        // Act
        var actual = sut.GetValue(null, string.Empty);

        // Assert
        actual.Should().BeNull();
    }

    [Fact]
    public void GetValue_Throws_When_Property_Is_Not_Found()
    {
        // Arrange
        var context = CreateOrder();
        var sut = new ValueProvider();

        // Act & Assert
        sut.Invoking(x => x.GetValue(context, "NonExistingProperty"))
           .Should().ThrowExactly<ArgumentOutOfRangeException>()
           .WithParameterName("fieldName")
           .And.Message.Should().StartWith("Fieldname [NonExistingProperty] is not found on type [ExpressionFramework.Core.Tests.Default.ValueProviderTests+Order]");
    }

    [Fact]
    public void GetValue_Returns_Null_When_Property_Value_Is_Null()
    {
        // Arrange
        var context = CreateOrder();
        var sut = new ValueProvider();

        // Act
        var actual = sut.GetValue(context, nameof(Order.EmptyField));

        // Assert
        actual.Should().BeNull();
    }

    [Fact]
    public void GetValue_Returns_NonNull_Value_When_Property_Value_Is_Not_Null()
    {
        // Arrange
        var context = CreateOrder();
        var sut = new ValueProvider();

        // Act
        var actual = sut.GetValue(context, nameof(Order.OrderNumber));

        // Assert
        actual.Should().Be(context.OrderNumber);
    }

    [Fact]
    public void GetValue_Returns_Nested_Property_Correctly()
    {
        // Arrange
        var context = CreateOrder();
        var sut = new ValueProvider();

        // Act
        var actual = sut.GetValue(context, $"{nameof(Order.OrderLines)}.{nameof(Order.OrderLines.Count)}");

        // Assert
        actual.Should().Be(context.OrderLines.Count);
    }

    [Fact]
    public void GetValue_Returns_Deeply_Nested_Property_Correctly()
    {
        // Arrange
        var context = CreateOrder();
        var sut = new ValueProvider();

        // Act
        var actual = sut.GetValue(context, $"{nameof(Order.DeliveryAddress)}.{nameof(Order.DeliveryAddress.ContactInformation)}.{nameof(Order.DeliveryAddress.ContactInformation.EmailAddress)}");

        // Assert
        actual.Should().Be(context.DeliveryAddress.ContactInformation.EmailAddress);
    }

    [Fact]
    public void GetValue_Handles_Null_Value_On_Nested_Property_Correctly()
    {
        // Arrange
        var context = CreateOrder();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        context.DeliveryAddress = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        var sut = new ValueProvider();

        // Act
        var actual = sut.GetValue(context, $"{nameof(Order.DeliveryAddress)}.{nameof(Order.DeliveryAddress.ContactInformation)}.{nameof(Order.DeliveryAddress.ContactInformation.EmailAddress)}");

        // Assert
        actual.Should().BeNull();
    }

    private static Order CreateOrder()
    {
        var order = new Order();
        order.OrderNumber = 12345;
        order.OrderLines.Add(new OrderLine { LineNumber = 10 });
        order.OrderLines.Add(new OrderLine { LineNumber = 20 });
        order.DeliveryAddress.Street = "Street";
        order.DeliveryAddress.HouseNumber = 1;
        order.DeliveryAddress.ContactInformation.EmailAddress = "test@test.com";
        return order;
    }

    public class Order
    {
        public int OrderNumber { get; set; }
        public int? EmptyField { get; set; }
        public List<OrderLine> OrderLines { get; } = new List<OrderLine>();
        public OrderAddress DeliveryAddress { get; set; } = new OrderAddress();
    }

    public class OrderLine
    {
        public int LineNumber { get; set; }
    }

    public class OrderAddress
    {
        public string Street { get; set; } = string.Empty;
        public int HouseNumber { get; set; }
        public ContactInformation ContactInformation { get; set; } = new ContactInformation();
    }

    public class ContactInformation
    {
        public string EmailAddress { get; set; } = string.Empty;
    }
}

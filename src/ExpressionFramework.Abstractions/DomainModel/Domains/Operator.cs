namespace ExpressionFramework.Abstractions.DomainModel.Domains;

public enum Operator
{
    Equal = 0,
    StartsWith = 1,
    EndsWith = 2,
    Contains = 3,
    Smaller = 4,
    SmallerOrEqual = 5,
    Greater = 6,
    GreaterOrEqual = 7,
    IsNull = 8,
    NotEqual = 9,
    NotStartsWith = 10,
    NotEndsWith = 11,
    NotContains = 12,
    IsNotNull = 13,
    IsNullOrEmpty = 14,
    IsNotNullOrEmpty = 15,
    IsNullOrWhiteSpace = 16,
    IsNotNullOrWhiteSpace = 17
}

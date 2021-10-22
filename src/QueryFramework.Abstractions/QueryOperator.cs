namespace QueryFramework.Abstractions
{
    /// <summary>
    /// All supported query operator types.
    /// </summary>
    public enum QueryOperator
    {
        /// <summary>
        /// Value should be equal to a value.
        /// </summary>
        Equal = 0,

        /// <summary>
        /// Value should start with a value.
        /// </summary>
        StartsWith = 1,

        /// <summary>
        /// Value should end with a value.
        /// </summary>
        EndsWith = 2,

        /// <summary>
        /// Value should contain a value.
        /// </summary>
        Contains = 3,

        /// <summary>
        /// Value should be lower than a value.
        /// </summary>
        Lower = 4,

        /// <summary>
        /// Value should be lower or equal than a value.
        /// </summary>
        LowerOrEqual = 5,

        /// <summary>
        /// Value should be greater than a value.
        /// </summary>
        Greater = 6,

        /// <summary>
        /// Value should be greater or equal than a value.
        /// </summary>
        GreaterOrEqual = 7,

        /// <summary>
        /// Value should be null.
        /// </summary>
        IsNull = 8,

        /// <summary>
        /// Value should not be equal to a value.
        /// </summary>
        NotEqual = 9,

        /// <summary>
        /// Value should not start with a value.
        /// </summary>
        NotStartsWith = 10,

        /// <summary>
        /// Value should not end with a value.
        /// </summary>
        NotEndsWith = 11,

        /// <summary>
        /// Value should not contain a value.
        /// </summary>
        NotContains = 12,

        /// <summary>
        /// Value should not be null.
        /// </summary>
        IsNotNull = 13,

        /// <summary>
        /// Value should be null or empty.
        /// </summary>
        IsNullOrEmpty = 14,

        /// <summary>
        /// Value should not be null or empty.
        /// </summary>
        IsNotNullOrEmpty = 15
    }
}

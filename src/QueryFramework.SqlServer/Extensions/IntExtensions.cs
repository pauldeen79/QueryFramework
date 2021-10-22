namespace QueryFramework.SqlServer.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Determines the record limit.
        /// </summary>
        /// <param name="queryLimit">The query limit.</param>
        /// <param name="overrideLimit">The override limit.</param>
        public static int DetermineLimit(this int? queryLimit, int? overrideLimit)
        {
            var result = 0;

            if (queryLimit.HasValue)
            {
                result = queryLimit.Value;
            }

            if (overrideLimit.HasValue && ((overrideLimit.Value < result && overrideLimit.Value >= 0) || result == 0))
            {
                result = overrideLimit.Value;
            }

            return result;
        }
    }
}

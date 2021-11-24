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

            if (queryLimit.HasValue && queryLimit.Value >= 0)
            {
                result = queryLimit.Value;
            }

            if (overrideLimit.HasValue && overrideLimit.Value > 0 && ((overrideLimit.Value < result && overrideLimit.Value >= 0) || result == 0))
            {
                result = overrideLimit.Value;
            }

            return result;
        }

        /// <summary>
        /// Determines the record offset.
        /// </summary>
        /// <param name="queryOffset">The query offset.</param>
        /// <param name="overrideOffset">The override offset.</param>
        public static int? DetermineOffset(this int? queryOffset, int? overrideOffset)
        {
            int? result = null;

            if (queryOffset.HasValue && queryOffset.Value > 0)
            {
                result = queryOffset.Value;
            }

            if (overrideOffset.HasValue && overrideOffset.Value > 0 && ((overrideOffset.Value < result.GetValueOrDefault() && overrideOffset.Value > 0) || result.GetValueOrDefault() == 0))
            {
                result = overrideOffset.Value;
            }

            return result;
        }
    }
}

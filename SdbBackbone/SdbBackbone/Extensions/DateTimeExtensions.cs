using System;
using System.Data.SqlTypes;

namespace SdbBackbone.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool NotNullDefaultOrMin(this DateTime? date)
        {
            if (date == null)
                return false;

            return ((DateTime) date).NotNullDefaultOrMin();
        }

        public static bool NotNullDefaultOrMin(this DateTime date)
        {
            if (date == (DateTime) SqlDateTime.MinValue)
                return false;

            if (date == DateTime.MinValue)
                return false;

            return date != new DateTime();
        }
    }
}
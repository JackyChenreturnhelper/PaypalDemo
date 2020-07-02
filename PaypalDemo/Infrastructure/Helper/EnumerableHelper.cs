using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Infrastructure.Helper
{
    public static class EnumerableHelper
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return (enumerable != null ? (enumerable.Any<T>() ? 1 : 0) : 0) == 0;
        }
    }
}
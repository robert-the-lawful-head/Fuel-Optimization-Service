using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.Core.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static string ToStringOrEmpty(this string value)
        {
            return value?.ToString() ?? string.Empty;
        }
    }
}

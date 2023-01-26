using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.Core.Utilities.Extensions
{
    public static class DoubleExtensions
    {
        public static string ToStringOrEmpty(this double? value)
        {
            return value?.ToString() ?? string.Empty;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.Core.Utilities.Extensions
{
    public static class IntExtensions
    {
        public static string ToStringOrEmpty(this int? value)
        {
            return value?.ToString() ?? string.Empty;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Entities;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.Core.Extensions
{
    public static class CustomSqlFunctions
    {
        public static IQueryable<DatabaseStringSplitResult> AsTable(this DbContext context, List<string> listOfStrings)
        {
            var commaDelimitedList = String.Join(",", listOfStrings == null ? new List<string>() : listOfStrings);
            var delimiter = ",";
            return context.Set<DatabaseStringSplitResult>()
                .FromSqlInterpolated($"SELECT Value FROM dbo.fn_Split({commaDelimitedList}, {delimiter})");
        }

        public static IQueryable<DatabaseStringSplitResult> AsTable(this DbContext context, List<int> listOfInts)
        {
            var commaDelimitedList = String.Join(",", listOfInts == null ? new List<string>() : listOfInts.Select(x => x.ToString()));
            var delimiter = ",";
            return context.Set<DatabaseStringSplitResult>()
                .FromSqlInterpolated($"SELECT Value FROM dbo.fn_Split({commaDelimitedList}, {delimiter})");
        }
    }
}
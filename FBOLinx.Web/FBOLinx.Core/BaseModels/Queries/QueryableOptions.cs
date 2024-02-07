using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.Core.BaseModels.Queries
{
    public class QueryableOptions<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, bool>> Predicate { get; set; }
        public int? MaxRecords { get; set; }
        public Expression<Func<TEntity, long>> OrderByExpression { get; set; }
        public Expression<Func<TEntity, long>> OrderByDescendingExpression { get; set; }
    }
}

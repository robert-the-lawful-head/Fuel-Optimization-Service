using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FBOLinx.Core.BaseModels.Specifications
{
    public abstract class Specification<TEntity> : ISpecification<TEntity>
    {
        protected Specification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<TEntity, bool>> Criteria { get; }
        public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();
        
        protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
    }

    public abstract class Specification<TEntity, TProjection> : Specification<TEntity>, ISpecification<TEntity, TProjection>
    {
        protected Specification(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TProjection>> projection) : base(criteria)
        {
            Projection = projection;
        }
        
        public Expression<Func<TEntity, TProjection>> Projection { get; }
    }
}

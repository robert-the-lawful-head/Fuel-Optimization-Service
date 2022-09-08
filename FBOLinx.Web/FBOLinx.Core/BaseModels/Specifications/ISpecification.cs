using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FBOLinx.Core.BaseModels.Specifications
{
    public interface ISpecification<TEntity>
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        List<Expression<Func<TEntity, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
    }

    public interface ISpecification<TEntity, TProjection>
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        List<Expression<Func<TEntity, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
        Expression<Func<TEntity, TProjection>> Projection { get; }
    }
}
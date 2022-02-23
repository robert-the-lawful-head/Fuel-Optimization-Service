using System;
using FBOLinx.Core.BaseModels.Entities;
using Mapster;

namespace FBOLinx.ServiceLayer.DTO
{
    public abstract class BaseEntityModelDTO<T> where T : class
    {
        public virtual T ConvertToEntity(T result = null)
        {        
            return this.Adapt<T>();
        }

        public virtual Dto CastFromEntity<Dto>(T item) where Dto : class
        {
            return item.Adapt<Dto>();
        }
    }
}

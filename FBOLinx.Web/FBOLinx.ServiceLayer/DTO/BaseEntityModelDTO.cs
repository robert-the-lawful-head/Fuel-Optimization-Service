using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FBOLinx.Core.BaseModels.Entities;

namespace FBOLinx.ServiceLayer.DTO
{
    public abstract class BaseEntityModelDTO<T> where T : BaseEntityModel
    {
        public virtual T ConvertToEntity(IMapper mapper, T result = null)
        {
            if (result == null)
                result = ((T)Activator.CreateInstance((typeof(T))));
            mapper.Map(this, result);
            
            return result;
        }

        public virtual void CastFromEntity(IMapper mapper, T item)
        {
            mapper.Map(item, this);
        }
    }
}

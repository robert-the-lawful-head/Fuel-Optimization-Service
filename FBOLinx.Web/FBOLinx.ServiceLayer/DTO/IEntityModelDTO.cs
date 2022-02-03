using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FBOLinx.Core.BaseModels.Entities;

namespace FBOLinx.ServiceLayer.DTO
{
    public interface IEntityModelDTO<T, TIDType> where T : BaseEntityModel
    {
        TIDType Oid { get; set; }
        T ConvertToEntity(IMapper mapper, T result = null);
        void CastFromEntity(IMapper mapper, T item);
    }
}

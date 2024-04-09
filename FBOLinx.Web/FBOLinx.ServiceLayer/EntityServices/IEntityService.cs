using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FBOLinx.Core.BaseModels.Entities;
using FBOLinx.Core.BaseModels.Specifications;
using FBOLinx.ServiceLayer.DTO;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IEntityService<T, TDTO, TIDType> where T : BaseEntityModel where TDTO : IEntityModelDTO<T, TIDType>
    {
        Task<TDTO> GetById(TIDType id);
        //Task<T> GetEntityById(int id);
        Task<TDTO> GetSingleBySpec(FBOLinx.Core.BaseModels.Specifications.ISpecification<T> spec);
        //Task<T> GetSingleEntityBySpec(ISpecification<T> spec);
        Task<List<TDTO>> GetListBySpec(ISpecification<T> spec);
        //Task<List<T>> GetEntityListBySpec(ISpecification<T> spec);
        Task<TDTO> Add(TDTO entityDTO);
        //Task<T> AddEntity(T entity);
        Task<TDTO> Update(TDTO entityDTO);
        //Task UpdateEntity(T entity);
        Task Delete(TDTO entityDTO);
        //Task DeleteEntity(T entity);
        Task Delete(TIDType id);
        Task BulkDeleteEntities(ISpecification<T> spec);
    }
}

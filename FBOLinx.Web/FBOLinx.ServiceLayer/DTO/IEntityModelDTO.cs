using FBOLinx.Core.BaseModels.Entities;

namespace FBOLinx.ServiceLayer.DTO
{
    public interface IEntityModelDTO<T, TIDType> where T : BaseEntityModel
    {
        TIDType Id { get; set; }
        T ConvertToEntity(T result = null);
        Dto CastFromEntity<Dto>(T item) where Dto : class;
    }
}

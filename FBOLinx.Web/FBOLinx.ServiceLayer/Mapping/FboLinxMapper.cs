using Mapster;

namespace FBOLinx.ServiceLayer.Mapping
{
    public static class FboLinxMapper
    {
        public static TDestination Map<TDestination>(this object source) where TDestination : class
        {
            return source.Adapt<TDestination>();
        }
    }
}

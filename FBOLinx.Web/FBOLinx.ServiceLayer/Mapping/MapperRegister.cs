using FBOLinx.DB.Models;
using Mapster;
using System.Reflection;

namespace FBOLinx.ServiceLayer.Mapping
{
    public class MapperRegister : ICodeGenerationRegister
    {
        public void Register(CodeGenerationConfig config)
        {
            config.AdaptTo("[name]Dto")
            .ForAllTypesInNamespace(Assembly.Load("FBOLinx.DB"), "FBOLinx.DB.Models")
             .ExcludeTypes(typeof(FBOLinxBaseEntityModel<>));

            config.GenerateMapper("[name]Mapper")
            .ForAllTypesInNamespace(Assembly.Load("FBOLinx.DB"), "FBOLinx.DB.Models")
            .ExcludeTypes(typeof(FBOLinxBaseEntityModel<>));
        }
    }
}

using FBOLinx.DB.Models;
using Mapster;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace FBOLinx.ServiceLayer.Mapping
{
    public class MapperRegister : ICodeGenerationRegister
    {
        public void Register(CodeGenerationConfig config)
        {
            TypeAdapterConfig.GlobalSettings.Default
                .IncludeAttribute(typeof(StringLengthAttribute));

            // configuration to create automatic Dto files
            config.AdaptTo("[name]Dto")
            .ForAllTypesInNamespace(Assembly.Load("FBOLinx.DB"), "FBOLinx.DB.Models")
             .IgnoreAttributes(typeof(NotMappedAttribute))
             .ExcludeTypes(typeof(FBOLinxBaseEntityModel<>));


            config.AdaptTo("[name]Dto")
                .ForType<CustomerInfoByGroup>(cfg => {
                    cfg.Ignore(poco => poco.Customer);
                });

            config.AdaptTo("[name]Dto")
               .ForType<Group>(cfg => {
                   cfg.Ignore(poco => poco.Fbos);
                   cfg.Ignore(poco => poco.Users);
                   cfg.Ignore(poco => poco.ContactInfoByGroup);
               });

        // configuration to create automatic mapping extension files
        //config.GenerateMapper("[name]Mapper")
        //.ForAllTypesInNamespace(Assembly.Load("FBOLinx.DB"), "FBOLinx.DB.Models")
        //.ExcludeTypes(typeof(FBOLinxBaseEntityModel<>));
    }
    }
}

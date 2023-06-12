using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBOLinx.Core.Extensions;
using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using FBOLinx.ServiceLayer.DTO;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IAircraftHexTailMappingEntityService : IRepository<AircraftHexTailMapping, DegaContext>
    {
        Task<List<AircraftHexTailMappingDTO>> GetAircraftHexTailMappingsForTails(List<string> tailNumbers);
    }

    public class AircraftHexTailMappingEntityService : Repository<AircraftHexTailMapping, DegaContext>, IAircraftHexTailMappingEntityService
    {
        public AircraftHexTailMappingEntityService(DegaContext context) : base(context)
        {
        }

        public async Task<List<AircraftHexTailMappingDTO>> GetAircraftHexTailMappingsForTails(List<string> tailNumbers)
        {
            var result = await (from hexTailMapping in context.AircraftHexTailMapping
                join tail in context.AsTable(tailNumbers) on hexTailMapping.TailNumber equals tail.Value
                    join faaMakeModel in context.FAAAircraftMakeModelReference on hexTailMapping.FAAAircraftMakeModelCode equals faaMakeModel.CODE
                    into faaMakeModelJoin
                    from faaMakeModel in faaMakeModelJoin.DefaultIfEmpty()
                                select new AircraftHexTailMappingDTO()
                                {
                                    Oid = hexTailMapping.Oid,
                                    AircraftHexCode = hexTailMapping.AircraftHexCode,
                                    TailNumber = hexTailMapping.TailNumber,
                                    FAAAircraftMakeModelCode = hexTailMapping.FAAAircraftMakeModelCode,
                                    FAARegisteredOwner = hexTailMapping.FAARegisteredOwner,
                                    FaaAircraftMakeModelReference = faaMakeModel == null ? null : new FaaAircraftMakeModelReferenceDto()
                                    {
                                        CODE = faaMakeModel.CODE,
                                        MFR = faaMakeModel.MFR,
                                        MODEL = faaMakeModel.MODEL,
                                        Oid = faaMakeModel.Oid,
                                        DegaAircraftID = faaMakeModel.DegaAircraftID
                                    }

                                }).ToListAsync();
            return result;
        }
    }
}

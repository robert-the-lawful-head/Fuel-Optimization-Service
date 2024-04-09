using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO
{
    public class FboContactsDTO : BaseEntityModelDTO<DB.Models.Fbocontacts>, IEntityModelDTO<DB.Models.Fbocontacts, int>
    {
        public int Oid { get; set; }
        public int Fboid { get; set; }
        public int ContactId { get; set; }
    }
}

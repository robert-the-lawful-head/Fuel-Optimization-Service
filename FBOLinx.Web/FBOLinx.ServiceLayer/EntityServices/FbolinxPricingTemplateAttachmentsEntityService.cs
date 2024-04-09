using FBOLinx.DB.Context;
using FBOLinx.DB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace FBOLinx.ServiceLayer.EntityServices
{
    public interface IFbolinxPricingTemplateAttachmentsEntityService : IRepository<FbolinxPricingTemplateFileAttachment, FilestorageContext>
    {
    }
    public class FbolinxPricingTemplateAttachmentsEntityService : Repository<FbolinxPricingTemplateFileAttachment, FilestorageContext>, IFbolinxPricingTemplateAttachmentsEntityService
    {
        private readonly FilestorageContext _context;
        public FbolinxPricingTemplateAttachmentsEntityService(FilestorageContext context) : base(context)
        {
            _context = context;
        }
    }
}

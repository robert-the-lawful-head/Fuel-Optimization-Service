using FBOLinx.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBOLinx.ServiceLayer.DTO
{
    public class DocumentsToAcceptDto
    {
        public int UserId { get; set; }
        public bool hasPendingDocumentsToAccept { get;set; }
        public PolicyAndAgreementDocuments? DocumentToAccept { get; set; }
    }
}

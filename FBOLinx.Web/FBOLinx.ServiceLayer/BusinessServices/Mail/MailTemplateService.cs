using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace FBOLinx.ServiceLayer.BusinessServices.Mail
{
    public class MailTemplateService: IMailTemplateService
    {
        private IFileProvider _fileProvider;

        public MailTemplateService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public string GetTemplatesFileContent(string folder, string fileName)
        {
            string filePath = "Templates/" + folder + "/" + fileName;
            var fileInfo = _fileProvider.GetFileInfo(filePath);
            if (System.IO.File.Exists(fileInfo.PhysicalPath))
                return System.IO.File.ReadAllText(fileInfo.PhysicalPath);
            return System.IO.File.ReadAllText(fileInfo.PhysicalPath.Replace("Templates", "wwwroot/Templates"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace FBOLinx.Web.Utilities
{
    public class FileLocater
    {
        public static string GetTemplatesFileContent(IFileProvider fileProvider, string folder, string fileName)
        {
            string filePath = "Templates/" + folder +  "/" + fileName;
            var fileInfo = fileProvider.GetFileInfo(filePath);
            if (System.IO.File.Exists(fileInfo.PhysicalPath))
                return System.IO.File.ReadAllText(fileInfo.PhysicalPath);
            return System.IO.File.ReadAllText(fileInfo.PhysicalPath.Replace("Templates", "wwwroot/Templates"));
        }
    }
}

namespace FBOLinx.ServiceLayer.BusinessServices.Mail
{
    public interface IMailTemplateService
    {
        string GetTemplatesFileContent(string folder, string fileName);
    }
}
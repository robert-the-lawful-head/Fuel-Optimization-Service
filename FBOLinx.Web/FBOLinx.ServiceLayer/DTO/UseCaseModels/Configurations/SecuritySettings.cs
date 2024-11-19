namespace FBOLinx.ServiceLayer.DTO.UseCaseModels.Configurations
{
    public class SecuritySettings
    {
        public int TokenExpirationInMinutes { get; set; }
        public string TokenKey { get; set; }
        public string RefreshTokenKey { get; set; }
        public int RefreshTokenExpirationInMinutes { get; set; }
    }
}

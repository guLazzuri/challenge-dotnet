namespace challenge.Infrastructure.Config
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = "MyUltraStrongSecretKey_2025_GefDotnetAPI_!@#1234567890";
        public string Issuer { get; set; } = "GefDotnetAPI";
        public string Audience { get; set; } = "GefDotnetAPIClient";
        public int ExpirationMinutes { get; set; } = 60;
    }
}

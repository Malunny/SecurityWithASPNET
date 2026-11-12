namespace Domain;

public static class Configuration
{
    public static SecretsConfiguration Secrets { get; } = new();
    public static ConnectionStringsConfiguration ConnectionStrings { get; } = new();
    public class SecretsConfiguration
    {
        public string PasswordSaltKey { get; set; } = string.Empty;
        public string JwtToken { get; set; } = string.Empty;
    }
    
    public class ConnectionStringsConfiguration
    {
        public string DefaultConnection { get; set; } = string.Empty;
    }
}
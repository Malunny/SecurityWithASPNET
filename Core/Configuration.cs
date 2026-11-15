namespace Domain;

public static class Configuration
{
    public static SecretsConfiguration Secrets { get; } = new();
    public static ConnectionStringsConfiguration ConnectionStrings { get; } = new();
    public static EmailSenderConfiguration EmailConfiguration { get; } = new();
    public class SecretsConfiguration
    {
        public string PasswordSaltKey { get; set; } = string.Empty;
        public string JwtToken { get; set; } = string.Empty;
        public string MailtrapToken { get; set; } = string.Empty;
    }

    public class EmailSenderConfiguration
    {
        public string Sender { get; set; }
    }
    
    public class ConnectionStringsConfiguration
    {
        public string DefaultConnection { get; set; } = string.Empty;
    }
}
using Flunt.Notifications;

namespace Domain.Contexts.Account.UseCases.Create;

public class Response : Shared.UseCases.Response
{
    public ResponseData? Data { get; }

    protected Response() { }

    public Response(ResponseData data)
        : base("Created", 201)
    {
        Data = data;
    }

    public Response(string message, int status, IEnumerable<Notification>? notifications = null) : base(message, status, notifications)
    {
        
    }
}

public record ResponseData(string Token, string Email, string Name);
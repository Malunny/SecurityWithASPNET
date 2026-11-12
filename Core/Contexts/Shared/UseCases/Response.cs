using Flunt.Notifications;

namespace Domain.Contexts.Shared.UseCases;

public abstract class Response
{
    public string Message { get; set; }
    public int Status { get; set; } = 200;
    public bool IsSuccess => Status >= 200 & Status <= 299;
    public IEnumerable<Notification>? Notifications { get; set; }

    public Response() { }

    public Response(string message, int status, IEnumerable<Notification>? notifications = null)
    {
        this.Message = message;
        this.Status = status;
        this.Notifications = notifications;
    }
}
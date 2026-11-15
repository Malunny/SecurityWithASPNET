using Domain;
using Domain.Contexts.Account.Entities.Users;
using Domain.Contexts.Account.UseCases.Create.Contracts;
using Mailtrap;
using Mailtrap.Core.Exceptions;
using Mailtrap.Emails.Models;
using Mailtrap.Emails.Requests;
using Mailtrap.Emails.Responses;
using Microsoft.Extensions.Configuration;

namespace Infraestructure.Contexts.Account.UseCases.Create;

public class Service : IService
{
    private readonly IMailtrapClient _emailClient;
    public Service(IMailtrapClient emailClient)
    {
        _emailClient = emailClient;
    }
    public async Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken)
    {
        var request = SendEmailRequest.Create()
            .From(Configuration.EmailConfiguration.Sender)
            .To(user.Email)
            .Subject("Email Verification")
            .Category("Email Verification")
            .Text($"{user.Name} has been verified");

        await _emailClient.Email().Send(request);
    }
}
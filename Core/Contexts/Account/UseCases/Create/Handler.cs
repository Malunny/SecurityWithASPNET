using Domain.Contexts.Account.Entities.Users;
using Domain.Contexts.Account.UseCases.Create.Contracts;
using Domain.Contexts.Account.ValueObjects;

namespace Domain.Contexts.Account.UseCases.Create;

public class Handler
{
    private readonly IService _service;
    private readonly IRepository _repository;

    public Handler(IRepository repository, IService service)
    {
        _repository = repository;
        _service = service;
    }
    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        #region 1. Validação
        try
        {
            var contract = Specification.Ensure(request);
            
            if (!contract.IsValid)
                return new Response("Invalid Request", 400, contract.Notifications);
        }
        catch (Exception ex)
        {
            return new Response("Invalid Request", 400, null);
        }
        #endregion

        #region 2. Criação de objetos

        User user;
        Email email;
        Password password;
        
        try
        {
            email = new Email(request.Email);
            password = new Password(request.Password);
            
            user = new User(request.Name, email, password);
        }
        catch (Exception ex)
        {
            return new Response(ex.Message, 404);
        }
        #endregion

        #region 3. Verificação de usuário duplicado

        var userAlreadyExists = await _repository.AnyAsync(request.Email, cancellationToken);

        #endregion

        #region 4. Persistir dados

        try
        {
            await _repository.SaveAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            return new Response("A problem ocurred when saving the user.", 500);
        }

        #endregion

        #region 5. Envio e-mail ativação

        try
        {
            await _service.SendVerificationEmailAsync(user, cancellationToken);
        }
        catch
        {
            
        }

        #endregion

        return new Response(new ResponseData(user.Token, user.Email, user.Name));
    }
}
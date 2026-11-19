using System.Security.Claims;
using Domain.Contexts.Account.Entities.Users;
using Domain.Contexts.Account.UseCases.Authentication.Contracts;
using MediatR;

namespace Domain.Contexts.Account.UseCases.Authentication;

public class Handler : IRequestHandler<Request, Response>
{
    private readonly IRepository _repository;

    public Handler(IRepository repository) => _repository = repository;

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        #region 01. Valida a requisição

        try
        {
            var requestValidation = Specification.Ensure(request);
            if (!requestValidation.IsValid)
                return new Response("Requisição inválida", 400, requestValidation.Notifications);
        }
        catch
        {
            return new Response("Não foi possível validar sua requisição", 500);
        }

        #endregion

        #region 02. Recupera o perfil

        User? user;
        try
        {
            user = await _repository.GetUserByEmailAsync(request.Email, cancellationToken);
            if (user is null)
                return new Response("Perfil não encontrado", 404);
        }
        catch (Exception e)
        {
            return new Response("Não foi possível recuperar seu perfil", 500);
        }

        #endregion

        #region 03. Checa se a senha é válida

        if (!user.Password.Equals(request.Password))
            return new Response("Usuário ou senha inválidos", 400);

        #endregion

        #region 04. Checa se a conta está verificada

        try
        {
            if (!user.Email.Verification.IsActive)
                return new Response("Conta inativa", 400);
        }
        catch
        {
            return new Response("Não foi possível verificar seu perfil", 500);
        }

        #endregion
        
        #region 05. Gera a resposta e o Jwt

        var claims = new List<Claim>();
        
        claims.Add(new Claim("id", user.Token));
        claims.Add(new Claim(ClaimTypes.Name, user.Email));
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        foreach (var role in user.GetUserRolesNames())
            claims.Add(new Claim(ClaimTypes.Role, role));
        
        var jwtToken = JwtExtensions.GenerateToken(claims);
        
        try
        {
            var data = new ResponseData
            {
                Token = jwtToken,
                Id = user.Token,
                Name = user.Name,
                Email = user.Email,
                Roles = user.Roles.Select(x => x.Name).ToArray()
            };

            return new Response(string.Empty, data);
        }
        catch
        {
            return new Response("Não foi possível obter os dados do perfil", 500);
        }

        #endregion
    }
}
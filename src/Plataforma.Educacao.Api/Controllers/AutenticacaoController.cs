using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Plataforma.Educacao.Api.Settings;
using Plataforma.Educacao.Api.ViewModels.Autenticacao;
using Plataforma.Educacao.Autenticacao.Data.Contexts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Plataforma.Educacao.Api.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AutenticacaoController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    //private readonly IUserRepository _userRepository;
    private readonly AppSettings _appSettings;
    private readonly ILogger _logger;
    private readonly AutenticacaoDbContext _identityContext;

    public AutenticacaoController(ILogger<AutenticacaoController> logger,
                          SignInManager<IdentityUser> signInManager,
                          UserManager<IdentityUser> userManager,
                          IOptions<AppSettings> appSettings,
                          //IUserRepository userRepository,
                          //IAppIdentityUser appIdentityUser,
                          AutenticacaoDbContext identityContext)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        //_userRepository = userRepository;
        _appSettings = appSettings.Value;
        _identityContext = identityContext;
    }


    [HttpPost("registro")]
    [ProducesResponseType(typeof(AutenticacaoOutputViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RegistroAsync(RegistroViewModel registroViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        var identitiyUser = new IdentityUser
        {
            UserName = registroViewModel.Email,
            Email = registroViewModel.Email,
            EmailConfirmed = true
        };

        var registerResult = await _userManager.CreateAsync(identitiyUser, registroViewModel.Password);

        if (registerResult.Succeeded)
        {
            try
            {
                #region Roles
                IdentityRole role = _identityContext.Roles.FirstOrDefault(x => x.Name == "USER");
                if (role != null)
                {
                    _identityContext.UserRoles.Add(new IdentityUserRole<string>()
                    {
                        RoleId = role.Id,
                        UserId = identitiyUser.Id
                    });

                    await _identityContext.SaveChangesAsync();
                }
                #endregion

                #region User
                //var user = new User
                //{
                //    UserId = Guid.Parse(identitiyUser.Id),
                //    Email = registerViewModel.Email,
                //    Name = registerViewModel.Name,
                //    AuthenticationId = Guid.Parse(identitiyUser.Id)
                //};

                //await _userRepository.CreateAsync(user);

                var loginOutput = new AutenticacaoOutputViewModel
                {
                    Id = Guid.Parse(identitiyUser.Id),
                    Nome = registroViewModel.Nome,
                    Email = registroViewModel.Email,
                    AccessToken = await GenerateJwt(registroViewModel.Email)
                };
                #endregion

                return GenerateResponse(loginOutput);
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(identitiyUser);

                _logger.LogError(ex, $"Erro ao tentar criar o usuário na base de dados: {ex.Message}");
                //Notify("Não foi possível criar o usuário, tente novamente mais tarde.");
            }
        }
        else
        {
            foreach (var error in registerResult.Errors)
            {
                //Notify(error.Description);
            }
        }

        return GenerateResponse();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AutenticacaoOutputViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> LoginAsync(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid) return GenerateResponse(ModelState);

        var loginResult = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, true);

        if (loginResult.Succeeded)
        {
            //var user = await _userRepository.GetByEmailAsync(loginViewModel.Email);

            var loginOutput = new AutenticacaoOutputViewModel
            {
                //Id = loginResult.,
                //Name = user.Name,
                //Email = user.Email,
                AccessToken = await GenerateJwt(loginViewModel.Email)
            };

            return GenerateResponse(loginOutput);
        }
        else
        {
            //Notify("Usuário não localizado com as credenciais informadas");
        }

        return GenerateResponse();
    }

    private async Task<string> GenerateJwt(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id),
            new (ClaimTypes.Name, user.UserName),
            new (ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _appSettings.JwtSettings.Issuer,
            Audience = _appSettings.JwtSettings.Audience,
            Expires = DateTime.UtcNow.AddHours(_appSettings.JwtSettings.ExpirationInHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        var encodedToken = tokenHandler.WriteToken(token);

        return encodedToken;
    }
}

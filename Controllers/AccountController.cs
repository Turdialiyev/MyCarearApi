# pragma warning disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Entities;
using MyCarearApi.Models.Account;
using MyCarearApi.Services.JwtServices;
using MyCarearApi.Services.JwtServices.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;
using MyCarearApi.Dtos.Account;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json.Linq;

namespace MyCarearApi.Controllers;

public class AccountController: ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly IPasswordValidator<AppUser> _passwordValidator;
    private readonly IUserValidator<AppUser> _userValidator;
    private readonly IServiceProvider serviceProvider;
    private readonly IJwtService _jwtService;
    private readonly IMailSender _mailSender;
    private readonly IConfiguration _configuration;


    ILogger<AccountController> _logger;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager, ILogger<AccountController> logger,
        IPasswordValidator<AppUser> passwordValidator, IUserValidator<AppUser> userValidator, IJwtService jwtService, 
        IServiceProvider serviceProvider, IMailSender mailSender, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
        _passwordValidator = passwordValidator;
        _userValidator = userValidator;
        _regex = new Regex(pattern);
        _jwtService = jwtService;
        this.serviceProvider = serviceProvider;
        _mailSender = mailSender;
        _configuration = configuration;
    }

    private string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

    private Regex _regex;

    [HttpPost("register")]
    [Authorize]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody]UserModel userModel)
    {
        var newUser = new AppUser { UserName = userModel.Email, Email = userModel.Email };
        Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        IdentityResult emailValidationResult = await _userValidator.ValidateAsync(_userManager, newUser);
        
        //Email validation
        errors.Add("EmailError", new List<string>());
        if (!emailValidationResult.Succeeded ) 
            errors["EmailError"].AddRange(emailValidationResult.Errors.Select(x => x.Description));


        if (!_regex.IsMatch(userModel.Email))
            errors["EmailError"].Add("You must enter valid Email");


        //Password Validation
        IdentityResult passwordValidationResult = await _passwordValidator.ValidateAsync(_userManager, newUser, userModel.Password);
        errors.Add("PasswordError", new List<string>());
        if(!passwordValidationResult.Succeeded)
            errors["PasswordError"].AddRange(passwordValidationResult.Errors.Select(x => x.Description));

        //Password Confirmation
        errors.Add("PasswordConfirmError", new List<string>());
        if(userModel.Password != userModel.ConfirmPassword)
            errors["PasswordConfirmError"].Add("Password not confirmed");

        if (!isSuccess(errors))
            return Ok(new { Succeded = isSuccess(errors), Errors = errors });
        //Signing Up 
        errors.Add("OtherError", new List<string>());
        var signUpResult = await _userManager.CreateAsync(newUser, userModel.Password);
        if (!signUpResult.Succeeded)
            errors["OtherError"].AddRange(signUpResult.Errors.Select(x => x.Description));

        var result = new { Succeded = isSuccess(errors), Errors = errors };        

        if(result.Succeded) return Ok(result);

        return BadRequest( result );
    }

    private bool isSuccess(Dictionary<string, List<string>> errors)
    {
        return (!errors.ContainsKey("EmailError") || !errors["EmailError"].Any())
            && (!errors.ContainsKey("PasswordError") || !errors["PasswordError"].Any())
            && (!errors.ContainsKey("PasswordConfirmError") || !errors["PasswordConfirmError"].Any())
            && (!errors.ContainsKey("OtherError") || !errors["OtherError"].Any());
    }


    [HttpGet("emailconfirm/{email}")]
    public async Task<IActionResult> EmailConfirm(string email)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(await _userManager.FindByEmailAsync(email));

        _mailSender.Send(email, "Email Confirmation Message", @$"
<html>
<head> 
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
</head>
<body>
    <h3>Your confirmation message</h3>
    <p>Visit through this link to confirm your email: </p>
    <form method=""post"" action=""{_configuration.GetSection("Urls")["ConfirmUrl"]}"">
        <input type=""hidden"" value=""{token}"" name=""token""/>
        <input type=""hidden"" value=""{email}"" name=""email""/>
        <button type=""submit"" style=""background-color:#0669B4; color: white; padding: 30px; margin: 50px; width:100px; height: 30px"">
            Confirm
        </button>
    </form>
    <h5 stype=""padding: 10px;"">{token}</h5>
</body>
</html>
        ");

        return Ok(new { Message = "Confirmation code has sent" });
    }

    [HttpPost("emailconfirm")]
    public async Task<IActionResult> EmailConfirm([FromForm]string token, [FromForm] string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return BadRequest(new
        {
            Succeded = false,
            Error="User Not Found"
        });
        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded ? Ok(new { Succeded = true }) 
            : BadRequest(new { Succeded = false, Error = result.Errors.Select(x => x.Description) });
    }

    [HttpPost("ExternalLogin")]
    public async Task<IActionResult> ExternalLogin(ExternalAuthDto externalAuth)
    {
        var payload = await _jwtService.VerifyGoogleToken(externalAuth.Provider, externalAuth.IdToken);
        if (payload is null)
            return BadRequest(new
            {
                Succeded = false
            });
        var info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);

        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        if(user is null)
        {
            user = await _userManager.FindByEmailAsync(payload.Email);
            if(user is null)
            {
                user = new AppUser { Email = payload.Email, UserName = payload.Email };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded) return BadRequest(new
                {
                    Succeeded = false,
                    Errors = new List<string>(result.Errors.Select(x => x.Description)) { "Invalid External Auth" }
                });
                var token = _userManager.GenerateEmailConfirmationTokenAsync(await _userManager.FindByEmailAsync(user.Email));
                _mailSender.Send(user.Email, "Email Confirmation Message", @$"
<html>
<head> 
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
</head>
<body>
    <h3>Your confirmation message</h3>
    <p>Visit through this link to confirm your email: </p>
    <form method=""post"" action=""{_configuration.GetSection("Urls")["ConfirmUrl"]}"">
        <input type=""hidden"" value=""{token}"" name=""token""/>
        <input type=""hidden"" value=""{user.Email}"" name=""email""/>
        <button type=""submit"" style=""background-color:#0669B4; color: white; padding: 30px; margin: 50px; width:100px; height: 30px"">
            Confirm
        </button>
    </form>
    <h5 stype=""padding: 10px;"">{token}</h5>
</body>
</html>
        ");

                await _userManager.AddLoginAsync(user, info);
            }
            else
            {
                await _userManager.AddLoginAsync(user, info);
            }
        }

        var jwtToken = await _jwtService.GenerateToken(await _userManager.FindByEmailAsync(user.Email));
        return Ok(new { Succeeded = true, Token = jwtToken });

    }

    [HttpPost("addtocompany")]
    [Authorize]
    public async Task<IActionResult> AddToCompany() => await AddToRole(StaticRoles.Company);

    [HttpPost("addtofreelancer")]
    [Authorize]
    public async Task<IActionResult> AddToFreelancer() => await AddToRole(StaticRoles.Freelancer);

    private async Task<IActionResult> AddToRole(string role)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        if (await _userManager.IsInRoleAsync(user, StaticRoles.Company)) return BadRequest(new
        {
            Succeded = false,
            CurrentRole = StaticRoles.Company,
            Errors = new string[] { }
        });
        if (await _userManager.IsInRoleAsync(user, StaticRoles.Freelancer)) return BadRequest(new
        {
            Succeded = false,
            CurrentRole = StaticRoles.Freelancer,
            Errors = new string[] { }
        });

        var result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded) return BadRequest(new
        {
            Succeded = false,
            CurrentRole = "",
            Errors = result.Errors.Select(x => x.Description)
        });

        return Ok(new
        {
            Succeded = true,//08269523
            CurrentRole = role
        });//
    }
    


    [HttpPost("login")]
    [Authorize]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody]UserModel userModel)
    {
        Console.WriteLine(JsonSerializer.Serialize(userModel));

        var errorResult = BadRequest(new
        {
            Succeded = false,
            Description = "Email or password incorrect"
        });

        if (!_regex.IsMatch(userModel.Email)) return errorResult;

        if( string.IsNullOrEmpty(userModel.Password) || userModel.Password.Any(x => char.IsWhiteSpace(x))) return errorResult;

        await _signInManager.SignOutAsync();

        AppUser user = await _userManager.FindByEmailAsync(userModel.Email);

        if(user == null) return errorResult;

        var signInResult = await _signInManager.PasswordSignInAsync(user, userModel.Password, true, false);

        if ( !signInResult.Succeeded)
        {
            return errorResult;
        }

        var anonymousResult = new
        {
            Succeded = true,
            Token = await _jwtService.GenerateToken(user)
        };
        Console.WriteLine(JsonSerializer.Serialize(anonymousResult));
        return Ok(anonymousResult);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet("CLAIMS")]
    public async Task<IActionResult> Claims()
    {
        await SeedData.SeedUserData(serviceProvider);

        return Ok();
    }
}

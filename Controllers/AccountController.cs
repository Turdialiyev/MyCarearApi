using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Entities;
using MyCarearApi.Models.Account;
using MyCarearApi.Services.JwtServices;
using System.Text.RegularExpressions;

namespace MyCarearApi.Controllers;

public class AccountController: ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly IPasswordValidator<AppUser> _passwordValidator;
    private readonly IUserValidator<AppUser> _userValidator;
    private readonly JwtService _jwtService;


    ILogger<AccountController> _logger;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager, ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    private string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]UserModel userModel)
    {
        var newUser = new AppUser { UserName = userModel.Email, Email = userModel.Email };

        IdentityResult emailValidationResult = await _userValidator.ValidateAsync(_userManager, newUser);

        if(!emailValidationResult.Succeeded) 
        {
            return Ok(new
            {
                EmailError = true,
                Errors = emailValidationResult.Errors.Select(x => x.Description)
            });
        }
        IdentityResult passwordValidationResult = await _passwordValidator.ValidateAsync(_userManager, newUser, userModel.Password);
        if(!passwordValidationResult.Succeeded)
        {
            return Ok(new
            {
                PasswordError = true,
                Errors = passwordValidationResult.Errors.Select(x => x.Description)
            });
        }

        if(userModel.Password != userModel.ConfirmPassword)
        {
            return Ok(new
            {
                PasswordConfirm = true,
                Errors = new List<string>() { "Password not confirmed" }
            });
        }

        var signUpResult = await _userManager.CreateAsync(newUser, userModel.Password);
        if(!signUpResult.Succeeded) 
        {
            return Ok(new
            {
                OtherError = true,
                Errors = signUpResult.Errors.Select(x => x.Description)
            });
        }

        

        return Ok( new { Succeded = true } );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]UserModel userModel)
    {
        Regex regex = new Regex(pattern);
        var errorResult = Ok(new
        {
            ErrorOccured = true,
            Description = "Email or password incorrect"
        });

        if (!regex.IsMatch(userModel.Email)) return errorResult;

        if(userModel.Password == null || userModel.Password.Any(x => char.IsWhiteSpace(x))) return errorResult;

        await _signInManager.SignOutAsync();

        AppUser user = await _userManager.FindByEmailAsync(userModel.Email);

        if(user == null) return errorResult;

        var signInResult = await _signInManager.PasswordSignInAsync(user, userModel.Password, true, false);

        if ( !signInResult.Succeeded)
        {
            return errorResult;
        }

        return Ok(new
        {
            ErrorOccured = false,
            Token = _jwtService.GenerateToken(user)
        });        
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout() 
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}

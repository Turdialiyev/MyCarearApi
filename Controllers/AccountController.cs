using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Entities;
using MyCarearApi.Models.Account;
using MyCarearApi.Services.JwtServices;
using MyCarearApi.Services.JwtServices.Interfaces;
using System.Text.RegularExpressions;

namespace MyCarearApi.Controllers;

public class AccountController: ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly IPasswordValidator<AppUser> _passwordValidator;
    private readonly IUserValidator<AppUser> _userValidator;
    
    private readonly IJwtService _jwtService;


    ILogger<AccountController> _logger;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager, ILogger<AccountController> logger,
        IPasswordValidator<AppUser> passwordValidator, IUserValidator<AppUser> userValidator, IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
        _passwordValidator = passwordValidator;
        _userValidator = userValidator;
        _regex = new Regex(pattern);
        _jwtService = jwtService;
    }

    private string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

    private Regex _regex;

    [HttpPost("register")]
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

        return Ok( result );
    }

    private bool isSuccess(Dictionary<string, List<string>> errors)
    {
        return (!errors.ContainsKey("EmailError") || !errors["EmailError"].Any())
            && (!errors.ContainsKey("PasswordError") || !errors["PasswordError"].Any())
            && (!errors.ContainsKey("PasswordConfirmError") || !errors["PasswordConfirmError"].Any())
            && (!errors.ContainsKey("OtherError") || !errors["OtherError"].Any());
    }

    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]UserModel userModel)
    {
        var errorResult = Ok(new
        {
            ErrorOccured = true,
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

        return Ok(new
        {
            ErrorOccured = false,
            Token = await _jwtService.GenerateToken(user)
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Entities;
using MyCarearApi.Models.Account;

namespace MyCarearApi.Controllers;

public class AccountController: ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly IPasswordValidator<AppUser> _passwordValidator;
    private readonly IUserValidator<AppUser> _userValidator;


    ILogger<AccountController> _logger;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager, ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
    }

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
    public Task<IActionResult> Login([FromBody]UserModel userModel)
    {

    }
}

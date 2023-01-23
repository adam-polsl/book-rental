using BlazorStudiesProject.Server.Entities;
using BlazorStudiesProject.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorStudiesProject.Server.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthorizeController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthorizeController(UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginParameters loginParameters)
    {
        var user = await _userManager.FindByNameAsync(loginParameters.UserName);

        if (user == null)
        {
            return BadRequest("User does not exist");
        }

        var singInResult = await _signInManager.CheckPasswordSignInAsync(user, loginParameters.Password, false);
        if (!singInResult.Succeeded)
        {
            return BadRequest("Invalid password");
        }

        await _signInManager.SignInAsync(user, loginParameters.RememberMe);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterParameters parameters)
    {
        var user = new ApplicationUser
        {
            UserName = parameters.UserName,
        };

        var result = await _userManager.CreateAsync(user, parameters.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        }

        await _userManager.AddToRoleAsync(user, Strings.BasicUserRole);

        return await Login(new LoginParameters
        {
            UserName = parameters.UserName,
            Password = parameters.Password
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<UserAuthInfo> UserInfo()
    {
        return await BuildUserInfo();
    }

    [HttpGet]
    public async Task GoogleSignIn() //GoogleSignIn https://localhost:7278/api/Authorize/GoogleSignIn
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = "/login" });
    }

    private async Task<UserAuthInfo> BuildUserInfo()
    {
        var claims = User.Claims.Where(c => c.Type != ClaimTypes.Role).ToDictionary(c => c.Type, c => c.Value);
        var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
        //var isAuthenticated = User.Identity.IsAuthenticated;

        if ((User.Identity).AuthenticationType == "Google")
        {
            var userName = claims.FirstOrDefault(x => x.Key == ClaimTypes.Email).Value;

            await HandleGoogle(userName);
            return new UserAuthInfo
            {
                IsAuthenticated = false,
                UserName = userName,
                ExposedClaims = claims,
                Roles = roles
            };
        }

        return new UserAuthInfo
        {
            IsAuthenticated = User.Identity.IsAuthenticated,
            UserName = User.Identity.Name,
            ExposedClaims = claims,
            Roles = roles
        };
    }

    private async Task HandleGoogle(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            var parameters = new RegisterParameters
            {
                UserName = userName,
                Password = userName,
                PasswordConfirm = userName
            };
            await Register(parameters);
            return;
        }
        await _signInManager.SignInAsync(user, isPersistent: false);
        //await Login(new LoginParameters { UserName = user, Password = userName, RememberMe = false });
        //  Task.Delay(100).ContinueWith(async (x) => await _signInManager.SignInAsync(user, false));
        //var result = await _signInManager.SignInAsync(user, false);

        //await Login(new LoginParameters { UserName = user, Password = userName });
    }
}

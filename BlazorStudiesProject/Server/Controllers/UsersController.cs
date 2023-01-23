using AutoMapper;
using BlazorStudiesProject.Server.Entities;
using BlazorStudiesProject.Shared;
using BlazorStudiesProject.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorStudiesProject.Server.Controllers;


[Route("api/[controller]")]
//[Authorize]
[ApiController]
public class UsersController : AppBaseController
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IMapper _mapper;

    public UsersController(UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IMapper mapper)
        : base(userManager)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    [HttpDelete("{userName}")]
    public async Task<IActionResult> DeleteUser(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return NotFound();
        }
        _ = await _userManager.DeleteAsync(user);
        return Ok();
    }

    [HttpPost("{userName}")]
    public async Task<IActionResult> MakeUserAdmin(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return NotFound();
        }

        //_ = await _userManager.AddToRoleAsync(user, Strings.AdminRole);
        _ = await _userManager.AddToRoleAsync(user, Strings.AdminRole);
        return Ok();
    }

    [HttpGet("{roleName}")]
    public async Task<ActionResult<IList<UserAdminViewDto>>> GetAllUsersByRole(string roleName)
    {
        //await AddRole("aaa", Strings.BasicUserRole);
        //await AddRole("aaa", Strings.AdminRole);

        var nonAdminUsers = await _userManager.GetUsersInRoleAsync(roleName);
        var dtos = nonAdminUsers.Select(_mapper.Map<UserAdminViewDto>).ToList();
        return Ok(dtos);
    }

    //development only

    private async Task CreateRole(string roleName)
    {
        var newRole = new ApplicationRole
        {
            Name = roleName
        };
        _ = await _roleManager.CreateAsync(newRole);
    }
    private async Task AddRole(string userName, string roleName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        _ = await _userManager.AddToRoleAsync(user, roleName);
    }
}
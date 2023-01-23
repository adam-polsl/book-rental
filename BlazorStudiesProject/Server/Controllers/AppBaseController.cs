using BlazorStudiesProject.Server.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorStudiesProject.Server.Controllers
{
    public class AppBaseController : ControllerBase
    {
        protected readonly UserManager<ApplicationUser> _userManager;

        public AppBaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetLoggedUserEnity()
        {
            var name = User.Identity.Name;

            return await _userManager.FindByNameAsync(name);
        }
    }
}

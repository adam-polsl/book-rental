using Microsoft.AspNetCore.Identity;

namespace BlazorStudiesProject.Server.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public virtual IList<BookUserInfoEntity> BookUserInfos { get; set; }
    }
}

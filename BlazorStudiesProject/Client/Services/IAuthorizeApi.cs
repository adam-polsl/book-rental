using BlazorStudiesProject.Shared;

namespace BlazorStudiesProject.Client.Services;

public interface IAuthorizeApi
{
    Task Login(LoginParameters loginParameters);
    Task Register(RegisterParameters registerParameters);
    Task Logout();
    Task<UserAuthInfo> GetUserInfo();
}

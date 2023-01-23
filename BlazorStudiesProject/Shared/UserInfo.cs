namespace BlazorStudiesProject.Shared;

public class UserAuthInfo
{
    public required bool IsAuthenticated { get; init; }
    public required string UserName { get; init; }
    public required Dictionary<string, string> ExposedClaims { get; init; }
    public required string[] Roles { get; init; }
    //public required List<Claim> ExposedClaims { get; init; }
}

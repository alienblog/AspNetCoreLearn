using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Galaxy
{
    public class AuthConfig
    {
	    public static void Config(AuthorizationOptions options)
	    {
			options.AddPolicy("AdminOnly", policy =>
			{
				policy.RequireClaim(ClaimTypes.Role, "Admin");
			});
		}
    }
}

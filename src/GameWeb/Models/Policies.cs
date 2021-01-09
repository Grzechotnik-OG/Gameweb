using Microsoft.AspNetCore.Authorization;

namespace GameWeb.Models
{
    public class Policies
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Mod = "Mod";
        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin).Build();
        }
        public static AuthorizationPolicy UserPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(User).Build();
        }
        public static AuthorizationPolicy ModPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Mod).Build();
        }
    }
}
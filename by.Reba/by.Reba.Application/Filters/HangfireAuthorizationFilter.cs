using Hangfire.Annotations;
using Hangfire.Dashboard;
using System.Security.Claims;

namespace by.Reba.Application.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            return httpContext.User.HasClaim(c => c.Type.Equals(ClaimsIdentity.DefaultRoleClaimType) && c.Value.ToLower().Equals("admin"));
        }
    }
}

using Hangfire.Annotations;
using Hangfire.Dashboard;
using System.Web;

namespace PFC.SGP.UI.Validation
{
    public class HangfireAuthorization : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return HttpContext.Current.User.Identity.Name.Equals("admin");
            }
            return false;
        }
    }
}
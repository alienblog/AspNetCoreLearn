using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Galaxy
{
    public class RouteConfig
    {
	    public static void RegisterRoute(IRouteBuilder routes)
	    {
            routes.MapRoute("default", "{controller}/{action}");
		}
    }
}

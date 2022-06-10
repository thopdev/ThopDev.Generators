namespace ThopDev.Generator.Blazor.Routes.Models.Routing
{

    public class RoutingBase
    {
        protected string Route { get; }

        protected RoutingBase(string route)
        {
            Route = route;
        }
    }

    public abstract class RoutableBase : RoutingBase
    {
        protected RoutableBase(string route) : base(route)
        {
        }

        public string ToRoute()
        {
            return Route;
        }
    }
}
namespace ThopDev.Generator.Blazor.Routes.Routing;


public static class StaticFiles
{
    public const string RoutingBase = @"
namespace ThopDev.Generator.Blazor.Routes.Routing;

public class RoutingBase
{
    protected RoutingBase(string route)
    {
        Route = route;
    }

    protected string Route { get; }
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
";
}
using System.Linq;
using ThopDev.Generator.Routes.Models;

namespace ThopDev.Generator.Routes.Factories;

public class RouteFactory
{
    private readonly IRouteSegmentFactory _routeSegmentFactory;

    public RouteFactory(IRouteSegmentFactory routeSegmentFactory)
    {
        _routeSegmentFactory = routeSegmentFactory;
    }

    public Route Create(string route, Component component)
    {
        var segment = route.Split('/').Skip(1).Select(_routeSegmentFactory.Create);

        return new Route(route, segment.ToArray(), component);
    }
}
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

    public RouteModel Create(string route, ComponentModel componentModel)
    {
        var segment = route.Split('/').Skip(1).Select(_routeSegmentFactory.Create);

        return new RouteModel(route, segment.ToArray(), componentModel);
    }
}
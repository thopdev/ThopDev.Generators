using System.Collections.Generic;
using System.Linq;
using ThopDev.Generator.Blazor.Routes.Models;

namespace ThopDev.Generator.Blazor.Routes.Factories;

public class RouteGroupingFactory
{
    public IEnumerable<RouteGroupingModel> GetAllGroupRoutes(IEnumerable<RouteModel> routes)
    {
        var groupRoutes = GroupRoutes(routes);
        return groupRoutes.SelectMany(route => route.GetAllSubRoutes());
    }

    private IEnumerable<RouteGroupingModel> GroupRoutes(IEnumerable<RouteModel> routes, int indent = 0,
        RouteGroupingModel parent = null)
    {
        var routeModels = routes as RouteModel[] ?? routes.ToArray();
        foreach (var group in routeModels.GroupBy(route => route.Segments[indent].Name))
        {
            var routeGrouping = new RouteGroupingModel(
                routeModels.First().Segments[indent],
                parent,
                group.FirstOrDefault(route => route.Segments.Length == indent + 1));

            routeGrouping.SubRoutes = GroupRoutes(
                group.Where(route => route.Segments.Length != indent + 1),
                indent + 1,
                routeGrouping).ToArray();
            yield return routeGrouping;
        }
    }
}
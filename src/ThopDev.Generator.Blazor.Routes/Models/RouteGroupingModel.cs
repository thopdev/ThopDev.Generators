using System.Collections.Generic;
using System.Linq;
using ThopDev.Generator.Blazor.Routes.Extensions;

namespace ThopDev.Generator.Blazor.Routes.Models;

public class RouteGroupingModel
{
    private string _fullName;
    private string _name;

    public RouteGroupingModel(RouteSegmentModel segment, RouteGroupingModel parent, RouteModel route)

    {
        Segment = segment;
        Parent = parent;
        Route = route;
    }

    public RouteModel Route { get; }
    public RouteSegmentModel Segment { get; }

    public RouteGroupingModel[] SubRoutes { get; set; }

    public RouteGroupingModel Parent { get; }

    public string FullName => _fullName ??= CreateFullName();

    public string Name => _name ??= CreateName();

    public IEnumerable<RouteGroupingModel> GetAllSubRoutes()
    {
        var allSubRoutes = SubRoutes.SelectMany(route => route.GetAllSubRoutes());
        return allSubRoutes.Concat(new[] { this });
    }

    private string CreateFullName()
    {
        return Parent?.FullName + Name;
    }

    private string CreateName()
    {
        if (Segment is RouteParameterSegmentModel) return "With" + Segment.Name.FirstCharToUpper();

        return Segment.Name.FirstCharToUpper();
    }

    public string GetRouteClassName()
    {
        return FullName + "Route";
    }
}
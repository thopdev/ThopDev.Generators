using System.Collections.Generic;

namespace ThopDev.Generator.Blazor.Routes.Models;

public class ComponentModel
{
    private readonly List<RouteModel> _routes = new();

    public string Name { get; set; }
    public string Namespace { get; set; }

    public List<QueryParameter> QueryParameters { get; set; }

    public IReadOnlyList<RouteModel> Routes => _routes;
    /// <summary>
    /// </summary>
    /// <param name="routeModel"></param>
    public void AddRoute(RouteModel routeModel)
    {
        _routes.Add(routeModel);
    }
}
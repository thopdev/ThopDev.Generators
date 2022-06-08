using System.Collections.Generic;

namespace ThopDev.Generator.Routes.Models;

public class Component
{
    private readonly List<Route> _routes = new();

    public string Name { get; set; }

    public IReadOnlyList<Route> Routes => _routes;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="route"></param>
    public void AddRoute(Route route)
    {
        _routes.Add(route);
    }
}
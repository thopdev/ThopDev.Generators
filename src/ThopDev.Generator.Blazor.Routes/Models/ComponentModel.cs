﻿using System.Collections.Generic;

namespace ThopDev.Generator.Routes.Models;

public class ComponentModel
{
    private readonly List<RouteModel> _routes = new();

    public string Name { get; set; }

    public IReadOnlyList<RouteModel> Routes => _routes;

    /// <summary>
    /// </summary>
    /// <param name="routeModel"></param>
    public void AddRoute(RouteModel routeModel)
    {
        _routes.Add(routeModel);
    }
}
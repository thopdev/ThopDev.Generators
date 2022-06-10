using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using ThopDev.Generator.Routes.Extensions;
using ThopDev.Generator.Routes.Models;
using ThopDev.Generator.Utils.Constants;
using ThopDev.Generator.Utils.Helpers;

namespace ThopDev.Generator.Routes.ClassConverters;

public class ClassConverter
{


    public static string CreateFactoryClassString(IEnumerable<RouteGroupingModel> routes)
    {
        var routeGroupingModels = routes as List<RouteGroupingModel> ?? routes.ToList();


        using var generator = FileGenerationHelper.Create();

        var namespaceGenerator = generator
            .AddUsing("ThopDev.Generator.Blazor.Routes.Models.Routing")
            .OpenNamespace("ThopDev.Generator.Blazor.Routes.Models.Routing");

        var navigationFactory = namespaceGenerator
            .OpenClass(Accessibility.Public, ClassType.Class, "NavigationFactory")
            .AddConst(Accessibility.Private, NativeTypes.String, "Route", string.Empty);

        foreach (var route in routeGroupingModels.Where(x => x.Parent is null))
        {
            navigationFactory.AddRouteMethod(route);
        }

        var fileToClose = navigationFactory.CloseClass();

        foreach (var routeGroup in routeGroupingModels)
        {
            var routeClass = namespaceGenerator
                .OpenClass(Accessibility.Public, ClassType.Class, routeGroup.GetRouteClassName(),
                    routeGroup.Route is null ? "RoutingBase" : "RoutableBase")
                .OpenConstructor(Accessibility.Internal, new[] { ("string", "route") }, new[] { "route" })
                .CloseMethod();

            foreach (var subRoute in routeGroup.SubRoutes)
            {
                routeClass.AddRouteMethod(subRoute);
            }

            routeClass.CloseClass();
        }

        return namespaceGenerator.CloseNamespace().ToFileString();
    }
}
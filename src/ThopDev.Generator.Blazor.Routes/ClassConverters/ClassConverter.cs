using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using ThopDev.Generator.Blazor.Routes.Constants;
using ThopDev.Generator.Blazor.Routes.Extensions;
using ThopDev.Generator.Blazor.Routes.Helpers;
using ThopDev.Generator.Blazor.Routes.Models;

namespace ThopDev.Generator.Blazor.Routes.ClassConverters;

public class ClassConverter
{
    public static string CreateFactoryClassString(IEnumerable<RouteGroupingModel> routes)
    {
        var routeGroupingModels = routes as List<RouteGroupingModel> ?? routes.ToList();


        using var generator = FileGenerationHelper.Create();

        var namespaceGenerator = generator
            .OpenNamespace("ThopDev.Generator.Blazor.Routes.Models.Routing");

        var navigationFactory = namespaceGenerator
            .OpenClass(Accessibility.Public, ClassType.Class, "NavigationFactory", "INavigationFactory")
            .AddConst(Accessibility.Private, NativeTypes.String, "Route", string.Empty);

        foreach (var route in routeGroupingModels.Where(x => x.Parent is null)) navigationFactory.AddRouteMethod(route);

        var fileToClose = navigationFactory.CloseClass();

        var navigationFactoryInterface = namespaceGenerator
            .OpenClass(Accessibility.Public, ClassType.Interface, "INavigationFactory");

        foreach (var route in routeGroupingModels.Where(x => x.Parent is null)) navigationFactoryInterface.AddRouteInterfaceMethod(route);

        fileToClose = navigationFactoryInterface.CloseClass();

        foreach (var routeGroup in routeGroupingModels)
        {
            var routeClass = namespaceGenerator
                .OpenClass(Accessibility.Public, ClassType.Class, routeGroup.GetRouteClassName(),
                    routeGroup.Route is null ? "RoutingBase" : "RoutableBase")
                .OpenConstructor(Accessibility.Internal, new[] { ("string", "route") }, new[] { "route" })
                .CloseMethod();

            foreach (var subRoute in routeGroup.SubRoutes) routeClass.AddRouteMethod(subRoute);

            routeClass.CloseClass();
        }

        return namespaceGenerator.CloseNamespace().ToFileString();
    }
}
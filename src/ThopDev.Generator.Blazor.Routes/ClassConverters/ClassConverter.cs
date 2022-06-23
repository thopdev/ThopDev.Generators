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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="routes"></param>

    public static string CreateFactoryClassString(IEnumerable<RouteGroupingModel> routes)
    {
        var routeGroupingModels = routes as List<RouteGroupingModel> ?? routes.ToList();


        using var generator = FileGenerationHelper.Create();

        var namespaceGenerator = generator
            .OpenNamespace("ThopDev.Generator.Blazor.Routes");

        var navigationFactory = namespaceGenerator
            .OpenClass(Accessibility.Public, ClassType.Class, "NavigationFactory", "INavigationFactory")
            .AddConst(Accessibility.Private, NativeTypes.String, "Route", string.Empty);

        foreach (var route in routeGroupingModels.Where(x => x.Parent is null)) navigationFactory.AddRouteMethod(route);

        var fileToClose = navigationFactory.CloseClass();

        var navigationFactoryInterface = namespaceGenerator
            .OpenClass(Accessibility.Public, ClassType.Interface, "INavigationFactory");

        foreach (var route in routeGroupingModels.Where(x => x.Parent is null))
            navigationFactoryInterface.AddRouteInterfaceMethod(route);

        fileToClose = navigationFactoryInterface.CloseClass();

        foreach (var routeGroup in routeGroupingModels)
        {
            var routeClass = namespaceGenerator
                .OpenClass(Accessibility.Public, ClassType.Class, routeGroup.GetRouteClassName(),
                    routeGroup.Route is null ? "RoutingBase" : "RoutableBase")
                .OpenConstructor(Accessibility.Internal, new[] { ("string", "route") }, new[] { "route" })
                .CloseMethod();

            if (routeGroup.Route is not null)
            {
                var route = routeGroup.Route;

                routeClass.WriteSummary("<summary>")
                    .WriteSummary(
                        $"Route from component: <see cref=\"{routeGroup.Route.ComponentModel.Namespace}.{routeGroup.Route.ComponentModel.Name}\" />")
                    .WriteSummary("</summary>")
                    .WriteSummary($"<returns>Route based on {routeGroup.Route.Value}</returns>");
                
                var method = routeClass.OpenMethod(Accessibility.Public, "string", "ToRoute",
                        route.ComponentModel.QueryParameters
                            .Select(qp => new MethodParameter(qp.Name.FirstCharToLower(), qp.Type, "default")).ToArray())
                    .WriteLine("var queryStringCollection = System.Web.HttpUtility.ParseQueryString(string.Empty);")
                    .WriteLine(string.Empty);
                    
                foreach (var parameter in route.ComponentModel.QueryParameters)
                {
                    method
                        .WriteLine($"if ({parameter.Name.FirstCharToLower()} != default)")
                        .WriteLine("{")
                        .Indent()
                        .WriteLine(
                            $"queryStringCollection.Add({Symbols.Quotation}{parameter.Name}{Symbols.Quotation}, {parameter.Name.FirstCharToLower()}.ToString(System.Globalization.CultureInfo.InvariantCulture));")
                        .Outdent()
                        .WriteLine("}");
                }

                method
                    .WriteLine("return Route + (string.IsNullOrEmpty(queryStringCollection.ToString()) ? string.Empty : \"?\" + queryStringCollection.ToString());" )
                    
                    .CloseMethod();
                
            }

            foreach (var subRoute in routeGroup.SubRoutes) routeClass.AddRouteMethod(subRoute);

            routeClass.CloseClass();
        }

        return namespaceGenerator.CloseNamespace().ToFileString();


    }
}
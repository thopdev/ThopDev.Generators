using Microsoft.CodeAnalysis;
using ThopDev.Generator.Blazor.Routes.Constants;
using ThopDev.Generator.Blazor.Routes.Helpers;
using ThopDev.Generator.Blazor.Routes.Models;

namespace ThopDev.Generator.Blazor.Routes.Extensions;

public static class ClassGeneratorExtensions
{
    public static IClassGenerator AddRouteMethod(this IClassGenerator generator, RouteGroupingModel route)
    {
        if (route.Segment is RouteParameterSegmentModel parameter)
            generator.OpenMethod(Accessibility.Public, route.GetRouteClassName(), route.Name,
                    new (string, string)[] { parameter })
                .WriteLine($"var route = Route + \"/\" + {parameter.Name};")
                .WriteLine($"return new {route.GetRouteClassName()}(route);")
                .CloseMethod();
        else
            generator.OpenMethod(Accessibility.Public, route.GetRouteClassName(), route.Name)
                .WriteLine($"var route = Route + {Symbols.Quotation}/{route.Segment.Name}{Symbols.Quotation};")
                .WriteLine($"return new {route.GetRouteClassName()}(route);")
                .CloseMethod();

        return generator;
    }
    public static IClassGenerator AddRouteInterfaceMethod(this IClassGenerator generator, RouteGroupingModel route)
    {
        if (route.Segment is RouteParameterSegmentModel parameter)
            generator.OpenInterfaceMethod(Accessibility.Public, route.GetRouteClassName(), route.Name,
                new (string, string)[] { parameter });
        else
            generator.OpenInterfaceMethod(Accessibility.Public, route.GetRouteClassName(), route.Name);

        return generator;
    }
}
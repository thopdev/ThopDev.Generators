using Microsoft.CodeAnalysis;
using ThopDev.Generator.Routes.Models;
using ThopDev.Generator.Utils.Constants;
using ThopDev.Generator.Utils.Helpers;

namespace ThopDev.Generator.Routes.Extensions;

public static class ClassGeneratorExtensions
{
    public static IClassGenerator AddRouteMethod(this IClassGenerator generator, RouteGroupingModel route)
    {
        if (route.Segment is RouteParameterSegmentModel parameter)
        {
            generator.OpenMethod(Accessibility.Public, route.GetRouteClassName(), route.Name,
                    new (string, string)[] { parameter })
                .WriteLine($"var route = Route + \"/\" + {parameter.Name};")
                .WriteLine($"return new {route.GetRouteClassName()}(route);")
                .CloseMethod();
        }
        else
        {
            generator.OpenMethod(Accessibility.Public, route.GetRouteClassName(), route.Name)
                .WriteLine($"var route = Route + {Symbols.Quotation}/{route.Segment.Name}{Symbols.Quotation};")
                .WriteLine($"return new {route.GetRouteClassName()}(route);")
                .CloseMethod();
        }

        return generator;
    }
    
}
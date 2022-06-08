using System.Linq;
using ThopDev.Generator.Routes.Extensions;

namespace ThopDev.Generator.Routes.Models;

public class Route
{
    public Route(string route, RouteSegment[] segments, Component component)
    {
        Value = route;
        Segments = segments;

        NameSegments = segments.Where(x => x is not RouteParameterSegment).ToArray();
        ParameterSegments = segments.OfType<RouteParameterSegment>().ToArray();
            
        Component = component;
        Component.AddRoute(this);
    }

    public string Value { get; set; }
    public RouteSegment[] Segments { get; }
    public RouteSegment[] NameSegments { get; }
    public RouteParameterSegment[] ParameterSegments { get; }


    public Component Component { get; }

    public string GetParametersString()
    {
        return string.Join(", ", ParameterSegments.Select(GetParameterString));
    }

    private string GetParameterString(RouteParameterSegment segment)
    {
        return $"{segment.Type} {segment.Name}{GetNullableParameterString(segment)}";
    }

    public string GetNullableParameterString(RouteParameterSegment segment)
    {
        return segment.Nullable ? " = null" : string.Empty;
    }

    public string GetFunctionName()
    {
        return NameSegments.Last().Name.FirstCharToUpper();
    }
    
    public string ToRouteString()
    {
        return "\"/\" + " + string.Join(" + \"/\" + ", Segments.Select(segment => segment.ToSegmentString()));
    }
}
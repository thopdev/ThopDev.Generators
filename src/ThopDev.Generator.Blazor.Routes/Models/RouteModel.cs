using System.Linq;
using ThopDev.Generator.Routes.Extensions;

namespace ThopDev.Generator.Routes.Models;

public class RouteModel
{
    public RouteModel(string route, RouteSegmentModel[] segments, ComponentModel componentModel)
    {
        Value = route;
        Segments = segments;

        NameSegments = segments.Where(x => x is not RouteParameterSegmentModel).ToArray();
        ParameterSegments = segments.OfType<RouteParameterSegmentModel>().ToArray();

        ComponentModel = componentModel;
        ComponentModel.AddRoute(this);
    }

    public string Value { get; set; }
    public RouteSegmentModel[] Segments { get; }
    public RouteSegmentModel[] NameSegments { get; }
    public RouteParameterSegmentModel[] ParameterSegments { get; }


    public ComponentModel ComponentModel { get; }

    public string GetParametersString()
    {
        return string.Join(", ", ParameterSegments.Select(GetParameterString));
    }

    private string GetParameterString(RouteParameterSegmentModel segmentModel)
    {
        return $"{segmentModel.Type} {segmentModel.Name}{GetNullableParameterString(segmentModel)}";
    }

    public string GetNullableParameterString(RouteParameterSegmentModel segmentModel)
    {
        return segmentModel.Nullable ? " = null" : string.Empty;
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
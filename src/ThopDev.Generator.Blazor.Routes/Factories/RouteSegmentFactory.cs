using System.Linq;
using ThopDev.Generator.Blazor.Routes.Models;

namespace ThopDev.Generator.Blazor.Routes.Factories;

public interface IRouteSegmentFactory
{
    RouteSegmentModel Create(string segment);
}

public class RouteSegmentFactory : IRouteSegmentFactory
{
    public RouteSegmentModel Create(string segment)
    {
        if (IsParameter(segment))
            return segment.Contains(":")
                ? CreateRestrictedRouteParameterSegment(segment)
                : CreateStringRouteParameterSegment(segment);

        return new RouteSegmentModel
        {
            Name = segment
        };
    }

    private RouteSegmentModel CreateStringRouteParameterSegment(string segment)
    {
        var (nullable, name) = IsNullable(segment.Substring(1, segment.Length - 2));

        return new RouteParameterSegmentModel
        {
            Value = segment,
            Name = name,
            Type = "string",
            Nullable = nullable
        };
    }

    private RouteSegmentModel CreateRestrictedRouteParameterSegment(string segment)
    {
        var substring = segment.Substring(1, segment.Length - 2).Split(':');
        var name = substring.First();
        var (nullable, type) = IsNullable(substring.Last());

        return new RouteParameterSegmentModel
        {
            Value = segment,
            Name = name,
            Type = type,
            Nullable = nullable
        };
    }

    private (bool, string) IsNullable(string value)
    {
        return value.EndsWith("?") ? (true, value.Substring(0, value.Length - 1)) : (false, value);
    }

    private static bool IsParameter(string value)
    {
        return value.StartsWith("{") && value.EndsWith("}");
    }
}
using System.Linq;
using ThopDev.Generator.Routes.Extensions;
using ThopDev.Generator.Routes.Models;

namespace ThopDev.Generator.Routes.Factories;

public interface IRouteSegmentFactory
{
    RouteSegment Create(string segment);
}

public class RouteSegmentFactory : IRouteSegmentFactory
{
    public RouteSegment Create(string segment)
    {
        if (IsParameter(segment))
            return segment.Contains(":")
                ? CreateRestrictedRouteParameterSegment(segment)
                : CreateStringRouteParameterSegment(segment);

        return new RouteSegment
        {
            Name = segment
        };
    }

    private RouteSegment CreateStringRouteParameterSegment(string segment)
    {
        var (nullable, name) = IsNullable(segment.Substring(1, segment.Length - 2));

        return new RouteParameterSegment
        {
            Value = segment,
            Name = name,
            Type = "string",
            Nullable = nullable
        };
    }

    private RouteSegment CreateRestrictedRouteParameterSegment(string segment)
    {
        var substring = segment.Substring(1, segment.Length - 2).Split(':');
        var name = substring.First();
        var (nullable, type) = IsNullable(substring.Last());

        return new RouteParameterSegment
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
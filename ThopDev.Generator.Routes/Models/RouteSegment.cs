namespace ThopDev.Generator.Routes.Models;

public class RouteParameterSegment : RouteSegment
{
    public string Value { get; set; }
    public string Type { get; set; }
    public bool Nullable { get; set; }

    public override string ToSegmentString()
    {
        return Name;
    }
}

public class RouteSegment
{
    public string Name { get; set; }

    public virtual string ToSegmentString()
    {
        return "\"" + Name + "\"";
    }
}
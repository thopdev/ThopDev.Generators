namespace ThopDev.Generator.Routes.Models;

public class RouteParameterSegmentModel : RouteSegmentModel
{
    public string Value { get; set; }
    public string Type { get; set; }
    public bool Nullable { get; set; }

    public override string ToSegmentString()
    {
        return Name;
    }

    public static implicit operator (string, string)(RouteParameterSegmentModel model) => (model.Type, model.Name);
}

public class RouteSegmentModel
{
    public string Name { get; set; }

    public virtual string ToSegmentString()
    {
        return "\"" + Name + "\"";
    }
}
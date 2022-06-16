namespace ThopDev.Generator.Blazor.Routes.Models;

public readonly struct MethodParameter
{
    public MethodParameter(string name, string type, string defaultValue = null)
    {
        Name = name;
        Type = type;
        DefaultValue = defaultValue;
    }

    public string Name { get; }
    public string Type { get; }
    public string DefaultValue { get; }

    public override string ToString()
    {
        return DefaultValue is null ? $"{Type} {Name}" : $"{Type} {Name} = {DefaultValue}";
    }
}
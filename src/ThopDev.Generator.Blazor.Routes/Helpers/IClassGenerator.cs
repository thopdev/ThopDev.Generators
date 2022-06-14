using Microsoft.CodeAnalysis;

namespace ThopDev.Generator.Blazor.Routes.Helpers;

public interface IClassGenerator
{
    public IClassGenerator AddConst(Accessibility accessibility, string type, string name, string value);

    public IMethodGenerator OpenConstructor(Accessibility accessibility, (string, string)[] parameters = null,
        string[] baseParameters = null);

    public IMethodGenerator OpenMethod(Accessibility accessibility, string returnType, string name,
        MethodParameter[] parameters = null);

    public IClassGenerator OpenInterfaceMethod(Accessibility accessibility, string returnType, string name,
        (string, string)[] parameters = null);
    
    public INamespaceGenerator CloseClass();
}

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
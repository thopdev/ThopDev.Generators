namespace ThopDev.Generator.Blazor.Routes.Helpers;

public interface IMethodGenerator
{
    public IMethodGenerator Write(string value);
    public IMethodGenerator WriteLine(string value);
    public IMethodGenerator Indent();
    public IMethodGenerator Outdent();
    public IClassGenerator CloseMethod();
}
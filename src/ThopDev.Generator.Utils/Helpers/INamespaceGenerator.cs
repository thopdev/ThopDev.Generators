using Microsoft.CodeAnalysis;

namespace ThopDev.Generator.Utils.Helpers;

public interface INamespaceGenerator
{
    public IClassGenerator OpenClass(Accessibility accessibility, ClassType type, string name,
        params string[] inherits);

    public ICloseFileGenerator CloseNamespace();
}
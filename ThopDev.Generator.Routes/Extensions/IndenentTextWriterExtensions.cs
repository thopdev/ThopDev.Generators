using System.CodeDom.Compiler;

namespace ThopDev.Generator.Routes.Extensions;

public static class IndentedTextWriterExtensions
{
    public static void OpenNamespace(this IndentedTextWriter indentWriter)
    {
        indentWriter.WriteLine("namespace ThopDev.Generator.Routes {");
        indentWriter.Indent++;
        indentWriter.WriteLine();
    }

    public static void CloseBracket(this IndentedTextWriter indentWriter)
    {
        indentWriter.Indent--;
        indentWriter.WriteLine("}");
    }

    public static void OpenClass(this IndentedTextWriter indentWriter, string className)
    {
        indentWriter.WriteLine($"public class {className} {{");
        indentWriter.Indent++;
        indentWriter.WriteLine();
    }
}
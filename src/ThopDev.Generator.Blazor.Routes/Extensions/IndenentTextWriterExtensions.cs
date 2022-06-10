using System;
using System.CodeDom.Compiler;

namespace ThopDev.Generator.Routes.Extensions;

public static class IndentedTextWriterExtensions
{
    public static void OpenNamespace(this IndentedTextWriter indentWriter)
    {
        indentWriter.WriteLine("namespace ThopDev.Generator.Blazor.Routes {");
        indentWriter.Indent++;
        indentWriter.WriteLine();
    }

    public static void CloseBracket(this IndentedTextWriter indentWriter)
    {
        indentWriter.Indent--;
        indentWriter.WriteLine("}");
        indentWriter.WriteLine();
    }

    public static void OpenClass(this IndentedTextWriter indentWriter, string className)
    {
        indentWriter.WriteLine($"public class {className} {{");
        indentWriter.Indent++;
        indentWriter.WriteLine();
    }

    public static void OpenClass(this IndentedTextWriter indentWriter, string type,  string className, params string[] inherents)
    {
        indentWriter.WriteLine($"{type} class {className}{GetInheretence(inherents)} {{");
        indentWriter.Indent++;
        indentWriter.WriteLine();
    }

    private static string GetInheretence(string[] inherents)
    {
        if (inherents is null || inherents.Length == 0)
        {
            return string.Empty;
        }

        return $" : " + string.Join(", ", inherents);



    }
    
}
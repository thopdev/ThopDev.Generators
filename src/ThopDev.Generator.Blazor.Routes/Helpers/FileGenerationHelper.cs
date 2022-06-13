using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using ThopDev.Generator.Blazor.Routes.Constants;

namespace ThopDev.Generator.Blazor.Routes.Helpers;

public class FileGenerationHelper : IFileGenerator, INamespaceGenerator, IClassGenerator, IMethodGenerator,
    ICloseFileGenerator
{
    private readonly IndentedTextWriter _indentedTextWriter;
    private readonly StringBuilder _stringBuilder;
    private readonly TextWriter _textWriter;

    private FileGenerationHelper()
    {
        _stringBuilder = new StringBuilder();
        _textWriter = new StringWriter(_stringBuilder);
        _indentedTextWriter = new IndentedTextWriter(_textWriter);
    }

    private string CurrentClassName { get; set; }

    public IClassGenerator AddConst(Accessibility accessibility, string type, string name, string value)
    {
        _indentedTextWriter.Write($"{accessibility.ToString().ToLower()} const {type} {name} = ");
        _indentedTextWriter.Write(type == NativeTypes.String
            ? $"{Symbols.Quotation}{value}{Symbols.Quotation}"
            : value);
        _indentedTextWriter.WriteLine(";");
        return this;
    }

    public IMethodGenerator OpenConstructor(Accessibility accessibility, (string, string)[] parameters,
        string[] baseParameters)
    {
        _indentedTextWriter.Write($"{accessibility.ToString().ToLower()} {CurrentClassName}(");
        _indentedTextWriter.Write(string.Join(", ",
            parameters?.Select(parameter => $"{parameter.Item1} {parameter.Item2}") ?? Array.Empty<string>()));
        _indentedTextWriter.Write(")");

        if (baseParameters is not null && baseParameters.Any())
            _indentedTextWriter.Write($": base({string.Join(", ", baseParameters)})");
        _indentedTextWriter.WriteLine();

        _indentedTextWriter.WriteLine("{");
        Indent();
        return this;
    }

    public IMethodGenerator OpenMethod(Accessibility accessibility, string returnType, string name,
        (string, string)[] parameters = null)
    {
        _indentedTextWriter.Write($"{accessibility.ToString().ToLower()} {returnType} {name}(");
        _indentedTextWriter.Write(string.Join(", ",
            parameters?.Select(parameter => $"{parameter.Item1} {parameter.Item2}") ?? Array.Empty<string>()));
        _indentedTextWriter.WriteLine(")");

        _indentedTextWriter.WriteLine("{");
        Indent();
        return this;
    }

    public IClassGenerator OpenInterfaceMethod(Accessibility accessibility, string returnType, string name,
        (string, string)[] parameters = null)
    {
        _indentedTextWriter.Write($"{accessibility.ToString().ToLower()} {returnType} {name}(");
        _indentedTextWriter.Write(string.Join(", ",
            parameters?.Select(parameter => $"{parameter.Item1} {parameter.Item2}") ?? Array.Empty<string>()));
        _indentedTextWriter.WriteLine(");");
        return this;
    }

    public INamespaceGenerator CloseClass()
    {
        Outdent();
        _indentedTextWriter.WriteLine("}");
        return this;
    }

    public string ToFileString()
    {
        return _textWriter.ToString();
    }

    public IFileGenerator AddUsing(string value)
    {
        _indentedTextWriter.WriteLine($"using {value};");
        return this;
    }

    public INamespaceGenerator OpenNamespace(string value)
    {
        _indentedTextWriter.WriteLine();
        _indentedTextWriter.WriteLine($"namespace {value}");
        _indentedTextWriter.WriteLine("{");
        _indentedTextWriter.Indent++;
        return this;
    }


    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _indentedTextWriter.Dispose();
        _textWriter.Dispose();
    }

    public IMethodGenerator Write(string value)
    {
        _indentedTextWriter.Write(value);
        return this;
    }

    public IMethodGenerator WriteLine(string value)
    {
        _indentedTextWriter.WriteLine(value);
        return this;
    }

    public IMethodGenerator Indent()
    {
        _indentedTextWriter.Indent++;
        return this;
    }

    public IMethodGenerator Outdent()
    {
        _indentedTextWriter.Indent--;
        return this;
    }

    public IClassGenerator CloseMethod()
    {
        Outdent();
        _indentedTextWriter.WriteLine("}");
        return this;
    }

    public IClassGenerator OpenClass(Accessibility accessibility, ClassType type, string name,
        params string[] inherits)
    {
        CurrentClassName = name;

        _indentedTextWriter.WriteLine();
        _indentedTextWriter.Write($"{accessibility.ToString().ToLower()} {type.ToString().ToLower()} {name}");

        if (inherits.Any())
        {
            _indentedTextWriter.Write(" : ");
            _indentedTextWriter.Write(string.Join(", ", inherits));
        }

        _indentedTextWriter.WriteLine();
        _indentedTextWriter.WriteLine("{");
        Indent();
        return this;
    }

    public ICloseFileGenerator CloseNamespace()
    {
        Outdent();
        _indentedTextWriter.WriteLine("}");
        return this;
    }

    public static IFileGenerator Create()
    {
        return new FileGenerationHelper();
    }
}
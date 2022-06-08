using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ThopDev.Generator.Routes.Extensions;
using ThopDev.Generator.Routes.Models;

namespace ThopDev.Generator.Routes.ClassConverters;

public class ClassConverter
{
    public static string CreateFactoryClassString(IEnumerable<Route> routes)
    {
        var classBuilder = new StringBuilder();
        using var textWriter = new StringWriter(classBuilder);
        using var indentWriter = new IndentedTextWriter(textWriter);

        indentWriter.OpenNamespace();

        indentWriter.OpenClass("NavigationFactory");
        foreach (var route in routes)
        {
            indentWriter.WriteLine("/// <summary>");
            indentWriter.WriteLine($"/// Route from: {route.Component.Name}");
            indentWriter.WriteLine("/// </summary>");
            foreach (var parameter in route.ParameterSegments)
            {
                indentWriter.WriteLine($"/// <param name=\"{parameter.Name}\">{parameter.Value}</param>");
            }
            
            
            indentWriter.WriteLine($"/// <returns>Creates string based on route: {route.Value}</returns>");
            
            indentWriter.WriteLine($"public string {route.GetFunctionName()}({route.GetParametersString()}){{");
            indentWriter.Indent++;


            indentWriter.Write("return ");
            indentWriter.Write(route.ToRouteString());
            indentWriter.WriteLine(";");

            indentWriter.CloseBracket();
            indentWriter.WriteLine();
        }

        indentWriter.CloseBracket();
        indentWriter.CloseBracket();

        return textWriter.ToString();
    }
}
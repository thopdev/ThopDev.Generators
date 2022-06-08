using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ThopDev.Generator.Routes.ClassConverters;
using ThopDev.Generator.Routes.Extensions;
using ThopDev.Generator.Routes.Factories;
using ThopDev.Generator.Routes.Models;

namespace ThopDev.Generator.Routes;

[Generator]
public class Generator : ISourceGenerator
{
    private readonly RouteFactory _routeFactory;

    public Generator()
    {
        _routeFactory = new RouteFactory(new RouteSegmentFactory());
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new WrapperAttributeSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        try
        {
            var receiver = context.SyntaxReceiver as WrapperAttributeSyntaxReceiver;
            var compilation = context.Compilation;
            var routes = GetClassRoutes(receiver, compilation);

            // ReSharper disable once PossibleNullReferenceException
            var textWriter = ClassConverter.CreateFactoryClassString(routes);
            context.AddSource("NavigationFactory.g.cs", textWriter.ToString());
        }
        catch (Exception e)
        {
            Debugger.Launch();
        } 
    }
    
    private IEnumerable<Route> GetClassRoutes(WrapperAttributeSyntaxReceiver receiver, Compilation compilation)
    {
        foreach (var routeClass in receiver.ClassToAugments)
        {
            var component = new Component{Name = routeClass.Identifier.Text};
            foreach (var route in GetRoutes(compilation, routeClass))
                yield return _routeFactory.Create(route, component);
        }
    }

    private static IEnumerable<string> GetRoutes(Compilation compilation, ClassDeclarationSyntax routeClass)
    {
        var semanticModel = compilation.GetSemanticModel(routeClass.SyntaxTree);
        foreach (var routeAttribute in routeClass.AttributeLists.SelectMany(attr =>
                     attr.Attributes.Where(x => x.Name.ToString() == "Route")))
        {
            var routeArg = routeAttribute?.ArgumentList?.Arguments[0];
            var routeExpr = routeArg?.Expression;
            if (routeExpr != null) yield return semanticModel.GetConstantValue(routeExpr).ToString();
        }
    }
}
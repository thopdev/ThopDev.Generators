using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ThopDev.Generator.Routes.ClassConverters;
using ThopDev.Generator.Routes.Factories;
using ThopDev.Generator.Routes.Models;

namespace ThopDev.Generator.Routes;

[Generator]
public class Generator : ISourceGenerator
{
    private readonly RouteFactory _routeFactory;
    private readonly RouteGroupingFactory _routeGroupingFactory;
    
    public Generator()
    {
        _routeGroupingFactory = new RouteGroupingFactory();
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

            var routes = GetClassRoutes(receiver, compilation).ToList();
            var groups = _routeGroupingFactory.GetAllGroupRoutes(routes);
            // ReSharper disable once PossibleNullReferenceException
            var textWriter = ClassConverter.CreateFactoryClassString(groups);
            context.AddSource("NavigationFactory.g.cs", textWriter);
            
        }
#pragma warning disable CS0168
        catch (Exception e)
#pragma warning restore CS0168
        {
            Debugger.Launch();
        }
    }



    private IEnumerable<RouteModel> GetClassRoutes(WrapperAttributeSyntaxReceiver receiver, Compilation compilation)
    {
        foreach (var routeClass in receiver.ClassToAugments)
        {
            var component = new ComponentModel { Name = routeClass.Identifier.Text };
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
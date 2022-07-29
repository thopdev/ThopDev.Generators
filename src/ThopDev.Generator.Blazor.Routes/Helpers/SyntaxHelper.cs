using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ThopDev.Generator.Blazor.Routes.Helpers;

public static class SyntaxNodeHelper
{


    public static string GetNamespace(this ClassDeclarationSyntax classDeclarationSyntax)
    {

        var namespaceDeclarationSyntax = classDeclarationSyntax.Parent as BaseNamespaceDeclarationSyntax;

        return namespaceDeclarationSyntax.Name.ToString();
        
    }
    
    public static bool TryGetParentSyntax<T>(SyntaxNode syntaxNode, out T result) 
        where T : SyntaxNode
    {
        // set defaults
        result = null;

        if (syntaxNode == null)
        {
            return false;
        }

        try
        {
            syntaxNode = syntaxNode.Parent;

            if (syntaxNode == null)
            {
                return false;
            }

            if (syntaxNode.GetType() == typeof (T))
            {
                result = syntaxNode as T;
                return true;
            }

            return TryGetParentSyntax<T>(syntaxNode, out result);
        }
        catch
        {
            return false;
        }
    }
}
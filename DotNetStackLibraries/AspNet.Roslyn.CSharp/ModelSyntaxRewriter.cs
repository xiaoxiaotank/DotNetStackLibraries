using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Roslyn.CSharp
{
    class ModelSyntaxRewriter : CSharpSyntaxRewriter
    {
        public readonly Dictionary<string, List<string>> Models = new Dictionary<string, List<string>>();

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var classnode = node.Parent as ClassDeclarationSyntax;
            if (classnode != null && !Models.ContainsKey(classnode.Identifier.ValueText))
            {
                Models.Add(classnode.Identifier.ValueText, new List<string>());
            }

            Models[classnode.Identifier.ValueText].Add(node.Identifier.ValueText);
            return base.VisitPropertyDeclaration(node);
        }
    }
}

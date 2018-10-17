using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Roslyn.MSBuild
{
    class TypeParameterVisitor : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitTypeParameterList(TypeParameterListSyntax node)
        {
            var tParameters = new SeparatedSyntaxList<TypeParameterSyntax>();
            tParameters = tParameters.Add(SyntaxFactory.TypeParameter("TParameter"));

            return base.VisitTypeParameterList(node);

            #region 这部分代码就是基类“CSharpSyntaxRewriter”中该方法的实现
            //var lessThanToken = this.VisitToken(node.LessThanToken);
            //var parameters = this.VisitList(node.Parameters);
            //var greaterThanToken = this.VisitToken(node.GreaterThanToken);
            //return node.Update(lessThanToken, parameters, greaterThanToken);
            #endregion
        }

    }
}

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Roslyn.MSBuild
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var solution = await MSBuildWorkspace
                .Create()
                .OpenSolutionAsync("../../../DotNetStackLibraries.sln");

#warning project中的documents为空，可能是目前版本MSBuild库的BUG
            var project = solution.Projects.Single(p => p.Name.Equals("AspNet.Roslyn.PendingAnalysis"));
            //.OpenProjectAsync(@"../../../AspNet.Roslyn.PendingAnalysis/AspNet.Roslyn.PendingAnalysis.csproj");
            
            var document = project.Documents.First(x => x.Name.Equals("ContractTestContext.cs", StringComparison.InvariantCultureIgnoreCase));

            var tree = await document.GetSyntaxTreeAsync();
            var root = tree.GetCompilationUnitRoot();

            var visitor = new TypeParameterVisitor();
            var node = visitor.Visit(root);

            var text = node.GetText();
            File.WriteAllText(document.FilePath, text.ToString());
        }
    }
}

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Roslyn.CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var codeText = @"
                using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;

                namespace TrrluujHlcdyqa
                {
                    class Program
                    {
                        static void Main(string[] args)
                        {
                            Console.WriteLine(""hello"");
                        }
                    }

                    class Foo
                    {
                        public string KiqHns { get; set; }
                    }
                }";

            var syntaxTree = CSharpSyntaxTree.ParseText(codeText);
            var root = syntaxTree.GetRoot() as CompilationUnitSyntax;

            var modelSyntaxRewriter = new ModelSyntaxRewriter();
            modelSyntaxRewriter.Visit(root);
            Console.WriteLine(JsonConvert.SerializeObject(modelSyntaxRewriter.Models));
        }
    }
}

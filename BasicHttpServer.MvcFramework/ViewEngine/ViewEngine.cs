using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace BasicHttpServer.MvcFramework.ViewEngine
{
    public class ViewEngine : IViewEngine
    {
        public string GetHtml(string templateCode, object viewModel)
        {
            var csharpCode = GenerateCSharpFromTemplate(templateCode);
            IView executableObject = GenerateExecutableCode(csharpCode, viewModel);
            var html = executableObject.ExecuteTemplate(viewModel);
            return html;
        }

        private string GenerateCSharpFromTemplate(string templateCode)
        {
            var csharpCode = @"
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using BasicHttpServer.MvcFramework.ViewEngine;

namespace ViewNamespace
{
    public class ViewClass : IView
    {
        public string ExecuteTemplate(object viewModel)
        {
            var html = new StringBuilder();

            " + GetMethodBody(templateCode) + @"

            return html.ToString();
        }
    }
}
";

            return csharpCode;
        }

        private object GetMethodBody(string templateCode)
        {
            var csharpCode = new StringBuilder();
            var stringReader = new StringReader(templateCode);
            string line;

            while ((line = stringReader.ReadLine()) != null)
            {
                if (line.TrimStart().StartsWith("@"))
                {
                    var atSignLocation = line.IndexOf("@", StringComparison.Ordinal);
                    line = line.Remove(atSignLocation);
                    csharpCode.AppendLine(line);
                }
                else if (line.TrimStart().StartsWith("{") ||
                         line.TrimStart().StartsWith("}"))
                {
                    csharpCode.AppendLine(line);
                }
                else
                {
                    csharpCode.AppendLine($"html.AppendLine(@\"{line.Replace("\"", "\"\"")}\");");
                }
            }

            return csharpCode.ToString();
        }

        private IView GenerateExecutableCode(string csharpCode, object viewModel)
        {
            var compileResult = CSharpCompilation.Create("ViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location));

            if (viewModel != null)
            {
                compileResult = compileResult.AddReferences(MetadataReference.CreateFromFile(viewModel.GetType().Assembly.Location));
            }

            var libraries = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();

            foreach (var library in libraries)
            {
                compileResult = compileResult.AddReferences(MetadataReference.CreateFromFile(Assembly.Load(library).Location));
            }

            compileResult = compileResult.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(csharpCode));

            using (var memoryStream = new MemoryStream())
            {
                var emitResult = compileResult.Emit(memoryStream);
                if (!emitResult.Success)
                {
                    return new ErrorView(emitResult.Diagnostics
                            .Where(e => e.Severity == DiagnosticSeverity.Error)
                            .Select(e => e.GetMessage()), csharpCode);
                }

                try
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var byteAssembly = memoryStream.ToArray();
                    var assembly = Assembly.Load(byteAssembly);
                    var viewType = assembly.GetType("ViewNamespace.ViewClass");
                    var instance = Activator.CreateInstance(viewType);
                    return instance as IView ?? new ErrorView(new List<string>{"Instance is null"}, csharpCode);
                }
                catch (Exception e)
                {
                    return new ErrorView(new List<string> { e.ToString() }, csharpCode);
                }

            }

            // Roslyn
            // C# -> executable -> IView -> ExecuteTemplate
        }
    }
}

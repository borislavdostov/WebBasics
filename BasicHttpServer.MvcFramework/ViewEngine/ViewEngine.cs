using System;

namespace BasicHttpServer.MvcFramework.ViewEngine
{
    public class ViewEngine : IViewEngine
    {
        public string GetHtml(string templateCode, object viewModel)
        {
            var csharpCode = GenerateCSharpFromTemplate(templateCode);
            IView executableObject = GenerateExecutableCode(csharpCode);
            var html = executableObject.ExecuteTemplate(viewModel);
            return html;
        }

        private string GenerateCSharpFromTemplate(string templateCode)
        {
            var methodBody = GetMethodBody(templateCode);
            var csharpCode = @"
using System;
using System.Text;
using System.Linq
using System.Collections.Generic;
using BasicHttpServer.MvcFramework.ViewEngine;

namespace ViewNameSpace
{
    public class ViewClass : IView
    {
        public string ExecuteTemplate(object viewModel)
        {
            var html = new StringBuilder();

            " + methodBody + @"

            return html.ToString();
        }
    }
}
";

            return csharpCode;
        }

        private object GetMethodBody(string templateCode)
        {
            return string.Empty;
        }

        private IView GenerateExecutableCode(string csharpCode)
        {
            // Roslyn
            // C# -> executable -> IView -> ExecuteTemplate
            throw new NotImplementedException();
        }
    }
}

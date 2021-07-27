using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicHttpServer.MvcFramework.ViewEngine
{
    public class ErrorView : IView
    {
        private readonly IEnumerable<string> _errors;
        private readonly string _csharpCode;

        public ErrorView(IEnumerable<string> errors, string csharpCode)
        {
            _errors = errors;
            _csharpCode = csharpCode;
        }

        public string ExecuteTemplate(object viewModel)
        {
            var html = new StringBuilder();
            html.AppendLine($"<h1>View compile {_errors.Count()} errors:</h1><ul>");
            foreach (var error in _errors)
            {
                html.AppendLine($"<li>{error}</li>");
            }

            html.AppendLine($"</ul><pre>{_csharpCode}</pre>");
            return html.ToString();
        }
    }
}

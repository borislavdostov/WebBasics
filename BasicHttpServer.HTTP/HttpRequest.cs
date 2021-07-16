using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicHttpServer.HTTP
{
    public class HttpRequest
    {
        public HttpRequest(string requestString)
        {
            Headers = new List<Header>();
            Cookies = new List<Cookie>();

            var lines = requestString.Split(new[] { HttpConstants.NewLine }, StringSplitOptions.None);
            var headerLine = lines[0];
            var headerLineParts = headerLine.Split(' ');
            Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), headerLineParts[0], true);
            Path = headerLineParts[1];


            var lineIndex = 1;
            var isInHeaders = true;
            var bodyBuilder = new StringBuilder();

            while (lineIndex < lines.Length)
            {
                var line = lines[lineIndex];
                lineIndex++;

                if (string.IsNullOrWhiteSpace(line))
                {
                    isInHeaders = false;
                    continue;
                }

                if (isInHeaders)
                {
                    Headers.Add(new Header(line));
                }
                else
                {
                    bodyBuilder.AppendLine(line);
                }
            }

            if (Headers.Any(h => h.Name.Equals(HttpConstants.RequestCookieHeader)))
            {
                var cookiesAsString = Headers.
                    FirstOrDefault(h => h.Name.Equals(HttpConstants.RequestCookieHeader))?.Value;
                var cookies = cookiesAsString.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var cookieAsString in cookies)
                {
                    Cookies.Add(new Cookie(cookieAsString));
                }
            }

            Body = bodyBuilder.ToString();
        }

        public HttpMethod Method { get; set; }
        public string Path { get; set; }
        public ICollection<Header> Headers { get; set; }
        public ICollection<Cookie> Cookies { get; set; }
        public string Body { get; set; }
    }
}

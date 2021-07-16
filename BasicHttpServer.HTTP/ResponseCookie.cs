
using System.Text;

namespace BasicHttpServer.HTTP
{
    public class ResponseCookie : Cookie
    {
        public ResponseCookie(string name, string value)
            : base(name, value)
        {
            Path = "/";
        }

        public int MaxAge { get; set; }
        public bool HttpOnly { get; set; }
        public string Path { get; set; }

        public override string ToString()
        {
            var responseCookieBuilder = new StringBuilder();

            responseCookieBuilder.Append($"{Name}={Value}; Path={Path};");

            if (MaxAge != 0)
            {
                responseCookieBuilder.Append($" Max-Age={MaxAge};");
            }

            if (HttpOnly)
            {
                responseCookieBuilder.Append(" HttpOnly;");
            }

            return responseCookieBuilder.ToString();
        }
    }
}

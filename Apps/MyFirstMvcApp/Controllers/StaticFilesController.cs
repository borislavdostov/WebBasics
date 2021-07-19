using System.IO;
using BasicHttpServer.HTTP;

namespace MyFirstMvcApp.Controllers
{
    public class StaticFilesController
    {
        public HttpResponse Favicon(HttpRequest request)
        {
            var fileBytes = File.ReadAllBytes("wwwroot/favicon.ico");
            var response = new HttpResponse("image/vnd.microsoft.icon", fileBytes);
            return response;
        }
    }
}

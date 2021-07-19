using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;

namespace MyFirstMvcApp.Controllers
{
    public class CardsController : Controller
    {
        public HttpResponse Add(HttpRequest request)
        {
            return View();
        }

        public HttpResponse All(HttpRequest request)
        {
            return View();
        }

        public HttpResponse Collection(HttpRequest request)
        {
            return View();
        }

        public HttpResponse BootstrapCss(HttpRequest arg)
        {
            throw new System.NotImplementedException();
        }

        public HttpResponse CustomCss(HttpRequest arg)
        {
            throw new System.NotImplementedException();
        }

        public HttpResponse CustomJs(HttpRequest arg)
        {
            throw new System.NotImplementedException();
        }

        public HttpResponse BootstrapJs(HttpRequest arg)
        {
            throw new System.NotImplementedException();
        }
    }
}

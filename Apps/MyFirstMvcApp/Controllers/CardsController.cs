using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;

namespace MyFirstMvcApp.Controllers
{
    public class CardsController : Controller
    {
        public HttpResponse Add()
        {
            return View();
        }

        public HttpResponse All()
        {
            return View();
        }

        public HttpResponse Collection()
        {
            return View();
        }
    }
}

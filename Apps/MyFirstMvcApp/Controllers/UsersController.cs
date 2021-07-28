using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;

namespace MyFirstMvcApp.Controllers
{
    public class UsersController : Controller
    {
        public HttpResponse Login(HttpRequest request)
        {
            return View();

        }

        public HttpResponse Register(HttpRequest request)
        {
            return View();
        }

        [HttpPost]
        public HttpResponse DoLogin(HttpRequest request)
        {
            //TODO: Read data
            //TODO: Check user
            //TODO: Log user
            return Redirect("/");
        }
    }
}

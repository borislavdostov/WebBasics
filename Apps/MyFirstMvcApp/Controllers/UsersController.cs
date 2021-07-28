using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;

namespace MyFirstMvcApp.Controllers
{
    public class UsersController : Controller
    {
        public HttpResponse Login()
        {
            return View();

        }

        public HttpResponse Register()
        {
            return View();
        }

        [HttpPost]
        public HttpResponse DoLogin()
        {
            //TODO: Read data
            //TODO: Check user
            //TODO: Log user
            return Redirect("/");
        }
    }
}

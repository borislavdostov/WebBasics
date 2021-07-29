using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;

namespace BattleCards.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (IsUserSignedIn())
            {
                return Redirect("/Cards/All");
            }

            return View();
        }
    }
}

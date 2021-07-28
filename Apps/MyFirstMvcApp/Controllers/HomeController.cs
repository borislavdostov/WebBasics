using System;
using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;
using MyFirstMvcApp.ViewModels;

namespace MyFirstMvcApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index(HttpRequest request)
        {
            var viewModel = new IndexViewModel
            {
                CurrentYear = DateTime.UtcNow.Year,
                Message = "Welcome to Battle Cards"
            };

            return View(viewModel);
        }

        public HttpResponse About(HttpRequest request)
        {
            return View();
        }
    }
}

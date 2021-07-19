using System.Collections.Generic;
using System.Threading.Tasks;
using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;
using MyFirstMvcApp.Controllers;

namespace MyFirstMvcApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var routeTable = new List<Route>
            {
                new Route("/", new HomeController().Index),
                new Route("/users/login", new UsersController().Login),
                new Route("/users/register", new UsersController().Register),
                new Route("/cards/all", new CardsController().All),
                new Route("/cards/add", new CardsController().Add),
                new Route("/cards/collection", new CardsController().Collection),

                new Route("/favicon.ico", new StaticFilesController().Favicon),
                new Route("/css/bootstrap.min.css", new StaticFilesController().BootstrapCss),
                new Route("/css/custom.css", new StaticFilesController().CustomCss),
                new Route("/js/custom.js", new StaticFilesController().CustomJs),
                new Route("/js/bootstrap.bundle.min.js", new StaticFilesController().BootstrapJs)
        };




            await Host.CreateHostAsync(routeTable);
        }
    }
}
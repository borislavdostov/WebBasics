using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;
using BattleCards.Data;
using BattleCards.ViewModels;
using System.Linq;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        public HttpResponse Add()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            return View();
        }

        [HttpPost("/Cards/Add")]
        public HttpResponse DoAdd()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            var dbContext = new ApplicationDbContext();

            if (Request.FormData["name"].Length < 5)
            {
                return Error("Name should be atleast 5 characters long!");
            }

            dbContext.Cards.Add(new Card
            {
                Attack = int.Parse(Request.FormData["attack"]),
                Health = int.Parse(Request.FormData["health"]),
                Description = Request.FormData["description"],
                Name = Request.FormData["name"],
                ImageUrl = Request.FormData["image"],
                Keyword = Request.FormData["keyword"],
            });

            dbContext.SaveChanges();

            return Redirect("/Cards/All");
        }

        public HttpResponse All()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            var db = new ApplicationDbContext();
            var cardsViewModel = db.Cards.Select(c => new CardViewModel
            {
                Name = c.Name,
                Description = c.Description,
                Attack = c.Attack,
                Health = c.Health,
                ImageUrl = c.ImageUrl,
                Type = c.Keyword
            }).ToList();

            return View(cardsViewModel);
        }

        public HttpResponse Collection()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            return View();
        }
    }
}

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
            return View();
        }

        [HttpPost("/Cards/Add")]
        public HttpResponse DoAdd()
        {
            var dbContext = new ApplicationDbContext();

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

            return Redirect("/");
        }

        public HttpResponse All()
        {
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

            return View(new AllCardsViewModel { Cards = cardsViewModel });
        }

        public HttpResponse Collection()
        {
            return View();
        }
    }
}

using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;
using BattleCards.Data;
using BattleCards.ViewModels;
using System.Linq;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext db;

        public CardsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public HttpResponse Add()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            return View();
        }

        [HttpPost("/Cards/Add")]
        public HttpResponse DoAdd(string attack, string health,
            string description, string name, string image, string keyword)
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            if (Request.FormData["name"].Length < 5)
            {
                return Error("Name should be atleast 5 characters long!");
            }

            db.Cards.Add(new Card
            {
                Attack = int.Parse(attack),
                Health = int.Parse(health),
                Description = description,
                Name = name,
                ImageUrl = image,
                Keyword = keyword
            });

            db.SaveChanges();

            return Redirect("/Cards/All");
        }

        public HttpResponse All()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

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

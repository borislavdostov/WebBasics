using System;
using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;
using BattleCards.ViewModels.Cards;
using BattleCards.Services;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private readonly ICardsService cardsService;

        public CardsController(ICardsService cardsService)
        {
            this.cardsService = cardsService;
        }

        public HttpResponse Add()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            return View();
        }

        [HttpPost]
        public HttpResponse Add(AddCardInputModel model)
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 5 || model.Name.Length > 15)
            {
                return Error("Name should be between 5 and 15 characters long!");
            }

            if (string.IsNullOrWhiteSpace(model.Image))
            {
                return Error("The image is required!");
            }

            if (!Uri.TryCreate(model.Image, UriKind.Absolute, out _))
            {
                return Error("Image url should be valid!");
            }

            if (string.IsNullOrWhiteSpace(model.Keyword))
            {
                return Error("Keyword is required!");
            }

            if (model.Attack < 0)
            {
                return Error("Attack should be non-negative integer!");
            }

            if (model.Health < 0)
            {
                return Error("Health should be non-negative integer!");
            }

            if (string.IsNullOrWhiteSpace(model.Description) || model.Description.Length > 200)
            {
                return Error("Description is required and its length should be at most to 200 characters!");
            }

            var cardId = cardsService.AddCard(model);
            cardsService.AddCardToUserCollection(GetUserId(), cardId);

            return Redirect("/Cards/All");
        }

        public HttpResponse All()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            var cardsViewModel = cardsService.GetAll();
            return View(cardsViewModel);
        }

        public HttpResponse Collection()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            var cardsViewModel = cardsService.GetByUserId(GetUserId());
            return View(cardsViewModel);
        }

        public HttpResponse AddToCollection(int cardId)
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            cardsService.AddCardToUserCollection(GetUserId(), cardId);
            return Redirect("/Cards/All");
        }

        public HttpResponse RemoveFromCollection(int cardId)
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            cardsService.RemoveCardToUserCollection(GetUserId(), cardId);
            return Redirect("/Cards/Collection");
        }
    }
}

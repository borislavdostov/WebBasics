using System.Collections.Generic;
using System.Linq;
using BattleCards.Data;
using BattleCards.ViewModels.Cards;

namespace BattleCards.Services
{
    public class CardsService : ICardsService
    {
        private readonly ApplicationDbContext db;

        public CardsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public int AddCard(AddCardInputModel input)
        {
            var card = new Card
            {
                Attack = input.Attack,
                Health = input.Health,
                Description = input.Description,
                Name = input.Name,
                ImageUrl = input.Image,
                Keyword = input.Keyword
            };

            db.Cards.Add(card);
            db.SaveChanges();
            return card.Id;
        }

        public IEnumerable<CardViewModel> GetAll()
        {
            return db.Cards.Select(c => new CardViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Attack = c.Attack,
                Health = c.Health,
                ImageUrl = c.ImageUrl,
                Type = c.Keyword
            }).ToList();
        }

        public IEnumerable<CardViewModel> GetByUserId(string userId)
        {
            return db.UserCards.Where(uc => uc.UserId == userId)
                .Select(c => new CardViewModel
                {
                    Id = c.CardId,
                    Attack = c.Card.Attack,
                    Health = c.Card.Health,
                    Name = c.Card.Name,
                    Description = c.Card.Description,
                    ImageUrl = c.Card.ImageUrl,
                    Type = c.Card.Keyword
                }).ToList();
        }

        public void AddCardToUserCollection(string userId, int cardId)
        {
            if (db.UserCards.Any(uc => uc.UserId == userId && uc.CardId == cardId))
            {
                return;
            }

            db.UserCards.Add(new UserCard
            {
                UserId = userId,
                CardId = cardId
            });

            db.SaveChanges();
        }

        public void RemoveCardToUserCollection(string userId, int cardId)
        {
            var userCard = db.UserCards.FirstOrDefault(uc => uc.UserId == userId && uc.CardId == cardId);

            if (userCard == null)
            {
                return;
            }

            db.UserCards.Remove(userCard);
            db.SaveChanges();
        }
    }
}

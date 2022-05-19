using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLogic.Players
{
    internal class Bot : PlayerBase
    {
        public Bot(GameSession session) : base(session) { }

        protected override void AddCards(IEnumerable<Card> cards)
        {
            foreach (Card card in cards)
            {
                card.LoadImage(Game.ReturnsImage($"Resources/{Card.CARD_BACK_IMAGE_NAME}"));
                _cards.Add(card);
            }
        }
    }
}

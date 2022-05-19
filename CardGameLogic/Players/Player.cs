using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CardGameLogic.Players
{
    internal class Player : PlayerBase
    {
        public Player(GameSession session) : base(session) { }

        protected override void AddCards(IEnumerable<Card> cards)
        {
            foreach (Card card in cards)
            {
                card.LoadImage(Game.ReturnsImage($"Resources/{card.ImageName}"));
                card.MouseUp += CardMouseUpHandler;
                _cards.Add(card);
            }
        }

        private void CardMouseUpHandler(object sender, MouseEventArgs e)
        {
            if (sender is Card card)
                if(_session.TryDropCard(this, card))
                    card.MouseUp -= CardMouseUpHandler;
        }
    }
}

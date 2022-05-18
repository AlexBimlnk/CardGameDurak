using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CardGameLogic
{
    internal class Player : IPlayer
    {
        private readonly GameSession _session;
        private List<Card> _cards = new List<Card>(12);

        public Player(GameSession session)
        {
            _session = session;

            int countCardInStartGame = 6;
            AddCards(_session.TryGetCards(countCardInStartGame));
        }

        public event EventHandler<Card> OnPlayerCardDropped;

        public int CountCards => _cards.Count;

        private void AddCards(IEnumerable<Card> cards)
        {
            foreach (Card card in cards)
            {
                card.MouseUp += CardMouseDownHandler;
                _cards.Add(card);
            }
        }

        private void CardMouseDownHandler(object sender, MouseEventArgs e)
        {
            if (sender is Card card)
            {

                OnPlayerCardDropped?.Invoke(this, card);
            }
        }

        private void TryDrop(object sender, MouseEventArgs e)
        {
            if (sender is Card card)
            {

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLogic.Players
{
    internal abstract class PlayerBase : IPlayer
    {
        private const int DEFAULT_SIZE_HAND = 12;
        private const int COUNT_CARDS_IN_START_GAME = 6;
        protected readonly List<Card> _cards = new List<Card>(DEFAULT_SIZE_HAND);
        protected readonly GameSession _session;

        public PlayerBase(GameSession session)
        {
            _session = session;
            AddCards(_session.TryGetCards(this, COUNT_CARDS_IN_START_GAME));
        }

        public int CountCards => _cards.Count;

        protected abstract void AddCards(IEnumerable<Card> cards);
    }
}

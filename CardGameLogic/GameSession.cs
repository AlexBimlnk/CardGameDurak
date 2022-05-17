using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CardGameLogic
{
    public sealed class GameSession
    {
        private const int MAX_DESK_CAPACITY = 36;
        private List<Card> _deck = new List<Card>(MAX_DESK_CAPACITY);

        private IPlayer _turnPlayer;

        private bool _deskIsClear = true;

        //Количество "подкинутых для побития карт" во время хода
        private int _countAddedCardOnDesk = 0;

        public event EventHandler EndGameEvent;

        public GameSession(SessionId sessionId, GameMode gameMode, Canvas canvasTemplate)
        {
            Id = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
            GameMode = gameMode;
            GameWindow = canvasTemplate;
            Players = gameMode == GameMode.MultiPlayer
                      ? new List<IPlayer>() { new Player(), new Player() }
                      : new List<IPlayer>() { new Player(), new Bot() };
            _turnPlayer = Players[new Random().Next(0,2)];
            TrumpSuit = Game.Suits[new Random().Next(0, 4)];
        }

        public SessionId Id { get; }
        public GameMode GameMode { get; }
        public Canvas GameWindow { get; }
        public IReadOnlyList<IPlayer> Players { get; }
        public Suit TrumpSuit { get; }

        public int ZIndex { get; private set; } = 1;

        private void CheckWin()
        {
            if (_deck.Count == 0 && Players.Any(player => player.CountCards == 0))
            {
                if (Players.All(player => player.CountCards == 0))
                    MessageBox.Show("Ничья", "Игра закончена", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    IPlayer winPlayer = Players.Where(x => x.CountCards > 0).Single();
                    MessageBox.Show($"Выиграл игрок {winPlayer}", "Игра закончена", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                EndGameEvent?.Invoke();
            }
        }

        public void Start()
        {
            //Заполнение колоды
            var random = new Random();
            int minCardRank = 6;
            int maxCardRank = 14;
            foreach (var suit in Game.Suits)
            {
                for (int rank = minCardRank; rank <= maxCardRank; rank++)
                {
                    _deck.Add(
                        new Card(suit,
                                 rank,
                                 $"{suit}{rank}.jpg")
                                 { IsTrumpCard = suit == TrumpSuit ? true : false });
                }
            }

            Game.ConfigureGameObject(this);
        }

        public void AddContorl(UIElement element, int left, int top)
        {
            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);
            Canvas.SetZIndex(element, ZIndex);
            GameWindow.Children.Add(element);
            ZIndex++;
        }
    }
}
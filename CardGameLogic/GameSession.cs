using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using CardGameLogic.Enums;
using CardGameLogic.Players;

namespace CardGameLogic
{

    public sealed class GameSession
    {
        private const int MAX_DECK_CAPACITY = 36;
        private const int MAX_BOARD_CAPACITY = 12;

        private Point? _movePoint;

        private List<Card> _deck = new List<Card>(MAX_DECK_CAPACITY);
        private List<Card> _board = new List<Card>(MAX_BOARD_CAPACITY);

        private Random _random = new Random();
        private bool _isGameStarted = false;

        private IPlayer _turnPlayer;
        private Bot _botPlayer;

        
        public GameSession(SessionId sessionId, GameMode gameMode, Canvas canvasTemplate)
        {
            Id = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
            GameMode = gameMode;
            GameWindow = canvasTemplate;
            _botPlayer = new Bot(this);
            Players = gameMode == GameMode.MultiPlayer
                      ? new List<IPlayer>() { new Player(this), new Player(this) }
                      : new List<IPlayer>() { new Player(this), _botPlayer };
            _turnPlayer = Players[_random.Next(0, 2)];
            TrumpSuit = Game.Suits[_random.Next(0, 4)];
            _isGameStarted = true;
        }


        public SessionId Id { get; }
        public GameMode GameMode { get; }
        public Canvas GameWindow { get; }
        public IReadOnlyList<IPlayer> Players { get; }
        public Suit TrumpSuit { get; }
        public int ZIndex { get; private set; } = 1;


        public event EventHandler EndGameEvent;


        #region Обработчики событий для карты

        private void MouseEnterFunc(object sender, MouseEventArgs e)
        {
            if (sender is Card card)
            {
                card.ChangeBorder(Brushes.Purple, 2);
                card.ChangeAnimation(Game.MARGIN_TOP_HAND, Game.MARGIN_TOP_HAND - card.Height / 2, TimeSpan.FromSeconds(0.3));
                card.BeginAnimation(Canvas.TopProperty, card.Animation);
            }
        }
        private void MouseLeaveFunc(object sender, MouseEventArgs e)
        {
            if (sender is Card card)
            {
                card.ChangeBorder(Brushes.Black, 1);
                card.ChangeAnimation(Canvas.GetTop(card), Game.MARGIN_TOP_HAND, TimeSpan.FromSeconds(0.3));
                card.BeginAnimation(Canvas.TopProperty, card.Animation);
            }
        }
        private void MouseDownFunc(object sender, MouseEventArgs e)
        {
            if (sender is Card card)
            {
                _movePoint = e.GetPosition(card);
                card.CaptureMouse();
                card.BeginAnimation(Canvas.TopProperty, null);

                Game.ColorHandField = Brushes.NavajoWhite;
                card.ZIndex = Canvas.GetZIndex(card);
                Canvas.SetZIndex(card, 100);
            }
        }
        public void MouseMoveFunc(object sender, MouseEventArgs e)
        {
            if (sender is Card card)
            {
                if (_movePoint is null)
                    return;
                var p = e.GetPosition(Game.GameWindow) - (Vector)_movePoint.Value;
                Canvas.SetLeft(card, p.X);
                Canvas.SetTop(card, p.Y);
            }
        }
        //Уедет частично в player
        private void MouseUpFunc(object sender, MouseEventArgs e)
        {
            if (sender is Card card)
            {
                _movePoint = null;
                card.ReleaseMouseCapture();

                Game.ColorHandField = null;
                Canvas.SetZIndex(card, ZIndex);
                //Если хотим оставить в руке
                if (Canvas.GetTop(card) + card.Height >= Game.HAND_FIELD_TOP)
                {
                    //ToDo: Оставили карту в руке
                }
                else
                {
                    //Todo: Посылаем сигнал о том, что хотим положить карту,
                    //предварительно проверив ход (событие на которое подписан игрок)
                }
                //else if (Game.CanDrop(this))
                //{
                //    if (!Game.TurnIsEnemy)
                //        Game.CountAddedCardOnDesk++;
                //    IsOnDesk = true;
                //    ChangeBorder(Brushes.Black, 1);
                //    Game.SetCardOnDesk(this, Game.TurnIsEnemy);
                //    Game.UpdateHand();
                //    Game.EnemyMoves();
                //}
                //else
                //{
                //    Game.UpdateHand();
                //    MessageBox.Show("Сейчас невозможно положить эту карту на стол.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
            }
        }
        #endregion


        private void ViewMessage(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool CanDropCardWhenPlayerAttack(Card card)
        {
            if (_board.Count == 0)
                return true;

            return _board.Any(x => x.Rank == card.Rank);
        }
        private bool CanDropCardWhenPlayerDefence(Card card, out Card closedCard)
        {
            closedCard = _board.Where(x => x.Owner == _turnPlayer)
                               .Where(x => !x.IsCloseOnDesk)
                               .Where(x => x.Suit == card.Suit)
                               .Where(x => x.Rank < card.Rank)
                               .FirstOrDefault();

            return !(closedCard is null);
        }

        

        private void CheckWin()
        {
            if (_deck.Count == 0 && Players.Any(player => player.CountCards == 0))
            {
                if (Players.All(player => player.CountCards == 0))
                    ViewMessage("Игра закончена, ничья.");
                else
                {
                    IPlayer winPlayer = Players.Where(x => x.CountCards > 0).Single();
                    ViewMessage($"Игра закончена, выиграл игрок: {winPlayer}");
                }
                EndGameEvent?.Invoke(this, null);
            }
        }

        private void ChangeCardEvents(Card card, CardEventsPolicy policy)
        {
            if (policy == CardEventsPolicy.Manage)
            {
                card.MouseEnter += new MouseEventHandler(MouseEnterFunc);
                card.MouseLeave += new MouseEventHandler(MouseLeaveFunc);
                card.MouseDown += new MouseButtonEventHandler(MouseDownFunc);
                card.MouseUp += new MouseButtonEventHandler(MouseUpFunc);
                card.MouseMove += new MouseEventHandler(MouseMoveFunc);
            }
            else if (policy == CardEventsPolicy.ReadOnly)
            {
                card.MouseEnter -= new MouseEventHandler(MouseEnterFunc);
                card.MouseLeave -= new MouseEventHandler(MouseLeaveFunc);
                card.MouseDown -= new MouseButtonEventHandler(MouseDownFunc);
                card.MouseUp -= new MouseButtonEventHandler(MouseUpFunc);
                card.MouseMove -= new MouseEventHandler(MouseMoveFunc);
            }
        }

        public void Start()
        {
            //Заполнение колоды
            int minCardRank = 6;
            int maxCardRank = 14;
            foreach (var suit in Game.Suits)
            {
                for (int rank = minCardRank; rank <= maxCardRank; rank++)
                {
                    var card = new Card(suit, rank, $"{suit}{rank}.jpg") 
                    { 
                        IsTrumpCard = suit == TrumpSuit ? true : false 
                    };

                    ChangeCardEvents(card, CardEventsPolicy.Manage);

                    _deck.Add(card);
                }
            }

            Game.ConfigureGameObject(this);

            if (GameMode == GameMode.SinglePlayer)
            {

            }
            else
            {

            }
        }

        public void AddContorl(UIElement element, int left, int top)
        {
            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);
            Canvas.SetZIndex(element, ZIndex);
            GameWindow.Children.Add(element);
            ZIndex++;
        }

        public IEnumerable<Card> TryGetCards(IPlayer player, int countCards)
        {
            IEnumerable<Card> _()
            {
                for (int i = 0; i < countCards; i++)
                {
                    if (_deck.Count > 0)
                    {
                        Card card = _deck[_random.Next(0, _deck.Count)];
                        card.Owner = player;
                        yield return card;
                    }
                    else
                        break;
                }
            };

            if (_isGameStarted)
            {
                //Todo: проверки на возможность взять карты

                return _();
            }

            return _();
        }

        public bool TryDropCard(IPlayer player, Card card)
        {
            if (Canvas.GetTop(card) + card.Height >= Game.HAND_FIELD_TOP || 
                _board.Count == MAX_BOARD_CAPACITY)
                return false;

            Card closedCard = default;
            bool isCanDropped = player == _turnPlayer
                                ? CanDropCardWhenPlayerAttack(card)
                                : CanDropCardWhenPlayerDefence(card, out closedCard);


            //
            if (isCanDropped)
            {
                _board.Add(card);

                if (closedCard != null)
                    closedCard.IsCloseOnDesk = true;

                if(GameMode == GameMode.SinglePlayer)
                {

                }
                else
                {
                    //Todo: MultiPlayer
                }
            }

            if (!isCanDropped)
                ViewMessage("Сейчас эту карту положить невозможно.");

            return isCanDropped;
        }

        public void Turn(IPlayer player)
        {

        }
    }
}
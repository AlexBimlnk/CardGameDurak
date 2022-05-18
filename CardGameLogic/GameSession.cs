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
        private const int MAX_DESK_CAPACITY = 36;
        private List<Card> _deck = new List<Card>(MAX_DESK_CAPACITY);
        private Random _random = new Random();
        private bool _isGameStarted = false;

        private IPlayer _turnPlayer;

        private Point? _movePoint;

        private bool _deskIsClear = true;


        //Количество "подкинутых для побития карт" во время хода
        private int _countAddedCardOnDesk = 0;

        public GameSession(SessionId sessionId, GameMode gameMode, Canvas canvasTemplate)
        {
            Id = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
            GameMode = gameMode;
            GameWindow = canvasTemplate;
            Players = gameMode == GameMode.MultiPlayer
                      ? new List<IPlayer>() { new Player(this), new Player(this) }
                      : new List<IPlayer>() { new Player(this), new Bot() };
            ConfigurePlayerEvents(Players);
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
        
        private void ConfigurePlayerEvents(IReadOnlyList<IPlayer> players)
        {
            foreach (var player in players)
                player.OnPlayerCardDropped += PlayerCardDroppped;
        }

        private void PlayerCardDroppped(object sender, Card card)
        {
            if (sender is IPlayer player && player == _turnPlayer)
            {
                //Todo: Player try drop card
            }
        }

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
                if (Canvas.GetTop(card) + card.Height >= Game.HAND_FIELD_TOP || _countAddedCardOnDesk == 6)
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
        #endregion

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
                EndGameEvent?.Invoke(this, null);
            }
        }
        private Card ConfigureCard(Card card)
        {
            card.MouseEnter += new MouseEventHandler(MouseEnterFunc);
            card.MouseLeave += new MouseEventHandler(MouseLeaveFunc);
            card.MouseDown += new MouseButtonEventHandler(MouseDownFunc);
            card.MouseMove += new MouseEventHandler(MouseMoveFunc);
            card.MouseUp += new MouseButtonEventHandler(MouseUpFunc);

            return card;
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
                    Card card = ConfigureCard(new Card(suit, rank, $"{suit}{rank}.jpg")
                        { IsTrumpCard = suit == TrumpSuit ? true : false }
                    );
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

        public IEnumerable<Card> TryGetCards(int countCards)
        {
            IEnumerable<Card> _()
            {
                for (int i = 0; i < countCards; i++)
                {
                    if (_deck.Count > 0)
                        yield return _deck[_random.Next(0, _deck.Count)];
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

        public void Turn(IPlayer player)
        {

        }
    }
}
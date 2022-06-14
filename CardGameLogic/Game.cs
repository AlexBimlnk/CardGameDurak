using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using CardGameLogic.Enums;
using CardGameLogic.Players;

namespace CardGameLogic
{
    public static class Game
    {
        /*
         * Для того, чтобы убрать отображение "рубашки" у бота
         * Нужно закомментить строку 207 и строку 383
         */

        #region Отступы, константы
        //В руках
        public const int MARGIN_LEFT = 10;
        public const int MARGIN_TOP_ENEMY = -90;
        public const int MARGIN_TOP_HAND = 599;

        //На столе
        public const int DESK_MARGIN_LEFT = 108;                   //Констатное начало для 1 карты на столе
        public static int _deskLeftVariable = DESK_MARGIN_LEFT;    //Меняется в зависимости от кол-ва карт
        public const int DESK_MARGIN_TOP = 258;
        public const int DESK_INTERVAL = 145;

        //Положение области, показывающей границы поля карт в руке
        public const int HAND_FIELD_LEFT = 0;
        public const int HAND_FIELD_TOP = 570;

        //Положение правой панели (кнопки, счетчик, коз. масть на столе)
        public const int RIGHT_PANEL_MARGIN_LEFT = 1180;
        public const int RIGHT_PANEL_MARGIN_TOP = 270;

        #endregion

        private static IReadOnlyList<Suit> _suitNamesList = new List<Suit> { Suit.Clubs, Suit.Diamonds,
                                                                             Suit.Hearts, Suit.Spades };

        private static Label _handFieldLabel = new Label();
        private static TextBlock _countDeckTextBlock = new TextBlock();

        private static List<GameSession> _sessions = new List<GameSession>();

        private static List<Card> _deck = new List<Card>();
        private static List<Card> _handList = new List<Card>();
        private static List<Card> _enemyList = new List<Card>();

        private static Player _turnPlayer = Player.Human;
        private static bool _deskIsClear = true;

        private static int _zIndex = 1;
        
        //Количество "подкинутых для побития карт" во время хода
        public static int CountAddedCardOnDesk = 0;


        public static IReadOnlyList<Suit> Suits => _suitNamesList;

        public static Canvas GameWindow { get; set; }
        public static bool TurnIsEnemy => _turnPlayer == Player.Bot ? true : false;
        public static SolidColorBrush ColorHandField
        {
            set { _handFieldLabel.Background = value; }
            get { return (SolidColorBrush)_handFieldLabel.Background; }
        }

        public static BitmapFrame ReturnsImage(string path)
        {
            Uri resourceUri = new Uri(path, UriKind.Relative);
            StreamResourceInfo stream_info = Application.GetResourceStream(resourceUri);
            BitmapFrame temp = BitmapFrame.Create(stream_info.Stream);

            return temp;
        }

        public static void ConfigureGameObject(GameSession session)
        {
            //field label
            var label = new Label();
            label.Height = 180;
            label.Width = 1272;
            session.AddContorl(label, HAND_FIELD_LEFT, HAND_FIELD_TOP);

            //Trump image
            var trumpImage = new Image();
            trumpImage.Source = ReturnsImage($"Resources/{session.TrumpSuit}Sym.jpg");
            trumpImage.Width = 70;
            trumpImage.Height = 70;
            session.AddContorl(trumpImage, RIGHT_PANEL_MARGIN_LEFT, RIGHT_PANEL_MARGIN_TOP);

            //Card counter
            var countDeckTextBlock = new TextBlock();
            countDeckTextBlock.Width = 70;
            countDeckTextBlock.Height = 70;
            countDeckTextBlock.Foreground = Brushes.Aqua;
            countDeckTextBlock.FontSize = 30;
            countDeckTextBlock.TextAlignment = TextAlignment.Center;
            session.AddContorl(countDeckTextBlock, RIGHT_PANEL_MARGIN_LEFT, RIGHT_PANEL_MARGIN_TOP + 70);

            //Button "Bito"
            Button btnBito = new Button();
            btnBito.Width = 80;
            btnBito.Height = 70;
            btnBito.Foreground = Brushes.Black;
            btnBito.Background = Brushes.NavajoWhite;
            btnBito.FontSize = 30;
            btnBito.Content = "Бито";
            btnBito.Click += new RoutedEventHandler(BtnBitoClick);
            session.AddContorl(btnBito, RIGHT_PANEL_MARGIN_LEFT, RIGHT_PANEL_MARGIN_TOP + 120);

            //Button "Take"
            Button btnTake = new Button();
            btnTake.Width = 80;
            btnTake.Height = 70;
            btnTake.Foreground = Brushes.Black;
            btnTake.Background = Brushes.NavajoWhite;
            btnTake.FontSize = 30;
            btnTake.Content = "Взять";
            btnTake.Click += new RoutedEventHandler(BtnTakeClick);
            session.AddContorl(btnTake, RIGHT_PANEL_MARGIN_LEFT, RIGHT_PANEL_MARGIN_TOP + 190);
        }

        public static GameSession StartNewSession(GameMode gameMode)
        {
            var session = new GameSession(new SessionId(_sessions.Count + 1), gameMode, GameWindow);
            _sessions.Add(session);
            session.Start();
            return session;
        }



        public static void SetCardOnDesk(Card element, bool forDefense = false)
        {
            _deskIsClear = false;
            if (forDefense)
            {
                SetCanvasPosition(element, _deskLeftVariable - DESK_INTERVAL, DESK_MARGIN_TOP - 90);
                if (_turnPlayer == Player.Bot)
                    ChangeCardEvents(element, true);
            }
            else
            {
                SetCanvasPosition(element, _deskLeftVariable, DESK_MARGIN_TOP);
                _deskLeftVariable += DESK_INTERVAL;
                if (_turnPlayer == Player.Human)
                    ChangeCardEvents(element, true);
                UpdateHand(_turnPlayer);
            }
        }

        public static void UpdateHand(Player player = Player.Human)
        {
            int step;
            int cardCount = player == Player.Bot ? _enemyList.Count : _handList.Count;

            if (cardCount <= 9)
                step = 140;
            else if (cardCount <= 12)
                step = 100;
            else if (cardCount <= 18)
                step = 70;
            else if (cardCount <= 25)
                step = 50;
            else
                step = 35;


            int leftPos = MARGIN_LEFT;
            int topPos = player == Player.Bot ? MARGIN_TOP_ENEMY : MARGIN_TOP_HAND;
            byte zIndex = 50;
            foreach (Card i in player == Player.Bot ? _enemyList : _handList)
                if (!i.IsOnDesk)
                {
                    Canvas.SetZIndex(i, zIndex);
                    SetCanvasPosition(i, leftPos, topPos);
                    leftPos += step;
                    zIndex++;
                }
        }

        //-------------------------------------------------------------------------


        private static void DropCard(List<Card> cardList, Player player = Player.Human)
        {
            Random rnd = new Random();

            while (cardList.Count < 6 && _deck.Count > 0)
            {
                int cardIndex = rnd.Next(0, _deck.Count);
                Card card = _deck[cardIndex];

                card.LoadImage(card.ImageName);

                if (player == Player.Bot)
                {
                    card.LoadImage(Card.CARD_BACK_IMAGE_NAME);
                    ChangeCardEvents(card, true);
                }

                else
                    card.IsHaveInHand = true;

                AddContorl(card, MARGIN_LEFT, player == Player.Bot ? MARGIN_TOP_ENEMY : MARGIN_TOP_HAND, ref _zIndex);

                cardList.Add(card);
                _deck.RemoveAt(cardIndex);
            }
            
            _countDeckTextBlock.Text = _deck.Count.ToString();

            UpdateHand(player);
        }

        //Передача хода
        private static void Turn()
        {
            _deskLeftVariable = DESK_MARGIN_LEFT;
            _turnPlayer = _turnPlayer == Player.Bot ? Player.Human : Player.Bot;
            CountAddedCardOnDesk = 0;
            ClearDesk();
            CheckWin();
            if (_turnPlayer == Player.Bot)
            {
                DropCard(_handList);
                DropCard(_enemyList, Player.Bot);
                EnemyMoves();
            }
            else
            {
                DropCard(_enemyList, Player.Bot);
                DropCard(_handList);
            }
        }


        #region -------------EnemyLogic----------------

        public static void EnemyMoves()
        {
            //Противник ходит
            if (_turnPlayer == Player.Bot)
            {
                int index = -1;
                
                //Если доска пустая
                if (_deskIsClear)
                {
                    int indexOptimalNormal = -1;
                    int indexOptimalTrump = -1;
                    for (int j = 0; j < _enemyList.Count; j++)
                    {
                        if (_enemyList[j].IsTrumpCard && (indexOptimalTrump == -1 ||
                           _enemyList[j].Rank < _enemyList[indexOptimalTrump].Rank))
                            indexOptimalTrump = j;
                        if (!_enemyList[j].IsTrumpCard && (indexOptimalNormal == -1 ||
                            _enemyList[j].Rank < _enemyList[indexOptimalNormal].Rank))
                            indexOptimalNormal = j;
                    }
                    index = indexOptimalNormal == -1 ? indexOptimalTrump : indexOptimalNormal;
                }
                //Подкидывает карты
                else
                    for (int j = 0; j < _enemyList.Count; j++)
                    {
                        if (CountAddedCardOnDesk == 6)
                            break;
                        if (!_enemyList[j].IsOnDesk && !_enemyList[j].IsTrumpCard)
                        {
                            bool find = false;
                            foreach(Card i in _enemyList)
                                if (i.IsOnDesk && _enemyList[j].Rank == i.Rank)
                                {
                                    index = j;
                                    find = true;
                                    break;
                                }
                            if (!find)
                                foreach(Card i in _handList)
                                    if (i.IsOnDesk && _enemyList[j].Rank == i.Rank)
                                    {
                                        index = j;
                                        break;
                                    }
                        }
                    }

                //Если нашли карту, которую можно подкинуть - подкидываем
                if (index != -1)
                {
                    _enemyList[index].IsOnDesk = true;
                    SetCardOnDesk(_enemyList[index]);
                    _enemyList[index].LoadImage(_enemyList[index].ImageName);
                    CountAddedCardOnDesk++;
                }
                else
                {
                    MessageBox.Show("Бито.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    Turn();     //Если подкинуть больше нечего - передаем ход
                }

            }
            //Противник отбивается
            else
            {
                int i = 0;
                for (;i<_handList.Count; i++)
                {
                    if (_handList[i].IsOnDesk &&
                       !_handList[i].IsCloseOnDesk)
                    {
                        if (_handList[i].IsTrumpCard)
                            break;
                        bool findNormal = false;
                        bool findTrump = false;
                        int indexOptimalNormal = -1;
                        int indexOptimalTrump = -1;
                        for (int j = 0; j < _enemyList.Count; j++)
                            if (!_enemyList[j].IsOnDesk)     //Если карта в руке
                            {
                                if (_enemyList[j].IsTrumpCard)
                                {
                                    findTrump = true;
                                    if (indexOptimalTrump == -1)
                                        indexOptimalTrump = j;
                                    else if (_enemyList[j].Rank <= _enemyList[indexOptimalTrump].Rank)
                                        indexOptimalTrump = j;
                                }
                                else if (_enemyList[j].Suit == _handList[i].Suit &&
                                        _enemyList[j].Rank > _handList[i].Rank)
                                {
                                    findNormal = true;
                                    if (indexOptimalNormal == -1)
                                        indexOptimalNormal = j;
                                    else if (_enemyList[j].Rank <= _enemyList[indexOptimalNormal].Rank)
                                        indexOptimalNormal = j;
                                }
                            }

                        //Если нашли карту для побития
                        if (findNormal || findTrump) 
                        {
                            int index = findNormal ? indexOptimalNormal : indexOptimalTrump;

                            _handList[i].IsCloseOnDesk = true;
                            _enemyList[index].IsOnDesk = true;
                            //Чтобы карта игрока на столе была за картой противника после побития
                            Canvas.SetZIndex(_enemyList[index], _handList[i].ZIndex + 1);
                            SetCardOnDesk(_enemyList[index], true);
                            _enemyList[index].LoadImage(_enemyList[index].ImageName);
                            UpdateHand(Player.Bot);
                        }                
                        else
                            break;
                    }
                }

                //Если был сделан break
                if (i != _handList.Count)
                    EnemyTake();
            }
        }

        //Противник берет карты
        private static void EnemyTake()
        {
            for (int i = _handList.Count-1; i >= 0; i--)
            {
                if (_handList[i].IsOnDesk)
                {
                    _handList[i].IsCloseOnDesk = false;
                    ChangeCardEvents(_handList[i], true);
                    _handList[i].LoadImage(Card.CARD_BACK_IMAGE_NAME);
                    _enemyList.Add(_handList[i]);
                    _handList.RemoveAt(i);
                }
            }

            for (int i = 0; i < _enemyList.Count; i++)
            {
                _enemyList[i].IsOnDesk = false;
                _enemyList[i].LoadImage(Card.CARD_BACK_IMAGE_NAME);
            }

            _deskLeftVariable = DESK_MARGIN_LEFT;
            _deskIsClear = true;
            CountAddedCardOnDesk = 0;
            CheckWin();
            UpdateHand(Player.Bot);
            DropCard(_handList);
        }

        #endregion


        #region -------------HumanLogic----------------

        //Можно ли положить карту на стол?
        public static bool CanDrop(Card element)
        {
            if (_deskIsClear)
                return true;

            if (_enemyList.All((x) => x.IsOnDesk))
                return false;

            //Если игрок отбивается
            if (_turnPlayer == Player.Bot)
            {
                foreach (var i in _enemyList)
                    if (i.IsOnDesk)
                    {
                        if (!i.IsCloseOnDesk &&
                            (i.Suit == element.Suit &&
                            i.Rank < element.Rank ||
                            !i.IsTrumpCard && element.IsTrumpCard))
                        {
                            Canvas.SetZIndex(element, Canvas.GetZIndex(i) + 1);
                            i.IsCloseOnDesk = true;
                            return true;
                        }
                    }
            }
            else
            {
                foreach (var i in _handList)
                    if (i.IsOnDesk && element.Rank == i.Rank)
                        return true;
                foreach (var i in _enemyList)
                    if (i.IsOnDesk && element.Rank == i.Rank)
                        return true;
            }

            return _deskIsClear;
        }

        private static void BtnBitoClick(object sender, RoutedEventArgs e)
        {
            if (_turnPlayer == Player.Human && !_deskIsClear)
                Turn();
            else
                MessageBox.Show("Сейчас не ваш ход или нет карт на столе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static void BtnTakeClick(object sender, RoutedEventArgs e)
        {
            if (_turnPlayer == Player.Human)
            {
                MessageBox.Show("Сейчас ваш ход.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            for (int i = _enemyList.Count - 1; i >= 0; i--)
            {
                if (_enemyList[i].IsOnDesk)
                {
                    _enemyList[i].IsCloseOnDesk = false;
                    _handList.Add(_enemyList[i]);
                    _enemyList.RemoveAt(i);
                }
            }

            for (int i = 0; i < _handList.Count; i++)
            {
                if (_handList[i].IsOnDesk)
                {
                    _handList[i].IsOnDesk = false;
                    ChangeCardEvents(_handList[i]);
                }
            }

            _deskLeftVariable = DESK_MARGIN_LEFT;
            _deskIsClear = true;
            CountAddedCardOnDesk = 0;
            CheckWin();
            UpdateHand();
            DropCard(_enemyList, Player.Bot);
            EnemyMoves();
        }

        #endregion


        private static void ClearDesk()
        {
            void DeleteFromGame(List<Card> list)
            {
                for (int i = list.Count-1; i >= 0; i--)
                    if (list[i].IsOnDesk)
                    {
                        GameWindow.Children.Remove(list[i]);
                        list.RemoveAt(i);
                    }
            }

            DeleteFromGame(_handList);
            DeleteFromGame(_enemyList);

            _deskIsClear = true;
        }

        private static void SetCanvasPosition(UIElement element, int left, int top)
        {
            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);
        }
    }
}

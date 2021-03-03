using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace CardGameLogic
{
    public static class Game
    {
        /*
         * Для того, чтобы убрать отображение "рубашки" у бота
         * Нужно закомментить строку 207 и строку 383
         */
        private static Canvas window;

        public delegate void EndGameHandler();
        public static event EndGameHandler EndGameEvent;

        private static Enum[] suitsNameList = { Card.Suits.Clubs, Card.Suits.Diamonds, Card.Suits.Hearts, Card.Suits.Spades };

        #region Отступы
        //В руках
        private static int marginLeft = 10;
        private static int marginTopHand = 599;
        private static int marginTopEnemy = -90;

        //На столе
        private const int deskMarginLeft = 108;                 //Констатное начало для 1 карты на столе
        private static int deskLeftVariable = deskMarginLeft;   //Меняется в зависимости от кол-ва карт
        private static int deskMarginTop = 258;
        private static int deskInterval = 145;

        //Положение области, показывающей границы поля карт в руке
        private static int handFieldLeft = 0;
        private static int handFieldTop = 570;

        //Положение правой панели (кнопки, счетчик, коз. масть на столе)
        private static int rightPanelMarginLeft = 1180;
        private static int rightPanelMarginTop = 270;

        #endregion

        private static Label handField = new Label();
        private static TextBlock countDeck = new TextBlock();

        private static List<Card> deck = new List<Card>();
        private static List<Card> handList = new List<Card>();
        private static List<Card> enemyList = new List<Card>();

        private static bool turnIsEnemy = false;
        private static bool deskIsClear = true;

        private static int zIndex = 1;
        
        //Количество "подкинутых для побития карт" во время хода
        private static int countAddedCardOnDesk = 0;

        public static void Start()
        {
            BitmapFrame ReturnsImage(string _path)
            {
                Uri resourceUri = new Uri(_path, UriKind.Relative);
                StreamResourceInfo stream_info = Application.GetResourceStream(resourceUri);
                BitmapFrame temp = BitmapFrame.Create(stream_info.Stream);

                return temp;
            }

            //Создаем поле, отражающее нашу руку
            handField.Height = 180;
            handField.Width = 1272;
            AddContorl(handField, handFieldLeft, handFieldTop, ref zIndex);


            //Выбираем козырную масть
            Random rnd = new Random();
            int indexTrumpSuit = rnd.Next(0, 4);

            //Создаем картинку козырной карты
            Image trumpImage = new Image();
            trumpImage.Source = ReturnsImage($"Resources/{suitsNameList[indexTrumpSuit].ToString()}Sym.jpg");
            trumpImage.Width = 70;
            trumpImage.Height = 70;
            AddContorl(trumpImage, rightPanelMarginLeft, rightPanelMarginTop, ref zIndex);


            //Заполняем колоду
            foreach (Enum i in suitsNameList)
                for(int j = 6; j<=14; j++)
                    deck.Add(new Card((Card.Suits)i, j, $"{i.ToString()}{j}.jpg") { TrumpCard = i == suitsNameList[indexTrumpSuit] ? true : false});


            //Cчетчик карт в колоде
            countDeck.Width = 70;
            countDeck.Height = 70;
            countDeck.Foreground = Brushes.Aqua;
            countDeck.FontSize = 30;
            countDeck.TextAlignment = TextAlignment.Center;
            AddContorl(countDeck, rightPanelMarginLeft, rightPanelMarginTop + 70, ref zIndex);


            //Кнопка "бито"
            Button btnBito = new Button();
            btnBito.Width = 80;
            btnBito.Height = 70;
            btnBito.Foreground = Brushes.Black;
            btnBito.Background = Brushes.NavajoWhite;
            btnBito.FontSize = 30;
            btnBito.Content = "Бито";
            btnBito.Click += new RoutedEventHandler(BtnBitoClick);
            AddContorl(btnBito, rightPanelMarginLeft, rightPanelMarginTop + 120, ref zIndex);


            //Кнопка "взять"
            Button btnTake = new Button();
            btnTake.Width = 80;
            btnTake.Height = 70;
            btnTake.Foreground = Brushes.Black;
            btnTake.Background = Brushes.NavajoWhite;
            btnTake.FontSize = 30;
            btnTake.Content = "Взять";
            btnTake.Click += new RoutedEventHandler(BtnTakeClick);
            AddContorl(btnTake, rightPanelMarginLeft, rightPanelMarginTop + 190, ref zIndex);


            //Выдаем по 6 карт
            DropCard(handList);
            DropCard(enemyList, true);
        }

        public static void SetCardOnDesk(Card element, bool forDefense = false)
        {
            deskIsClear = false;
            if (forDefense)
            {
                SetCanvasPosition(element, deskLeftVariable - deskInterval, deskMarginTop - 90);
                if (turnIsEnemy)
                    ChangeCardEvents(element, true);
            }
            else
            {
                SetCanvasPosition(element, deskLeftVariable, deskMarginTop);
                deskLeftVariable += deskInterval;
                if (!turnIsEnemy)
                    ChangeCardEvents(element, true);
                UpdateHand(turnIsEnemy);
            }
        }

        public static void UpdateHand(bool forEnemy = false)
        {
            int step;
            int cardCount = forEnemy ? enemyList.Count : handList.Count;

            if (cardCount <= 9)
                step = 140;
            else if (9 < cardCount && cardCount <= 12)
                step = 100;
            else if (12 < cardCount && cardCount <= 18)
                step = 70;
            else if (18 < cardCount && cardCount <= 25)
                step = 50;
            else
                step = 35;


            int leftPos = marginLeft;
            int topPos = forEnemy ? marginTopEnemy : marginTopHand;
            byte zIndex = 50;
            foreach (Card i in forEnemy ? enemyList : handList)
                if (!i.IsOnDesk)
                {
                    Canvas.SetZIndex(i, zIndex);
                    SetCanvasPosition(i, leftPos, topPos);
                    leftPos += step;
                    zIndex++;
                }
        }

        //-------------------------------------------------------------------------


        private static void DropCard(List<Card> cardList, bool forEnemy = false)
        {
            Random rnd = new Random();

            while (cardList.Count < 6 && deck.Count > 0)
            {
                int cardIndex = rnd.Next(0, deck.Count);
                Card card = deck[cardIndex];

                card.LoadImage(card.ImageName);

                if (forEnemy)
                {
                    card.LoadImage(Card.CardBackImageName);
                    ChangeCardEvents(card, true);
                }

                else
                    card.HaveInHand = true;

                AddContorl(card, marginLeft, forEnemy ? marginTopEnemy : marginTopHand, ref zIndex);

                cardList.Add(card);
                deck.RemoveAt(cardIndex);
            }
            
            countDeck.Text = deck.Count.ToString();

            UpdateHand(forEnemy);
        }

        //Передача хода
        private static void Turn()
        {
            deskLeftVariable = deskMarginLeft;
            turnIsEnemy = !turnIsEnemy;
            ClearDesk();
            CheckWin();
            if (turnIsEnemy)
            {
                DropCard(handList);
                DropCard(enemyList, true);
                EnemyMoves();
            }
            else
            {
                DropCard(enemyList, true);
                DropCard(handList);
            }
        }


        #region -------------EnemyLogic----------------

        public static void EnemyMoves()
        {
            //Противник ходит
            if (turnIsEnemy)
            {
                int index = -1;
                
                //Если доска пустая
                if (deskIsClear)
                {
                    int indexOptimalNormal = -1;
                    int indexOptimalTrump = -1;
                    for (int j = 0; j < enemyList.Count; j++)
                    {
                        if (enemyList[j].TrumpCard && (indexOptimalTrump == -1 ||
                           enemyList[j].Rank < enemyList[indexOptimalTrump].Rank))
                            indexOptimalTrump = j;
                        if (!enemyList[j].TrumpCard && (indexOptimalNormal == -1 ||
                            enemyList[j].Rank < enemyList[indexOptimalNormal].Rank))
                            indexOptimalNormal = j;
                    }
                    index = indexOptimalNormal == -1 ? indexOptimalTrump : indexOptimalNormal;
                }
                //Подкидывает карты
                else
                    for (int j = 0; j < enemyList.Count; j++)
                    {
                        if (countAddedCardOnDesk == 6)
                            break;
                        if (!enemyList[j].IsOnDesk && !enemyList[j].TrumpCard)
                        {
                            bool find = false;
                            foreach(Card i in enemyList)
                                if (i.IsOnDesk && enemyList[j].Rank == i.Rank)
                                {
                                    index = j;
                                    countAddedCardOnDesk++;
                                    find = true;
                                    break;
                                }
                            if (!find)
                                foreach(Card i in handList)
                                    if (i.IsOnDesk && enemyList[j].Rank == i.Rank)
                                    {
                                        index = j;
                                        countAddedCardOnDesk++;
                                        break;
                                    }
                        }
                    }

                //Если нашли карту, которую можно подкинуть - подкидываем
                if (index != -1)
                {
                    enemyList[index].IsOnDesk = true;
                    SetCardOnDesk(enemyList[index]);
                    enemyList[index].LoadImage(enemyList[index].ImageName);
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
                for (;i<handList.Count; i++)
                {
                    if (handList[i].IsOnDesk &&
                       !handList[i].IsCloseOnDesk)
                    {
                        if (handList[i].TrumpCard)
                            break;
                        bool findNormal = false;
                        bool findTrump = false;
                        int indexOptimalNormal = -1;
                        int indexOptimalTrump = -1;
                        for (int j = 0; j < enemyList.Count; j++)
                            if (!enemyList[j].IsOnDesk)     //Если карта в руке
                            {
                                if (enemyList[j].TrumpCard)
                                {
                                    findTrump = true;
                                    if (indexOptimalTrump == -1)
                                        indexOptimalTrump = j;
                                    else if (enemyList[j].Rank <= enemyList[indexOptimalTrump].Rank)
                                        indexOptimalTrump = j;
                                }
                                else if (enemyList[j].Suit == handList[i].Suit &&
                                        enemyList[j].Rank > handList[i].Rank)
                                {
                                    findNormal = true;
                                    if (indexOptimalNormal == -1)
                                        indexOptimalNormal = j;
                                    else if (enemyList[j].Rank <= enemyList[indexOptimalNormal].Rank)
                                        indexOptimalNormal = j;
                                }
                            }

                        //Если нашли карту для побития
                        if (findNormal || findTrump) 
                        {
                            int index = findNormal ? indexOptimalNormal : indexOptimalTrump;

                            handList[i].IsCloseOnDesk = true;
                            enemyList[index].IsOnDesk = true;
                            //Чтобы карта игрока на столе была за картой противника после побития
                            Canvas.SetZIndex(enemyList[index], handList[i].GetZIndex + 1);
                            SetCardOnDesk(enemyList[index], true);
                            enemyList[index].LoadImage(enemyList[index].ImageName);
                            UpdateHand(true);
                        }                
                        else
                            break;
                    }
                }

                //Если был сделан break
                if (i != handList.Count)
                    EnemyTake();
            }
        }

        //Противник берет карты
        private static void EnemyTake()
        {
            for (int i = handList.Count-1; i >= 0; i--)
            {
                if (handList[i].IsOnDesk)
                {
                    handList[i].IsCloseOnDesk = false;
                    ChangeCardEvents(handList[i], true);
                    handList[i].LoadImage(Card.CardBackImageName);
                    enemyList.Add(handList[i]);
                    handList.RemoveAt(i);
                }
            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].IsOnDesk = false;
                enemyList[i].LoadImage(Card.CardBackImageName);
            }

            deskLeftVariable = deskMarginLeft;
            deskIsClear = true;
            CheckWin();
            UpdateHand(true);
            DropCard(handList);
        }

        #endregion


        #region -------------HumanLogic----------------

        //Можно ли положить карту на стол?
        public static bool CanDrop(Card element)
        {
            if (deskIsClear)
                return true;

            if (enemyList.Count == 0)
                return false;

            //Если игрок отбивается
            if (turnIsEnemy)
            {
                foreach (var i in enemyList)
                    if (i.IsOnDesk)
                    {
                        if (!i.IsCloseOnDesk &&
                            (i.Suit == element.Suit &&
                            i.Rank < element.Rank ||
                            !i.TrumpCard && element.TrumpCard))
                        {
                            Canvas.SetZIndex(element, Canvas.GetZIndex(i) + 1);
                            i.IsCloseOnDesk = true;
                            return true;
                        }
                    }
            }
            else
            {
                foreach (var i in handList)
                    if (i.IsOnDesk && element.Rank == i.Rank)
                        return true;
                foreach (var i in enemyList)
                    if (i.IsOnDesk && element.Rank == i.Rank)
                        return true;
            }

            return deskIsClear;
        }

        private static void BtnBitoClick(object sender, RoutedEventArgs e)
        {
            if (!turnIsEnemy && !deskIsClear)
                Turn();
            else
                MessageBox.Show("Сейчас не ваш ход или нет карт на столе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static void BtnTakeClick(object sender, RoutedEventArgs e)
        {
            if (!turnIsEnemy)
            {
                MessageBox.Show("Сейчас ваш ход.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            for (int i = enemyList.Count - 1; i >= 0; i--)
            {
                if (enemyList[i].IsOnDesk)
                {
                    enemyList[i].IsCloseOnDesk = false;
                    handList.Add(enemyList[i]);
                    enemyList.RemoveAt(i);
                }
            }

            for (int i = 0; i < handList.Count; i++)
            {
                if (handList[i].IsOnDesk)
                {
                    handList[i].IsOnDesk = false;
                    ChangeCardEvents(handList[i]);
                }
            }

            deskLeftVariable = deskMarginLeft;
            deskIsClear = true;
            CheckWin();
            UpdateHand();
            DropCard(enemyList, true);
            EnemyMoves();
        }

        #endregion


        private static void CheckWin()
        {
            if(deck.Count == 0 && (handList.Count == 0 || enemyList.Count == 0))
            {
                if(handList.Count == 0 && enemyList.Count == 0)
                    MessageBox.Show("Ничья", "Игра закончена", MessageBoxButton.OK, MessageBoxImage.Information);
                else if(handList.Count == 0 && enemyList.Count!=0)
                    MessageBox.Show("Вы выиграли", "Игра закончена", MessageBoxButton.OK, MessageBoxImage.Information);
                else if(handList.Count!=0 && enemyList.Count == 0)
                    MessageBox.Show("Вы проиграли", "Игра закончена", MessageBoxButton.OK, MessageBoxImage.Information);
                EndGameEvent?.Invoke();
            }
        }

        private static void ClearDesk()
        {
            void DeleteFromGame(List<Card> list)
            {
                for (int i = list.Count-1; i >= 0; i--)
                    if (list[i].IsOnDesk)
                    {
                        window.Children.Remove(list[i]);
                        list.RemoveAt(i);
                    }
            }

            DeleteFromGame(handList);
            DeleteFromGame(enemyList);

            deskIsClear = true;
        }

        private static void AddContorl(UIElement element, int left, int top, ref int zIndex)
        {
            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);
            Canvas.SetZIndex(element, zIndex);
            window.Children.Add(element);
            zIndex++;
        }

        private static void SetCanvasPosition(UIElement element, int left, int top)
        {
            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);
        }

        private static void ChangeCardEvents(Card card, bool forEnemy = false)
        {
            if (forEnemy)      //Если карта для врага
            {
                card.MouseEnter -= new MouseEventHandler(card.MouseEnterFunc);
                card.MouseLeave -= new MouseEventHandler(card.MouseLeaveFunc);
                card.MouseDown -= new MouseButtonEventHandler(card.MouseDownFunc);
                card.MouseMove -= new MouseEventHandler(card.MouseMoveFunc);
                card.MouseUp -= new MouseButtonEventHandler(card.MouseUpFunc);
            }
            else
            {
                card.MouseEnter += new MouseEventHandler(card.MouseEnterFunc);
                card.MouseLeave += new MouseEventHandler(card.MouseLeaveFunc);
                card.MouseDown += new MouseButtonEventHandler(card.MouseDownFunc);
                card.MouseMove += new MouseEventHandler(card.MouseMoveFunc);
                card.MouseUp += new MouseButtonEventHandler(card.MouseUpFunc);
            }
        }


        #region Свойства

        public static Canvas GameWindow
        {
            set { window = value; }
            get { return window; }
        }

        public static SolidColorBrush ColorHandField
        {
            set { handField.Background = value; }
            get { return (SolidColorBrush)handField.Background; }
        }

        public static bool TurnIsEnemy
        {
            get { return turnIsEnemy; }
        }

        public static int HandFieldTop
        {
            get { return handFieldTop; }
        }

        public static int MarginTopHand
        {
            get { return marginTopHand; }
        }

        #endregion
    }
}

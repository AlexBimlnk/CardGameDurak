using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Data;

namespace CardGameLogic
{
    public abstract class Game
    {
        private static Canvas window;

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


        private static List<Card> deck = new List<Card>();
        private static List<Card> handList = new List<Card>();
        private static List<Card> enemyList = new List<Card>();

        private static bool turnIsEnemy = false;

        private static int zIndex = 1;

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
            Debug.WriteLine($"TrumpCard is {suitsNameList[indexTrumpSuit]}");

            //Создаем картинку козырной карты
            Image trumpImage = new Image();
            trumpImage.Source = ReturnsImage($"Resources/{suitsNameList[indexTrumpSuit].ToString()}Sym.jpg");
            trumpImage.Width = 70;
            trumpImage.Height = 70;
            AddContorl(trumpImage, rightPanelMarginLeft, rightPanelMarginTop, ref zIndex);


            //Заполняем колоду
            foreach (Enum i in suitsNameList)
                for(int j = 6; j<=14; j++)
                    deck.Add(new Card((Card.Suits)i, j, $"{i.ToString()}{j}.jpg"));


            //Cчетчик карт в колоде
            TextBlock countDeck = new TextBlock();
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
            int leftPos = marginLeft;
            for(int i = 1; i<=12; i++)  //<=18 чтою до конца экрана заполнить
            {
                int cardIndex = rnd.Next(0, deck.Count);
                Card card = deck[cardIndex];
                if (card.Suit == suitsNameList[indexTrumpSuit].ToString())  //Если козырная масть
                    card.TrumpCard = true;
                if (i % 2 == 0)                                                //Выдаем противнику
                {
                    //card.LoadImage(Card.CardBackImageName);
                    card.LoadImage(card.ImageName);
                    ChangeCardEvents(card, true);
                    AddContorl(card, leftPos, marginTopEnemy, ref zIndex);
                    enemyList.Add(card);
                    leftPos += 140;                                             //Ширина карт + 10
                }
                else
                {
                    card.HaveInHand = true;
                    card.LoadImage(card.ImageName);
                    AddContorl(card, leftPos, marginTopHand, ref zIndex);
                    handList.Add(card);
                }
                deck.RemoveAt(cardIndex);
            }
            countDeck.Text = deck.Count.ToString();
        }


        //------------EnemyLogic----------------

        public static void EnemyMoves()
        {
            //Противник ходит
            if (turnIsEnemy)
            {

            }
            //Противник отбивается
            else
            {
                foreach (var humanCard in handList)
                {
                    if (humanCard.IsOnDesk &&
                       !humanCard.IsCloseOnDesk &&
                       !humanCard.TrumpCard)
                    {
                        bool findNormal = false;
                        bool findTrump = false;
                        int indexOptimalNormal = -1;
                        int indexOptimalTrump = -1;
                        for (int i = 0; i < enemyList.Count; i++)
                            if (!enemyList[i].IsOnDesk)     //Если карта в руке
                            {
                                if (enemyList[i].TrumpCard)
                                {
                                    findTrump = true;
                                    if (indexOptimalTrump == -1)
                                        indexOptimalTrump = i;
                                    else if (enemyList[i].Rank <= enemyList[indexOptimalTrump].Rank)
                                        indexOptimalTrump = i;
                                }
                                else if (enemyList[i].Suit == humanCard.Suit &&
                                        enemyList[i].Rank > humanCard.Rank)
                                {
                                    findNormal = true;
                                    if (indexOptimalNormal == -1)
                                        indexOptimalNormal = i;
                                    else if (enemyList[i].Rank <= enemyList[indexOptimalNormal].Rank)
                                        indexOptimalNormal = i;
                                }
                            }

                        //Если нашли карту для побития
                        if (findNormal || findTrump) 
                        {
                            int index = findNormal ? indexOptimalNormal : indexOptimalTrump;

                            humanCard.IsCloseOnDesk = true;
                            enemyList[index].IsOnDesk = true;
                            //Чтобы карта игрока на столе была за картой противника после побития
                            Canvas.SetZIndex(enemyList[index], Canvas.GetZIndex(humanCard) + 1);
                            SetCardOnDesk(enemyList[index], true, (int)Canvas.GetLeft(humanCard));
                        }

                        //Противник берет карты
                        else
                        {

                        }
                    }
                }
            }
        }

        //------------EnemyLogic----------------


        public static void SetCardOnDesk(Card element, bool forEnemy = false, int leftPos = 0)
        {
            if (forEnemy)
                SetCanvasPosition(element, leftPos, deskMarginTop-90);
            else
            {
                SetCanvasPosition(element, deskLeftVariable, deskMarginTop);
                deskLeftVariable += deskInterval;
                ChangeCardEvents(element, true);
                UpdateHand();
            }
        }

        public static void UpdateHand()
        {
            int leftPos = marginLeft;
            foreach(Card i in handList)
                if (!i.IsOnDesk)
                {
                    SetCanvasPosition(i, leftPos, marginTopHand);
                    leftPos += 140;
                }
        }

        private static void ClearDesk(bool _turnIsEnemy = false)
        {
            void DeleteFromGame(List<Card> list)
            {
                for (int i = 0; i < list.Count; i++)
                    if (list[i].IsOnDesk)
                    {
                        window.Children.Remove(list[i]);
                        list[i] = null;
                        list.RemoveAt(i);
                    }
            }

            //Если "Бито" говорит бот
            if (_turnIsEnemy)
            {

            }
            else
            {
                DeleteFromGame(handList);
                DeleteFromGame(enemyList);
            }

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

        private static void BtnBitoClick(object sender, RoutedEventArgs e)
        {

        }

        private static void BtnTakeClick(object sender, RoutedEventArgs e)
        {

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

        public static int DeskLeftVariable
        {
            set { if (value == 0) deskLeftVariable = deskMarginLeft; }
            get { return deskLeftVariable; }
        }

        public static int DeskMarginTop
        {
            get { return deskMarginTop; }
        }

        public static int DeskInterval
        {
            get { return deskInterval; }
        }


        public static int HandFieldTop
        {
            get { return handFieldTop; }
        }

        public static int MarginTopHand
        {
            get { return marginTopHand; }
        }

        public static int MarginTopEnemy
        {
            get { return marginTopEnemy; }
        }

        public static int MarginLeft
        {
            get { return marginLeft; }
        }
        #endregion
    }
}

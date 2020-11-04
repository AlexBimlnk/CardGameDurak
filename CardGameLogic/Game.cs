﻿using System;
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

        //Положение картинки козырной масти на столе
        private static int trumpImageMarginLeft = 1200;
        private static int trumpImageMarginTop = 270;

        //Положение счетчика карт в колоде на столе
        private static int countDeckMarginLeft = 1200;
        private static int countDeckMarginTop = 340;

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

            //Создаем карттинку козырной карты
            Image trumpImage = new Image();
            trumpImage.Source = ReturnsImage($"Resources/{suitsNameList[indexTrumpSuit].ToString()}Sym.jpg");
            trumpImage.Width = 70;
            trumpImage.Height = 70;
            AddContorl(trumpImage, trumpImageMarginLeft, trumpImageMarginTop, ref zIndex);


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
            AddContorl(countDeck, countDeckMarginLeft, countDeckMarginTop, ref zIndex);


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
                    card.LoadImage(Card.CardBackImageName);
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

        private static void ClearDesk(bool _turnIsEnemy = false)
        {
            //Если "Бито" говорит бот
            if (_turnIsEnemy)
            {

            }
            else
            {
                void DeleteFromGame(List<Card> list)
                {
                    for(int i = 0; i<list.Count; i++)
                        if (list[i].IsOnDesk)
                        {
                            list[i] = null;
                            window.Children.Remove(list[i]);
                            list.RemoveAt(i);
                        }
                }
                DeleteFromGame(handList);
                DeleteFromGame(enemyList);
            }
            
        }

        public static void SetCardOnDesk(Card element)
        {
            SetCanvasPosition(element, deskLeftVariable, deskMarginTop);
            deskLeftVariable += deskInterval;
            ChangeCardEvents(element, true);
            UpdateHand();
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

        public static int DeskMarginLeft
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
    }
}

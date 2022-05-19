using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using CardGameLogic.Enums;
using CardGameLogic.Players;

namespace CardGameLogic
{
    public class Card : Label, IEquatable<Card>
    {
        private readonly ImageBrush _brush = new ImageBrush();
        private readonly DoubleAnimation _animation = new DoubleAnimation();

        private readonly int _width = 130;
        private readonly int _height = 180;

        public const string CARD_BACK_IMAGE_NAME = "_CardBack.jpg";


        public Card(Suit suit, int rank, string imageName)
        {
            Suit = suit;
            Rank = rank;
            ImageName = imageName;
            Width = _width;
            Height = _height;
            Background = _brush;
            LoadDefaultSettings();
        }

        public DoubleAnimation Animation => _animation;
        /// <summary>
        /// Игрок, имеющий данную карту
        /// </summary>
        public IPlayer Owner { get; set; }
        public int ZIndex { get; set; }
        public int Rank { get; private set; }       // Сила карты

        public Suit Suit { get; private set; }
        public string ImageName { get; private set; }

        public bool IsOnDesk { get; set; }
        public bool IsCloseOnDesk { get; set; }     //Карта, выложенная на стол, побита
        public bool IsHaveInHand { get; set; }      //Может быть не нужно и надо убрать в будущем
        public bool IsTrumpCard { get; set; }       //Козырная масть


        private void LoadDefaultSettings()
        {
            ChangeBorder(Brushes.Black, 1);
            HorizontalAlignment = HorizontalAlignment.Left;
        }

        //Придает рамке карты соответствующий цвет и ширину
        public void ChangeBorder(SolidColorBrush color, int tWidth)
        {
            BorderBrush = color;
            BorderThickness = new Thickness(tWidth, tWidth, tWidth, tWidth);
        }

        //Задает параметры анимации
        public void ChangeAnimation(double from, double to, TimeSpan time)
        {
            _animation.From = from;
            _animation.To = to;
            _animation.Duration = time;
        }

        public void LoadImage(BitmapFrame bitmapFrame) => _brush.ImageSource = bitmapFrame;

        public bool Equals(Card other) => Suit == other.Suit && Rank == other.Rank;
    }
}

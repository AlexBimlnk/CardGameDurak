using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using CardGameLogic.Enums;

namespace CardGameLogic
{
    public class Card : Label, IEquatable<Card>
    {
        private ImageBrush _brush;
        private readonly DoubleAnimation _animation = new DoubleAnimation();

        private readonly int _width = 130;
        private readonly int _height = 180;

        public const string CardBackImageName = "_CardBack.jpg";


        public Card(Suit suit, int rank, string imageName)
        {
            Suit = suit;
            Rank = rank;
            ImageName = imageName;
            Width = _width;
            Height = _height;
            LoadDefaultSettings();
        }

        public DoubleAnimation Animation => _animation;

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

        public void LoadImage(string imageName)
        {
            string path = $"Resources/{imageName}";

            // Создание картинки для карты
            Uri resourceUri = new Uri(path, UriKind.Relative);
            StreamResourceInfo stream_info = Application.GetResourceStream(resourceUri);
            BitmapFrame temp = BitmapFrame.Create(stream_info.Stream);
            _brush = new ImageBrush() { ImageSource = temp };

            Background = _brush;
        }

        public bool Equals(Card other) => Suit == other.Suit && Rank == other.Rank;
    }
}

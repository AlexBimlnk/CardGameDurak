using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace CardGameLogic
{
    public class Card : Label
    {
        public enum Suits
        {
            Clubs,      //Трефы
            Diamonds,   //Буби
            Hearts,     //Червы
            Spades      //Пики
        }

        private ImageBrush brush;
        private DoubleAnimation animation = new DoubleAnimation();

        public Point? movePoint;

        private int rank;                   //сила 
        private int width = 130;
        private int height = 180;

        private string suit;                //масть 
        private string imageName;
        public static string CardBackImageName = "_CardBack.jpg";

        private bool isOnDesk = false;
        private bool isCloseOnDesk = false; //Карта, выложенная на стол, побита
        private bool haveInHand = false;
        private bool trumpCard = false;     //Козырная масть

        public Card(Suits _suit, int _rank, string _imageName)
        {
            suit = _suit.ToString();
            rank = _rank;
            imageName = _imageName;
            LoadDefaultSettings();
        }

        public void LoadImage(string _imageName)
        {
            string path = $"Resources/{_imageName}";

            #region Создание картинки для карты
            Uri resourceUri = new Uri(path, UriKind.Relative);
            StreamResourceInfo stream_info = Application.GetResourceStream(resourceUri);
            BitmapFrame temp = BitmapFrame.Create(stream_info.Stream);
            brush = new ImageBrush();
            brush.ImageSource = temp;

            this.Background = brush;
            #endregion
        }

        private void LoadDefaultSettings()
        {
            //default settings UI-------------------------------------
            Width = width;
            Height = height;
            ChangeBorder(Brushes.Black, 1);
            HorizontalAlignment = HorizontalAlignment.Left;
            this.MouseEnter += new MouseEventHandler(MouseEnterFunc);
            this.MouseLeave += new MouseEventHandler(MouseLeaveFunc);
            this.MouseDown += new MouseButtonEventHandler(MouseDownFunc);
            this.MouseMove += new MouseEventHandler(MouseMoveFunc);
            this.MouseUp += new MouseButtonEventHandler(MouseUpFunc);
        }


        //Придает рамке карты соответствующий цвет и ширину
        private void ChangeBorder(SolidColorBrush _color, int _tWidth)
        {
            this.BorderBrush = _color;
            this.BorderThickness = new Thickness(_tWidth, _tWidth, _tWidth, _tWidth);
        }

        //Задать параметры анимации
        private void ChangeAnimation(double _from, double _to, TimeSpan _time)
        {
            animation.From = _from;
            animation.To = _to;
            animation.Duration = _time;
        }


        #region События
        public void MouseEnterFunc(object sender, MouseEventArgs e)
        {
            ChangeBorder(Brushes.Purple, 2);
            ChangeAnimation(Game.MarginTopHand, Game.MarginTopHand - this.height / 2, TimeSpan.FromSeconds(0.3));
            this.BeginAnimation(Canvas.TopProperty, animation);
        }

        public void MouseLeaveFunc(object sender, MouseEventArgs e)
        {
            ChangeBorder(Brushes.Black, 1);
            ChangeAnimation(Canvas.GetTop(this), Game.MarginTopHand, TimeSpan.FromSeconds(0.3));
            this.BeginAnimation(Canvas.TopProperty, animation);
        }

        public void MouseDownFunc(object sender, MouseEventArgs e)
        {
            movePoint = e.GetPosition(this);
            this.CaptureMouse();
            this.BeginAnimation(Canvas.TopProperty, null);

            Game.ColorHandField = Brushes.NavajoWhite;
            //Canvas.SetZIndex(this, 100);
            Debug.WriteLine(Canvas.GetZIndex(this));
            Debug.WriteLine($"y = {Canvas.GetTop(this)} x = {Canvas.GetLeft(this)}");
        }

        public void MouseUpFunc(object sender, MouseEventArgs e)
        {
            movePoint = null;
            this.ReleaseMouseCapture();

            Game.ColorHandField = null;
        }

        public void MouseMoveFunc(object sender, MouseEventArgs e)
        {
            if (movePoint == null)
                return;
            var p = e.GetPosition(Game.GameWindow) - (Vector)movePoint.Value;
            Canvas.SetLeft(this, p.X);
            Canvas.SetTop(this, p.Y);
        }

        #endregion

        #region Свойства

        public int Rank
        {
            get { return rank; }
        }

        public string Suit
        {
            get { return suit; }
        }
        public string ImageName
        {
            get { return imageName; }
        }

        //BOOL----------------------------

        public bool IsOnDesk
        {
            get { return isOnDesk; }
            set { isOnDesk = value; }
        }
        public bool IsCloseOnDesk
        {
            get { return isCloseOnDesk; }
            set { isCloseOnDesk = value; }
        }
        public bool HaveInHand
        {
            get { return haveInHand; }
            set { haveInHand = value; }
        }
        public bool TrumpCard
        {
            get { return trumpCard; }
            set { trumpCard = value; }
        }
        #endregion
    }
}

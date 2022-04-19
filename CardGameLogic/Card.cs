using System;
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
        private ImageBrush _brush;
        private DoubleAnimation _animation = new DoubleAnimation();

        private Point? _movePoint;

        private readonly int _width = 130;
        private readonly int _height = 180;


        public const string CardBackImageName = "_CardBack.jpg";


        public Card(Suit suit, int rank, string imageName)
        {
            Suit = suit;
            Rank = rank;
            ImageName = imageName;
            LoadDefaultSettings();
        }


        public int ZIndex { get; private set; }
        public int Rank { get; private set; }       // Сила карты

        public Suit Suit { get; private set; }
        public string ImageName { get; private set; }

        public bool IsOnDesk { get; set; }
        public bool IsCloseOnDesk { get; set; }     //Карта, выложенная на стол, побита
        public bool IsHaveInHand { get; set; }      //Может быть не нужно и надо убрать в будущем
        public bool IsTrumpCard { get; set; }       //Козырная масть


        private void LoadDefaultSettings()
        {
            //default settings UI-------------------------------------
            Width = _width;
            Height = _height;
            ChangeBorder(Brushes.Black, 1);
            HorizontalAlignment = HorizontalAlignment.Left;
            MouseEnter += new MouseEventHandler(MouseEnterFunc);
            MouseLeave += new MouseEventHandler(MouseLeaveFunc);
            MouseDown += new MouseButtonEventHandler(MouseDownFunc);
            MouseMove += new MouseEventHandler(MouseMoveFunc);
            MouseUp += new MouseButtonEventHandler(MouseUpFunc);
        }

        //Придает рамке карты соответствующий цвет и ширину
        private void ChangeBorder(SolidColorBrush color, int tWidth)
        {
            BorderBrush = color;
            BorderThickness = new Thickness(tWidth, tWidth, tWidth, tWidth);
        }

        //Задает параметры анимации
        private void ChangeAnimation(double from, double to, TimeSpan time)
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


        #region Обработчики событий для карты
        public void MouseEnterFunc(object sender, MouseEventArgs e)
        {
            ChangeBorder(Brushes.Purple, 2);
            ChangeAnimation(Game.MARGIN_TOP_HAND, Game.MARGIN_TOP_HAND - _height / 2, TimeSpan.FromSeconds(0.3));
            BeginAnimation(Canvas.TopProperty, _animation);
        }

        public void MouseLeaveFunc(object sender, MouseEventArgs e)
        {
            ChangeBorder(Brushes.Black, 1);
            ChangeAnimation(Canvas.GetTop(this), Game.MARGIN_TOP_HAND, TimeSpan.FromSeconds(0.3));
            BeginAnimation(Canvas.TopProperty, _animation);
        }

        public void MouseDownFunc(object sender, MouseEventArgs e)
        {
            _movePoint = e.GetPosition(this);
            CaptureMouse();
            BeginAnimation(Canvas.TopProperty, null);

            Game.ColorHandField = Brushes.NavajoWhite;
            ZIndex = Canvas.GetZIndex(this);
            Canvas.SetZIndex(this, 100);
        }

        public void MouseUpFunc(object sender, MouseEventArgs e)
        {
            _movePoint = null;
            ReleaseMouseCapture();

            Game.ColorHandField = null;
            Canvas.SetZIndex(this, ZIndex);
            //Если хотим оставить в руке
            if(Canvas.GetTop(this)+_height >= Game.HAND_FIELD_TOP || Game.CountAddedCardOnDesk == 6)
                Game.UpdateHand();
            else if(Game.CanDrop(this))
            {
                if(!Game.TurnIsEnemy)
                    Game.CountAddedCardOnDesk++;
                IsOnDesk = true;
                ChangeBorder(Brushes.Black,1);
                Game.SetCardOnDesk(this,Game.TurnIsEnemy);
                Game.UpdateHand();
                Game.EnemyMoves();
            }
            else
            {
                Game.UpdateHand();
                MessageBox.Show("Сейчас невозможно положить эту карту на стол.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void MouseMoveFunc(object sender, MouseEventArgs e)
        {
            if (_movePoint is null)
                return;
            var p = e.GetPosition(Game.GameWindow) - (Vector)_movePoint.Value;
            Canvas.SetLeft(this, p.X);
            Canvas.SetTop(this, p.Y);
        }

        #endregion
    }
}

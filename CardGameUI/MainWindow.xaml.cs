using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CardGameLogic;
using System.Windows.Resources;

namespace CardGameUI
{
    public partial class MainWindow : Window
    {
        private static Canvas windowUI = new Canvas();
        private static ImageBrush brush;
        public MainWindow()
        {
            InitializeComponent();
            CreateWindowUI();
            Game.GameWindow = windowUI;
            Game.EndGameEvent += CloseApp;
            Content = windowUI;
            Game.Start();
        }

        //Создание окна приложения (игрального стола), Canvas
        private void CreateWindowUI()
        {
            BitmapFrame ReturnsImage(string _path)
            {
                Uri resourceUri = new Uri(_path, UriKind.Relative);
                StreamResourceInfo stream_info = Application.GetResourceStream(resourceUri);
                BitmapFrame temp = BitmapFrame.Create(stream_info.Stream);

                return temp;
            }

            brush = new ImageBrush();
            brush.ImageSource = ReturnsImage($"Resources/_Desk.jpg");
            windowUI.Background = brush;

            windowUI.Height = 689;
            windowUI.Width = 1272;
        }

        private void CloseApp()
        {
            Application.Current.Shutdown();
        }
    }
}

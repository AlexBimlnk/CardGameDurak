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
        public MainWindow()
        {
            InitializeComponent();
            Game.GameWindow = window;
            Game.EndGameEvent += CloseApp;
            Game.Start();
        }

        private void CloseApp()
        {
            Application.Current?.Shutdown();
        }
    }
}

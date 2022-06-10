using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CardGameLogic;
using System.Windows.Resources;
using CardGameLogic.Enums;

namespace CardGameUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Game.GameWindow = window;
            Game.StartNewSession(GameMode.SinglePlayer).EndGameEvent += CloseApp; ;
        }

        private void CloseApp(object sender, EventArgs e) => Application.Current?.Shutdown();
    }
}

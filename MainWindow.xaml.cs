using System;
using System.Windows;
using System.Diagnostics;

namespace BinanceLockedStakingAlert
{
    public partial class MainWindow : Window
    {
        public static Stopwatch Stopwatch;

        public MainWindow()
        {
            Stopwatch = new Stopwatch();
            Stopwatch.Start();

            Logger.Info("Binance Locked Staking Alert opened");

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Logger.Info("Initial window loaded");

            this.WindowState = WindowState.Maximized;
            var config = Utilities.GetConfig<Config>("config.json");

            Utilities.DelayAction(config.reloadTime, new Action(() => { StartProgram(config); }));
        }

        private void StartProgram(Config config)
        {
            new LockedStakingChecker(this.webView, config);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Logger.Info("Binance Locked Staking Alert closed");

            Stopwatch.Stop();
        }
    }
}
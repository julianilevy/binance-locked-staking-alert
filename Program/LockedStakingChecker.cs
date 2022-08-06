using System;
using Microsoft.Web.WebView2.Wpf;

namespace BinanceLockedStakingAlert
{
    public class LockedStakingChecker
    {
        private WebView2 _webView = null;
        private Coin _lastFirstCoinListed = null;
        private TwilioMessager _twilioMessager = null;

        // Config //
        private string _binanceUrl = String.Empty;
        private int _reloadTime = 0;

        public LockedStakingChecker(WebView2 webView, Config config)
        {
            _webView = webView;
            _lastFirstCoinListed = new Coin();
            _twilioMessager = new TwilioMessager(config);
            _binanceUrl = config.binanceUrl;
            _reloadTime = config.reloadTime;

            Logger.Info("Locked Staking Checker initialized");

            CheckLockedStaking();
        }

        private async void CheckLockedStaking()
        {
            Logger.Info($"A new Locked Staking check is starting");

            try
            {
                var currentHtmlJson = await _webView.CoreWebView2.ExecuteScriptAsync("document.body.outerHTML");

                var currentFirstCoinListed = new Coin();
                currentFirstCoinListed.GatherDataFromBinanceHtml(currentHtmlJson);

                if (_lastFirstCoinListed.name != String.Empty && _lastFirstCoinListed.name != currentFirstCoinListed.name)
                {
                    Logger.Warn($"{currentFirstCoinListed.name} was detected as a new listing on Binance Locked Staking!");

                    _lastFirstCoinListed.CloneFrom(currentFirstCoinListed);
                    _lastFirstCoinListed.LoadRemainingData();

                    Utilities.DelayAction(_reloadTime / 3, new Action(() => { SendMessage(); }));

                    return;
                }

                _lastFirstCoinListed.CloneFrom(currentFirstCoinListed);

                Logger.Info("Locked Staking successfully checked");
            }
            catch (Exception e)
            {
                Logger.Error("An error occurred while checking Locked Staking", e);
            }

            ReloadUrl();
        }

        private void SendMessage()
        {
            var message = $". \n" +
                          $"..... \n" +
                          $"{_lastFirstCoinListed.name} was just listed on Binance Locked Staking!\n" +
                          $"Est. APY: {_lastFirstCoinListed.apyPercentage}\n" +
                          $"USD Price: {_lastFirstCoinListed.usdPrice}\n" +
                          $"ETH Price: {_lastFirstCoinListed.ethPrice}\n" +
                          $"Market Cap: {_lastFirstCoinListed.marketCap}";

            if (_lastFirstCoinListed.name.Length >= 3)
            {
                _twilioMessager.Send(message);
                ReloadUrl();
            }
            else
                Logger.Error("An attempt was made to send a message but the content was empty");
        }

        private void ReloadUrl()
        {
            _webView.CoreWebView2.Navigate(_binanceUrl);

            Utilities.DelayAction(_reloadTime, new Action(() => { CheckLockedStaking(); }));

            Logger.Info($"Binance Locked Staking URL will be reloaded in {_reloadTime}ms");
        }
    }
}
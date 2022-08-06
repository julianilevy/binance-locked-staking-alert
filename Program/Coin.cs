using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace BinanceLockedStakingAlert
{
    public class Coin
    {
        public string name = String.Empty;
        public string apyPercentage = String.Empty;
        public string usdPrice = String.Empty;
        public string ethPrice = String.Empty;
        public string marketCap = String.Empty;

        private string _currentHtmlJson = String.Empty;
        private string _name = String.Empty;

        public void GatherDataFromBinanceHtml(string binanceHtmlJson)
        {
            _currentHtmlJson = binanceHtmlJson;

            GetName();
        }

        public void LoadRemainingData()
        {
            GetApyPercentage();
            RequestToCoinGeckoApi();
        }

        private void GetName()
        {
            try
            {
                if (_currentHtmlJson.Contains("https://bin.bnbstatic.com/static/images/coin/"))
                {
                    name = _currentHtmlJson.GetBetween("https://bin.bnbstatic.com/static/images/coin/", ".svg").ToUpper();
                    _name = name.ToLower();

                    Logger.Info($"{name} name successfully gathered from Binance HTML");
                }
            }
            catch (Exception e)
            {
                Logger.Error("An error occurred while gathering a new listed coin name", e);
            }
        }

        private void GetApyPercentage()
        {
            try
            {
                if (_currentHtmlJson.Contains("css-80o0mu"))
                {
                    apyPercentage = _currentHtmlJson.GetAfter("css-80o0mu", 3);
                    apyPercentage = apyPercentage.GetBefore("%");

                    Logger.Info($"{name} APY Percentage successfully gathered from Binance HTML");
                }
            }
            catch (Exception e)
            {
                Logger.Error($"An error occurred while gathering {name} APY Percentage from Binance HTML", e);
            }

        }

        private async void RequestToCoinGeckoApi()
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://api.coingecko.com/api/v3/simple/price?ids={ _name }&vs_currencies=usd%2Ceth&include_market_cap=true");
                var responseBodyJson = await response.Content.ReadAsStringAsync();
                var responseBodyObject = JObject.Parse(responseBodyJson);

                GetUsdPrice(responseBodyObject);
                GetEthPrice(responseBodyObject);
                GetMarketCap(responseBodyObject);

                Logger.Info($"Coin Gecko API request for {name} successfully executed");
            }
            catch (Exception e)
            {
                Logger.Error($"An error occurred while executing the Coin Gecko API request for {name}", e);
            }
        }

        private void GetUsdPrice(JObject response)
        {
            usdPrice = $"${response[_name]["usd"]}";
        }

        private void GetEthPrice(JObject response)
        {
            ethPrice = $"{response[_name]["eth"]}ETH";
        }

        private void GetMarketCap(JObject response)
        {
            marketCap = $"{response[_name]["usd_market_cap"]}";
            marketCap = marketCap.GetBefore(".", 1);
            marketCap = String.Format("{0:#,###0.#}", Single.Parse(marketCap));
            marketCap = $"${ marketCap }";
        }

        public void CloneFrom(Coin otherCoin)
        {
            name = otherCoin.name;
            apyPercentage = otherCoin.apyPercentage;
            usdPrice = otherCoin.usdPrice;
            ethPrice = otherCoin.ethPrice;
            marketCap = otherCoin.marketCap;

            _currentHtmlJson = otherCoin._currentHtmlJson;
            _name = otherCoin._name;
        }
    }
}
using System.Collections.Generic;

namespace BinanceLockedStakingAlert
{
    public class Config
    {
        public string binanceUrl;
        public int reloadTime;
        public string fromNumber;
        public List<string> toNumbers;
    }
}
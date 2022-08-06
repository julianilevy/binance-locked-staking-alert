using System;
using System.Linq;
using System.Collections.Generic;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

namespace BinanceLockedStakingAlert
{
    public class TwilioMessager
    {
        // Config //
        private PhoneNumber _fromNumber;
        private List<PhoneNumber> _toNumbers;

        public TwilioMessager(Config config)
        {
            try
            {
                _fromNumber = new PhoneNumber(config.fromNumber);
                _toNumbers = config.toNumbers.Select(toNumber => new PhoneNumber(toNumber)).ToList();

                var accountSid = Environment.GetEnvironmentVariable("TwilioAccountSid-01");
                var authToken = Environment.GetEnvironmentVariable("TwilioAuthToken-01");

                TwilioClient.Init(accountSid, authToken);

                Logger.Info("Twilio Messager initialized");
            }
            catch (Exception e)
            {
                Logger.Error("An error occurred while initializing Twilio Messager", e);
            }
        }

        public void Send(string message)
        {
            foreach (var toNumber in _toNumbers)
            {
                try
                {
                    MessageResource.Create(body: message, from: _fromNumber, to: toNumber);

                    Logger.Info($"A twilio message from {_fromNumber} to {toNumber} was successfully sent: {message}");
                }
                catch (Exception e)
                {
                    Logger.Error($"An error occurred while sending a twilio message from {_fromNumber} to {toNumber}: {message}", e);
                }
            }
        }
    }
}
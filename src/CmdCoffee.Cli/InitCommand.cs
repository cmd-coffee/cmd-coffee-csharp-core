using System;
using System.Collections.Generic;
using System.Linq;
using CmdCoffee.Client;

namespace CmdCoffee.Cli
{
    public class InitCommand : ICoffeeCommand
    {
        private readonly ICmdCoffeeApi _cmdCoffeeApi;
        private readonly Func<ICmdCoffeeApiSettings> _apiSettingsFactory;
        private readonly IOutputWriter _outputWriter;
        public string Name => "init";
        public string Parameters => "invite-code";
        public string Description => "request an access-key";

        public InitCommand(ICmdCoffeeApi cmdCoffeeApi, Func<ICmdCoffeeApiSettings> apiSettingsFactory,
            IOutputWriter outputWriter)
        {
            _cmdCoffeeApi = cmdCoffeeApi;
            _apiSettingsFactory = apiSettingsFactory;
            _outputWriter = outputWriter;
        }

        public void Execute(IList<string> args)
        {
            var appSettings = _apiSettingsFactory();

            if (!string.IsNullOrEmpty(appSettings.AccessKey))
            {
                _outputWriter.WriteError("An access-key is already specified in your settings.");
                return;
            }

            var inviteCode = args.FirstOrDefault();

            if (string.IsNullOrEmpty(inviteCode))
            {
                _outputWriter.WriteError("'invite-code' is required");
                return;
            }

            var result = _cmdCoffeeApi.Join(inviteCode).Result;

            _outputWriter.WriteLine("\nInvite code accepted!");
            _outputWriter.WriteLine($"\n{result.legal}");
            _outputWriter.WriteLine($"{result.termsOfUse}"); 
            _outputWriter.WriteLine($"{result.privacyPolicy}"); 

            var answer = _outputWriter.AskYesNo("Ok?");

            if (!answer)
            {
                _outputWriter.AwaitAnyKey("Okay. When you change your mind, we're here for you");
                return;
            }

            _outputWriter.WriteLine($"\n{result.welcomeMessage}");

            _outputWriter.WriteLine($"access-key: {result.accessKey}");

            _outputWriter.WriteLine("\nTo use your accessKey, update your app-settings.json file");
            _outputWriter.WriteLine("Update your shipping address while you're at it!");

            _outputWriter.AwaitAnyKey();
        }

    }
}
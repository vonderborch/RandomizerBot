using System;
using Discord;
using Discord.WebSocket;

namespace RandomGamePickerBot
{
    public class Bot
    {
        private DiscordSocketClient _client;

        public async Task BotAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;

            var token = "";
            // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            // var token = File.ReadAllText("token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}

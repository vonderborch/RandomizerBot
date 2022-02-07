using Discord;
using Discord.WebSocket;

namespace RandomizerBot
{
    public class Bot
    {
        private DiscordSocketClient? _client;

        private BotCommandParser? _parser;

        public async Task RunBot()
        {
            _parser = new BotCommandParser();

            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.MessageReceived += ClientOnMessageReceived;

            // var token = "";
            // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            var token = File.ReadAllText("Token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

            Console.WriteLine($"Bot Version: {Constants.Version}");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task ClientOnMessageReceived(SocketMessage arg)
        {
            _parser?.ParseCommand(arg.Content, arg);
            return Task.CompletedTask;
        }
    }
}
using Discord;
using Discord.WebSocket;

namespace RandomizerBot
{
    public static class CommandParser
    {
        public static void ParseCommand(string message, SocketMessage messageArgs)
        {
            if (!message.StartsWith("!rb", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            Log($"Parsing message [{message}]");

            var commands = CommandRegister.Instance.Commands;

            var server = ((SocketGuildChannel)messageArgs.Channel).Guild;
            var args = message.Split(' ');
            if (args.Length > 0)
            {
                if (commands.TryGetValue(args[0].Replace("!rb_", "").Replace("!rb", "").ToLowerInvariant(), out var cmd))
                {
                    if (cmd.RequiredArgumentCount > args.Length - 1)
                    {
                        Log("Invalid usage of command!");
                        messageArgs.Channel.SendMessageAsync($"Invalid Usage! Correct Usage:```{cmd.GetErrorMessage()}```");
                        return;
                    }

                    var parsedArguments = new Dictionary<string, string>();
                    if (cmd.Arguments.Count > 0)
                    {
                        for (var i = 1; i < args.Length; i++)
                        {
                            parsedArguments.Add(cmd.Arguments[i - 1].Name, args[i]);
                        }
                    }
                    
                    var good = cmd.Execute(parsedArguments, messageArgs, server);
                    if (!good)
                    {
                        Log("Invalid usage of command!");
                        messageArgs.Channel.SendMessageAsync($"Invalid Usage! Correct Usage:```{cmd.GetErrorMessage()}```");
                    }
                }
                else
                {
                    Log("Unknown command!");
                    commands["help"].Execute(new Dictionary<string, string>(), messageArgs, server);
                }
            }
        }

        public static void Log(string message, LogSeverity severity = LogSeverity.Info, Exception exception = null)
        {
            Console.WriteLine(new LogMessage(severity, "CommandParser", message, exception).ToString());
        }
    }
}

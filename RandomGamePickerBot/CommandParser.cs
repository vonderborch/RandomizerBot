using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace RandomGamePickerBot
{
    public static class CommandParser
    {
        public static void ParseCommand(string message, SocketMessage messageArgs)
        {
            if (!message.StartsWith("!rb_"))
            {
                return;
            }

            var commands = CommandRegister.Instance.Commands;

            var args = message.Split(' ');
            if (args.Length > 0)
            {
                if (commands.TryGetValue(args[0].Replace("!rb_", ""), out var cmd))
                {
                    if (cmd.RequiredArgumentCount > args.Length - 1)
                    {
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

                    cmd.Execute(parsedArguments, messageArgs);
                }
                else
                {
                    commands["help"].Execute(new Dictionary<string, string>(), messageArgs);
                }
            }

        }
    }
}

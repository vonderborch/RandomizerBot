﻿using Discord.WebSocket;
using RandomizerBot.Commands.Internal;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands
{
    public class Version : AbstractBotCommand
    {
        public Version() : base("version", "Prints out the bot's current version", numArguments: 1)
        {
        }

        public override bool ExecuteInternal(Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            SendMessage(messageArgs, $"RandomizerBot Version {Constants.Version}");
            return true;
        }
    }
}

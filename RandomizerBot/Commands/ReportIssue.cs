using Discord.WebSocket;
using RandomizerBot.Commands.Internal;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands
{
    public class ReportIssue : AbstractBotCommand
    {
        public ReportIssue() : base("report_issue", "Prints a link to report an issue with the Bot", numArguments: 1)
        {
        }

        public override bool ExecuteInternal(Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            SendMessage(messageArgs, "Please report an issue using the following link: https://github.com/vonderborch/RandomizerBot/issues/new");
            return true;
        }
    }
}

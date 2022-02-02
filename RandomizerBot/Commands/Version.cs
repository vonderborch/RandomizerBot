using System.Reflection;
using Discord.WebSocket;
using RandomizerBot.Commands.Internal;

namespace RandomizerBot.Commands;

public class Version : AbstractCommand
{
    public Version() : base("version", "Displays the current version to the user.")
    {
    }

    public override void BuildParameterHelper()
    {
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        SendMessage(messageArgs, $"RandomizerBot Version {Assembly.GetEntryAssembly().GetName().Version}");
        return true;
    }
}
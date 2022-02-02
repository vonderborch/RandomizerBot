using System.Text;
using Discord.WebSocket;
using RandomizerBot.Commands.Internal;

namespace RandomizerBot.Commands;

public class Changelog : AbstractCommand
{
    public Changelog() : base("changelog", "Displays a changelog of the bot to the user")
    {
    }

    public override void BuildParameterHelper()
    {
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        if (!File.Exists("Changelog.txt"))
        {
            SendMessage(messageArgs, "Could not find the changelog! Please let the developer know of this issue!");
        }
        else
        {
            var str = new StringBuilder();
            str.Append("```");
            str.Append(File.ReadAllText("Changelog.txt"));
            str.AppendLine("```");

            SendMessage(messageArgs, str.ToString());
        }

        return true;
    }
}
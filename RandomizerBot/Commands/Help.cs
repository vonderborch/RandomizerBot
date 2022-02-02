using System.Text;
using Discord.WebSocket;
using RandomizerBot.Commands.Internal;

namespace RandomizerBot.Commands;

public class Help : AbstractCommand
{
    public Help() : base("help", "Displays a list of all available commands to the user")
    {
    }

    public override void BuildParameterHelper()
    {
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        var maxCommandNameLength = int.MinValue;
        foreach (var cmd in CommandRegister.Instance.Commands)
            if (cmd.Key.Length > maxCommandNameLength)
                maxCommandNameLength = $"!rb_{cmd.Key}".Length;

        var str = new StringBuilder();
        str.AppendLine("Available Commands:```");
        foreach (var cmd in CommandRegister.Instance.Commands)
        {
            var actualName = $"!rb_{cmd.Key}";
            str.AppendLine($"{actualName.PadRight(maxCommandNameLength)} - {cmd.Value.HelpMessage}");
        }

        str.AppendLine("```");

        SendMessage(messageArgs, str.ToString());

        return true;
    }
}
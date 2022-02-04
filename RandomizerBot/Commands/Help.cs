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
        Arguments.Add(new Argument("use_table_formatting", "Use table formatting to display available commands. Defaults to True.", false));
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        var use_table_formatting = true;
        if (args.TryGetValue("use_table_formatting", out var use_table_formattingRaw))
        {
            if (!bool.TryParse(use_table_formattingRaw, out use_table_formatting))
            {
                use_table_formatting = true;
            }
        }

        var maxCommandNameLength = int.MinValue;
        foreach (var cmd in CommandRegister.Instance.Commands)
            if (cmd.Key.Length > maxCommandNameLength)
                maxCommandNameLength = $"!rb_{cmd.Key}".Length;

        var str = new StringBuilder();
        str.AppendLine($"Available Commands (bot v{Constants.Version}):```");
        var seperator = use_table_formatting ? " | " : " - ";
        
        foreach (var cmd in CommandRegister.Instance.Commands)
        {
            var actualName = $"!rb_{cmd.Key}";
            var line = $"{actualName.PadRight(maxCommandNameLength)}{seperator}{cmd.Value.HelpMessage}";

            str.AppendLine(line);
        }

        str.AppendLine("```");

        var output = str.ToString();
        SendMessage(messageArgs, str.ToString());

        return true;
    }
}
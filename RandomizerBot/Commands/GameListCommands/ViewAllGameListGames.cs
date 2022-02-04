using System.Text;
using Discord.WebSocket;
using RandomizerBot.Commands.Internal;

namespace RandomizerBot.Commands.GameListCommands;

public class ViewAllGameListGames : AbstractCommand
{
    public ViewAllGameListGames() : base("view_all_game_lists", "Views all the game lists you have access to")
    {
    }

    public override void BuildParameterHelper()
    {
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        var personalDirectory = messageArgs.Author.Username;
        var serverDirectory = server.Name;

        var str = new StringBuilder();
        str.AppendLine("Available Game Lists: ```");
        if (Directory.Exists(personalDirectory))
        {
            str.AppendLine("Personal Lists:");
            foreach (var list in Directory.EnumerateFiles(personalDirectory))
            {
                str.AppendLine($"  {Path.GetFileNameWithoutExtension(list)}");
            }
            str.AppendLine(" ");
        }
        if (Directory.Exists(serverDirectory))
        {
            str.AppendLine("Server Lists:");
            foreach (var list in Directory.EnumerateFiles(serverDirectory))
            {
                str.AppendLine($"  {Path.GetFileNameWithoutExtension(list)}");
            }
            str.AppendLine(" ");
        }

        str.AppendLine("```");
        SendMessage(messageArgs, str.ToString());

        return true;
    }
}
using Discord.WebSocket;
using RandomizerBot.Commands.Internal;
using RandomizerBot.Helpers;

namespace RandomizerBot.Commands;

public class DeleteGameList : AbstractCommand
{
    public DeleteGameList() : base("delete_game_list", "Deletes a games list")
    {
    }

    public override void BuildParameterHelper()
    {
        Arguments.Add(new Argument("name", "The name of the list to delete"));
        Arguments.Add(new Argument("defaultuserpersonallists", "Whether to use a personal list if a personal list and a server list with the same name exist. Defaults to true (use a personal list if duplicates exist), false will default to the server list.", false));
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        // get the name args...
        if (!args.TryGetValue("name", out var name))
        {
            return false;
        }
        var defaultuserpersonallists = false;
        if (args.TryGetValue("defaultuserpersonallists", out var defaultuserpersonallistsRaw))
            if (!bool.TryParse(defaultuserpersonallistsRaw, out defaultuserpersonallists))
                return false;

        var serverName = NameHelpers.GetListFileName(name, false, messageArgs, server);
        var personalName = NameHelpers.GetListFileName(name, true, messageArgs, server);
        var serverExists = File.Exists(serverName);
        var personalExists = File.Exists(personalName);

        var fileName = personalExists && !serverExists
            ? personalName
            : !personalExists && serverExists
                ? serverName
                : personalExists && serverExists && defaultuserpersonallists
                    ? personalName
                    : serverName;
        
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
            SendMessage(messageArgs, $"A list named {name} (full name: {fileName}) has been deleted!");
        }
        else
        {
            SendMessage(messageArgs, $"A list named {name} did not exist!");
        }

        return true;
    }
}
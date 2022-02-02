using System.Text;
using Discord.WebSocket;
using Newtonsoft.Json;
using RandomizerBot.Commands.GameListCommands.Objects;
using RandomizerBot.Commands.Internal;
using RandomizerBot.Helpers;

namespace RandomizerBot.Commands.GameListCommands;

public class AddGameToGameList : AbstractCommand
{
    public AddGameToGameList() : base("add_game_to_gamelist", "Adds a game to a game list")
    {
    }

    public override void BuildParameterHelper()
    {
        Arguments.Add(new Argument("listname", "The name of the list"));
        Arguments.Add(new Argument("gamename", "The name of the list"));
        Arguments.Add(new Argument("defaultuserpersonallists", "Whether to use a personal list if a personal list and a server list with the same name exist. Defaults to true (use a personal list if duplicates exist), false will default to the server list.", false));
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        // get the args...
        if (!args.TryGetValue("listname", out var listname))
        {
            return false;
        }
        if (!args.TryGetValue("gamename", out var gamename))
        {
            return false;
        }
        var defaultuserpersonallists = false;
        if (args.TryGetValue("defaultuserpersonallists", out var defaultuserpersonallistsRaw))
            if (!bool.TryParse(defaultuserpersonallistsRaw, out defaultuserpersonallists))
                return false;

        var serverName = NameHelpers.GetListFileName(listname, false, messageArgs, server);
        var personalName = NameHelpers.GetListFileName(listname, true, messageArgs, server);
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
            GameList games = null;
            try
            {
                games = JsonConvert.DeserializeObject<GameList>(File.ReadAllText(fileName));
            }
            catch (Exception ex)
            {
                games = null;
            }

            if (games == null)
            {
                SendMessage(messageArgs, $"The list named {listname} at filepath {fileName} is corrupt or un-loadable. Please inform the developer or delete the list!");
            }
            else
            {
                games.Games.Add(new Game(gamename, true));
                File.WriteAllText(fileName, JsonConvert.SerializeObject(games));

                SendMessage(messageArgs, $"The game {gamename} has been added to list named {listname}!");
            }
        }
        else
        {
            SendMessage(messageArgs, $"A list named {listname} did not exist!");
        }

        return true;
    }
}
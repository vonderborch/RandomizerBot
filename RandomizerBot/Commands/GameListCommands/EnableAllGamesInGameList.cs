using System.Text;
using Discord.WebSocket;
using Newtonsoft.Json;
using RandomizerBot.Commands.GameListCommands.Objects;
using RandomizerBot.Commands.Internal;
using RandomizerBot.Helpers;

namespace RandomizerBot.Commands.GameListCommands;

public class EnableAllGamesInGameList : AbstractCommand
{
    public EnableAllGamesInGameList() : base("enable_all_games_in_gamelist", "Enables all games for randomization in a game list")
    {
    }

    public override void BuildParameterHelper()
    {
        Arguments.Add(new Argument("listname", "The name of the list"));
        Arguments.Add(new Argument("defaultuserpersonallists", "Whether to use a personal list if a personal list and a server list with the same name exist. Defaults to true (use a personal list if duplicates exist), false will default to the server list.", false));
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        // get the args...
        if (!args.TryGetValue("listname", out var listname))
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
                for (var i = 0; i < games.Games.Count; i++)
                {
                    games.Games[i].IsEnabled = true;
                }
                File.WriteAllText(fileName, JsonConvert.SerializeObject(games));
                SendMessage(messageArgs, $"All games in the list named {listname} have been enabled for randomization!");
            }
        }
        else
        {
            SendMessage(messageArgs, $"A list named {listname} did not exist!");
        }

        return true;
    }
}
using System.Text;
using Discord.WebSocket;
using Newtonsoft.Json;
using RandomizerBot.Commands.GameListCommands.Objects;
using RandomizerBot.Commands.Internal;
using RandomizerBot.Helpers;

namespace RandomizerBot.Commands.GameListCommands;

public class RandomizeGameListGames : AbstractCommand
{
    private readonly Random _randomizer;

    public RandomizeGameListGames() : base("randomize_game_list_games", "Views the games in a game list")
    {
        _randomizer = new Random();
    }

    public override void BuildParameterHelper()
    {
        Arguments.Add(new Argument("name", "The name of the list"));
        Arguments.Add(new Argument("defaultuserpersonallists", "Whether to use a personal list if a personal list and a server list with the same name exist. Defaults to true (use a personal list if duplicates exist), false will default to the server list.", false));
        Arguments.Add(new Argument("showfirstitemonly", "Shows only the first item in the randomized list. Defaults to False.", false));
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        // get the args...
        if (!args.TryGetValue("name", out var name))
        {
            return false;
        }
        var defaultuserpersonallists = false;
        if (args.TryGetValue("defaultuserpersonallists", out var defaultuserpersonallistsRaw))
            if (!bool.TryParse(defaultuserpersonallistsRaw, out defaultuserpersonallists))
                return false;
        var onlyFirst = false;
        if (args.TryGetValue("showfirstitemonly", out var onlyFirstRaw))
        {
            if (!bool.TryParse(onlyFirstRaw, out onlyFirst))
            {
                return false;
            }
        }

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
                SendMessage(messageArgs, $"The list named {name} at filepath {fileName} is corrupt or un-loadable. Please inform the developer or delete the list!");
            }
            else
            {
                if (games.Games.Count == 0)
                {
                    SendMessage(messageArgs, $"The list named {name} does not contain any games yet!");
                }
                else
                {
                    var randomizedGames = games.Games.Where(x => x.IsEnabled).OrderBy(x => _randomizer.Next()).ToList();


                    var str = new StringBuilder();

                    if (onlyFirst)
                    {
                        str.AppendLine($"Top Game: {randomizedGames[0].Name}");
                    }
                    else
                    {
                        str.AppendLine("Randomized Game List:```");
                        for (var i = 0; i < randomizedGames.Count; i++)
                        {
                            str.AppendLine(randomizedGames[i].Name);
                        }
                        str.Append("```");
                    }


                    SendMessage(messageArgs, str.ToString());
                }
            }
        }
        else
        {
            SendMessage(messageArgs, $"A list named {name} did not exist!");
        }

        return true;
    }
}
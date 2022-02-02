using System.Text;
using Discord.WebSocket;
using Newtonsoft.Json;
using RandomizerBot.Commands.GameListCommands.Objects;
using RandomizerBot.Commands.Internal;
using RandomizerBot.Helpers;

namespace RandomizerBot.Commands.GameListCommands;

public class ViewGameListGames : AbstractCommand
{
    public ViewGameListGames() : base("view_game_list_games", "Views the games in a game list")
    {
    }

    public override void BuildParameterHelper()
    {
        Arguments.Add(new Argument("name", "The name of the list"));
        Arguments.Add(new Argument("defaultuserpersonallists", "Whether to use a personal list if a personal list and a server list with the same name exist. Defaults to true (use a personal list if duplicates exist), false will default to the server list.", false));
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
                    var str = new StringBuilder();
                    str.AppendLine("Games:```");
                    var longestLength = games.Games.Max(x => x.Name.Length) + 1;

                    str.AppendLine($"{("Game Name".PadRight(longestLength))}| Is Enabled For Game Selection");
                    str.AppendLine($"{("-".PadRight(longestLength, '-'))}-------------------------------");
                    for (var i = 0; i < games.Games.Count; i++)
                    {
                        str.AppendLine($"{(games.Games[i].Name.PadRight(longestLength))}| {games.Games[i].IsEnabled}");
                    }

                    str.AppendLine("```");


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
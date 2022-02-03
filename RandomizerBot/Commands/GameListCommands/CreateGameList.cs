using Discord.WebSocket;
using Newtonsoft.Json;
using RandomizerBot.Commands.GameListCommands.Objects;
using RandomizerBot.Commands.Internal;
using RandomizerBot.Helpers;

namespace RandomizerBot.Commands.GameListCommands;

public class CreateGameList : AbstractCommand
{
    public CreateGameList() : base("create_game_list", "Creates a new games list")
    {
    }

    public override void BuildParameterHelper()
    {
        Arguments.Add(new Argument("name", "The name of the list"));
        Arguments.Add(new Argument("ispersonal", "Whether the list is personal or shared with the whole server. Defaults to True.", false));
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        // get the name and ispersonal args...
        if (!args.TryGetValue("name", out var name))
        {
            return false;
        }
        var isPersonal = true;
        if (args.TryGetValue("ispersonal", out var isPersonalRaw))
        {
            if (!bool.TryParse(isPersonalRaw, out isPersonal))
            {
                return false;
            }
        }

        var fileName = NameHelpers.GetListFileName(name, isPersonal, messageArgs, server);

        if (File.Exists(fileName))
        {
            SendMessage(messageArgs, $"A list named {name} (full name: {fileName}) already exists! Please delete the existing list to recreate it!");
        }
        else
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            File.WriteAllText(fileName, JsonConvert.SerializeObject(new GameList(name, isPersonal ? messageArgs.Author.Username : server.Name, isPersonal)));

            SendMessage(messageArgs, $"Created a new list named {name} (full name: {fileName})!");
        }

        return true;
    }
}
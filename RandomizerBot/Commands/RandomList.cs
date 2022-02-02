using System.Text;
using Discord.WebSocket;
using RandomizerBot.Commands.Internal;
using RandomizerBot.Helpers;

namespace RandomizerBot.Commands;

public class RandomList : AbstractCommand
{
    private readonly Random _randomizer;

    public RandomList() : base("randomize_list", "Randomizes a list comma-seperated list of items")
    {
        _randomizer = new Random();
    }

    public override void BuildParameterHelper()
    {
        Arguments.Add(new Argument("items", "A comma-seperated list of items to randomize"));
        Arguments.Add(new Argument("showfirstitemonly", "Shows only the first item in the randomized list. Defaults to False.", false));
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        // get the args...
        if (!args.TryGetValue("items", out var itemsRaw))
        {
            return false;
        }
        var onlyFirst = false;
        if (args.TryGetValue("showfirstitemonly", out var onlyFirstRaw))
        {
            if (!bool.TryParse(onlyFirstRaw, out onlyFirst))
            {
                return false;
            }
        }

        var items = itemsRaw.Split(',').ToList();
        var itemsBackup = new List<string>(items);
        items = items.Select(x => x.Trim()).ToList();
        items = items.OrderBy(x => _randomizer.Next()).ToList();

        var str = new StringBuilder();

        if (onlyFirst)
        {
            str.AppendLine($"Top Item: {items[0]}");
        }
        else
        {
            str.Append("```");
            for (var i = 0; i < items.Count; i++)
            {
                str.AppendLine(items[i]);
            }
            str.Append("```");
        }

        SendMessage(messageArgs, str.ToString());

        return true;
    }
}
using System.Text;
using Discord.WebSocket;

namespace RandomizerBot.Commands.Internal;

public abstract class AbstractDiceRoll : AbstractCommand
{
    protected readonly Random _randomizer;
    protected int _faces = 0;
    protected List<string> _namedFaces;

    protected AbstractDiceRoll(int faces, List<string> namedFaces = null) : base($"roll_d{faces}", $"Rolls a D{faces} dice for the user.")
    {
        _randomizer = new Random();
        _faces = faces;
        _namedFaces = namedFaces == null
            ? new List<string>()
            : new List<string>(namedFaces);

        if (_namedFaces.Count == 0)
        {
            for (var i = 0; i < _faces; i++)
            {
                _namedFaces.Add($"{i + 1} Pips");
            }
        }
    }

    public override void BuildParameterHelper()
    {
        Arguments.Add(new Argument("times", "The number of times to roll the dice. Defaults to 1, must be at least 1.",
            false));
        Arguments.Add(new Argument("showindividualresults", "Whether to show individual results. Defaults to false.",
            false));
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        //// Determine the number of coin flips to execute...
        var timesRaw = "1";
        if (args.Count > 0) args.TryGetValue("times", out timesRaw);

        var times = 0;
        if (!int.TryParse(timesRaw, out times)) return false;
        if (times <= 0) return false;

        //// figure out if we're showing individual results or not
        var showIndividualResults = false;
        if (args.TryGetValue("showindividualresults", out var individualResultsRaw))
            if (!bool.TryParse(individualResultsRaw, out showIndividualResults))
                return false;

        //// Roll the Die!
        var str = new StringBuilder();

        // Path 1 - Single Roll
        if (times == 1)
        {
            var flip = _randomizer.Next(0, _faces);
            str.Append($"Face = {flip + 1} Pips");
        }
        // Path 2 - Multiple Roll
        else
        {
            var faceCount = new List<int>();
            for (var i = 0; i < _faces; i++)
            {
                faceCount.Add(0);
            }

            for (var i = 0; i < times; i++)
            {
                var flip = _randomizer.Next(0, _faces);
                faceCount[flip]++;

                if (showIndividualResults)
                {
                    str.Append($"Roll #{i + 1}: FACE = {flip + 1} Pips");
                }
            }

            if (showIndividualResults)
            {
                str.AppendLine("");
            }

            str.AppendLine($"Totals:");
            for (var i = 0; i < faceCount.Count; i++)
            {
                str.AppendLine($"{i + 1} Pips: {faceCount[i]}");
            }
        }


        SendMessage(messageArgs, str.ToString());
        return true;
    }
}
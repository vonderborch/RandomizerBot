using System.Text;
using Discord.WebSocket;
using RandomizerBot.Commands.Internal;

namespace RandomizerBot.Commands;

public class FlipCoin : AbstractCommand
{
    private readonly Random _randomizer;

    public FlipCoin() : base("flip_coin", "Flips a coin and returns heads or tails depending on the result")
    {
        _randomizer = new Random();
    }

    public override void BuildParameterHelper()
    {
        Arguments.Add(new Argument("times", "The number of times to flip the coin. Defaults to 1, must be at least 1.",
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

        //// Flip the Coin!
        var str = new StringBuilder();

        // Path 1 - Single Flip
        if (times == 1)
        {
            var flip = _randomizer.Next(0, 2);
            str.Append($"{(flip == 1 ? "HEADS" : "TAILS")}");
        }
        // Path 2 - Multiple flips
        else
        {
            var heads = 0;
            var tails = 0;
            for (var i = 0; i < times; i++)
            {
                var flip = _randomizer.Next(0, 2);

                if (flip == 1) heads++;
                if (flip == 0) tails++;

                if (showIndividualResults) str.AppendLine($"Flip #{i + 1}: {(flip == 1 ? "HEADS" : "TAILS")}");
            }

            if (showIndividualResults) str.AppendLine("");
            str.AppendLine($"Totals:");
            str.AppendLine($"  - Heads: {heads}");
            str.AppendLine($"  - Tails: {tails}");
        }


        SendMessage(messageArgs, str.ToString());
        return true;
    }
}
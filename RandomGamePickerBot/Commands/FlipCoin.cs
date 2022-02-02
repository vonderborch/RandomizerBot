using System.Text;
using Discord.WebSocket;
using RandomGamePickerBot.Commands.Internal;

namespace RandomGamePickerBot.Commands
{
    public class FlipCoin : AbstractCommand
    {
        Random _randomizer;

        public FlipCoin() : base("flipcoin", "Flips a coin and returns heads or tails depending on the result")
        {
            _randomizer = new Random();
        }

        public override void BuildParameterHelper()
        {
            Arguments.Add(new Argument("times", "The number of times to flip the coin", false));
        }

        public override void ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs)
        {
            var timesRaw = "1";

            if (args.Count > 0)
            {
                args.TryGetValue("times", out timesRaw);
            }

            int.TryParse(timesRaw, out var times);
            if (times <= 0)
            {
                times = 1;
            }

            var str = new StringBuilder();
            var heads = 0;
            var tails = 0;
            for (var i = 0; i < times; i++)
            {
                var flip = _randomizer.Next(0, 2);

                if (flip == 1)
                {
                    heads++;
                }
                if (flip == 0)
                {
                    tails++;
                }

                str.AppendLine($"Flip #{i + 1}: {(flip == 1 ? "HEADS" : "TAILS")}");
            }

            str.AppendLine("");
            str.AppendLine($"Total Heads: {heads}, Total Tails: {tails}");

            messageArgs.Channel.SendMessageAsync(str.ToString());
        }
    }
}

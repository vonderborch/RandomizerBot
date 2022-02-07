using System.Text;
using Discord.WebSocket;
using RandomizerBot.Commands.Internal;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands
{
    public class GenericDice : AbstractBotCommand
    {
        protected readonly Random _randomizer;

        public GenericDice() : base($"roll_die", $"Rolls a Die with variable sides dice for the user", numArguments: 3)
        {
            _randomizer = new Random();

            AddArgument("faces", "The number of timesfaces on the dice. Defaults to 6, must be at least 2.", typeof(int), 6);
            AddArgument("times", "The number of times to execute the command. Defaults to 1, must be at least 1.", typeof(int), 1);
            AddArgument("show_individual_results", "Whether to show individual results. Defaults to false.", typeof(bool), false);
        }

        public override bool ExecuteInternal(Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            // Get the args...
            var faces = parameters["faces"].Value<int>();
            if (faces <= 1)
            {
                return false;
            }
            var times = parameters["times"].Value<int>();
            if (times <= 0)
            {
                return false;
            }
            var showIndividualResults = parameters["show_individual_results"].Value<bool>();

            //// Roll the Die!
            var str = new StringBuilder();

            // Path 1 - Single Roll
            if (times == 1)
            {
                var roll = _randomizer.Next(0, faces);
                str.Append(GetSingleRollResultText(roll));
            }
            // Path 2 - Multiple Roll
            else
            {
                var faceCount = new List<int>();
                for (var i = 0; i < faces; i++)
                {
                    faceCount.Add(0);
                }

                for (var i = 0; i < times; i++)
                {
                    var roll = _randomizer.Next(0, faces);
                    faceCount[roll]++;

                    if (showIndividualResults)
                    {
                        str.Append(GetIndividualRollResultText(i, roll));
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
            
            SendMessage(messageArgs, parameters["print_as_text_file"], str.ToString());
            return true;
        }

        protected string GetSingleRollResultText(int roll)
        {
            return $"Face = {roll + 1} Pips";
        }

        protected string GetIndividualRollResultText(int rollNumber, int roll)
        {
            return $"Roll #{rollNumber + 1}: FACE = {roll + 1} Pips";
        }

        protected string GetTotalsText(List<int> results)
        {
            var str = new StringBuilder();
            for (var i = 0; i < results.Count; i++)
            {
                str.AppendLine($"  {i + 1} Pips: {results[i]}");
            }
            return str.ToString();
        }
    }
}

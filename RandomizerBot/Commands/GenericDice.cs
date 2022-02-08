/// <file>
/// RandomizerBot\Commands\GenericDice.cs
/// </file>
///
/// <copyright file="GenericDice.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the generic dice class.
/// </summary>
using SimpleDiscordBot.Commands;
using System.Text;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands
{
    /// <summary>
    /// A generic dice.
    /// </summary>
    ///
    /// <seealso cref="AbstractBotCommand"/>
    public class GenericDice : AbstractBotCommand
    {
        /// <summary>
        /// (Immutable) the randomizer.
        /// </summary>
        protected readonly Random _randomizer;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GenericDice() : base($"roll_die", $"Rolls a Die with variable sides dice for the user", numArguments: 3)
        {
            _randomizer = new Random();

            AddArgument("faces", "The number of timesfaces on the dice. Defaults to 6, must be at least 2.", typeof(int), 6);
            AddArgument("times", "The number of times to execute the command. Defaults to 1, must be at least 1.", typeof(int), 1);
            AddArgument("show_individual_results", "Whether to show individual results. Defaults to false.", typeof(bool), false);
        }

        /// <summary>
        /// Executes the 'internal' operation.
        /// </summary>
        ///
        /// <param name="parameters">   Options for controlling the operation. </param>
        /// <param name="messageInfo">  Information describing the message. </param>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        public override bool ExecuteInternal(Dictionary<string, IParameter> parameters, MessageInfo messageInfo)
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

            SendMessage(str.ToString(), messageInfo, true);
            return true;
        }

        /// <summary>
        /// Gets single roll result text.
        /// </summary>
        ///
        /// <param name="roll"> The roll. </param>
        ///
        /// <returns>
        /// The single roll result text.
        /// </returns>
        protected string GetSingleRollResultText(int roll)
        {
            return $"Face = {roll + 1} Pips";
        }

        /// <summary>
        /// Gets individual roll result text.
        /// </summary>
        ///
        /// <param name="rollNumber">   The roll number. </param>
        /// <param name="roll">         The roll. </param>
        ///
        /// <returns>
        /// The individual roll result text.
        /// </returns>
        protected string GetIndividualRollResultText(int rollNumber, int roll)
        {
            return $"Roll #{rollNumber + 1}: FACE = {roll + 1} Pips";
        }

        /// <summary>
        /// Gets totals text.
        /// </summary>
        ///
        /// <param name="results">  The results. </param>
        ///
        /// <returns>
        /// The totals text.
        /// </returns>
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
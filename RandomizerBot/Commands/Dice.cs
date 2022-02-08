/// <file>
/// RandomizerBot\Commands\Dice.cs
/// </file>
///
/// <copyright file="Dice.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the dice class.
/// </summary>
using SimpleDiscordBot.Commands;
using System.Text;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands
{
    /// <summary>
    /// A dice.
    /// </summary>
    ///
    /// <seealso cref="AbstractBotCommand"/>
    public class Dice : AbstractBotCommand
    {
        /// <summary>
        /// (Immutable) the randomizer.
        /// </summary>
        protected readonly Random _randomizer;

        /// <summary>
        /// The faces.
        /// </summary>
        protected int _faces = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="faces">                The faces. </param>
        /// <param name="nameOverride">         (Optional) The name override. </param>
        /// <param name="descriptionOverride">  (Optional) The description override. </param>
        /// <param name="isHidden">             (Optional) True if is hidden, false if not. </param>
        public Dice(int faces, string nameOverride = "", string descriptionOverride = "", bool isHidden = false) : base(string.IsNullOrWhiteSpace(nameOverride) ? $"roll_d{faces}" : nameOverride, $"Rolls a D{faces} dice for the user", numArguments: 2, isHidden: isHidden)
        {
            _randomizer = new Random();
            _faces = faces;

            if (!string.IsNullOrWhiteSpace(descriptionOverride))
            {
                Description = descriptionOverride;
            }

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
                var roll = _randomizer.Next(0, _faces);
                str.Append(GetSingleRollResultText(roll));
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
                    var roll = _randomizer.Next(0, _faces);
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
        protected virtual string GetSingleRollResultText(int roll)
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
        protected virtual string GetIndividualRollResultText(int rollNumber, int roll)
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
        protected virtual string GetTotalsText(List<int> results)
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
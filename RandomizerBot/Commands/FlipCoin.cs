/// <file>
/// RandomizerBot\Commands\FlipCoin.cs
/// </file>
///
/// <copyright file="FlipCoin.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the flip coin class.
/// </summary>
using System.Text;

namespace RandomizerBot.Commands
{
    /// <summary>
    /// A flip coin.
    /// </summary>
    ///
    /// <seealso cref="Dice"/>
    public class FlipCoin : Dice
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public FlipCoin() : base(2, "flip_coin", "Flips a coin and returns heads or tails depending on the result")
        {
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
        ///
        /// <seealso cref="RandomizerBot.Commands.Dice.GetSingleRollResultText(int)"/>
        protected override string GetSingleRollResultText(int roll)
        {
            return roll == 1 ? "HEADS" : "TAILS";
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
        ///
        /// <seealso cref="RandomizerBot.Commands.Dice.GetIndividualRollResultText(int,int)"/>
        protected override string GetIndividualRollResultText(int rollNumber, int roll)
        {
            return $"Flip #{rollNumber + 1}: {(roll == 1 ? "HEADS" : "TAILS")}";
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
        ///
        /// <seealso cref="RandomizerBot.Commands.Dice.GetTotalsText(List{int})"/>
        protected override string GetTotalsText(List<int> results)
        {
            var str = new StringBuilder();
            str.AppendLine($"  Heads: {results[1]}");
            str.AppendLine($"  Tails: {results[0]}");

            return str.ToString();
        }
    }
}
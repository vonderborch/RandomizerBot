using System.Text;

namespace RandomizerBot.Commands
{
    public class FlipCoin : Dice
    {
        public FlipCoin() : base(2, "flip_coin", "Flips a coin and returns heads or tails depending on the result")
        {
        }

        protected override string GetSingleRollResultText(int roll)
        {
            return roll == 1 ? "HEADS" : "TAILS";
        }

        protected override string GetIndividualRollResultText(int rollNumber, int roll)
        {
            return $"Flip #{rollNumber + 1}: {(roll == 1 ? "HEADS" : "TAILS")}";
        }

        protected override string GetTotalsText(List<int> results)
        {
            var str = new StringBuilder();
            str.AppendLine($"  Heads: {results[1]}");
            str.AppendLine($"  Tails: {results[0]}");

            return str.ToString();
        }
    }
}

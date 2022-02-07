using System.Text;
using Discord.WebSocket;
using RandomizerBot.Commands.Internal;
using RandomizerBot.Helpers;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands
{
    public class RandomList : AbstractBotCommand
    {
        protected readonly Random _randomizer;

        public RandomList() : base($"random_list", $"Randomizes a comma-separated list of items.", numArguments: 3)
        {
            _randomizer = new Random();

            AddArgument("items", "A comma-separated list of items to randomize.", typeof(string), "", true);
            AddArgument("show_first_item_only", "Shows only the first item in the randomized list. Defaults to False.", typeof(bool), false);
            AddArgument("show_original_list", "Shows the list in the original order. Defaults to False.", typeof(bool), false);
        }

        public override bool ExecuteInternal(Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            // Get the args...
            var itemsRaw = parameters["items"].RawValue;
            var items = itemsRaw.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            if (items.Count == 0)
            {
                return false;
            }
            var showFirstItemOnly = parameters["show_first_item_only"].Value<bool>();
            var showOriginalList = parameters["show_original_list"].Value<bool>();

            var str = new StringBuilder();

            //// Randomize the list
            var itemsBackup = new List<string>(items);
            items = items.OrderBy(x => _randomizer.Next()).ToList();

            if (showOriginalList)
            {
                str.AppendLine("Randomized Result:");
            }
            if (showFirstItemOnly)
            {
                str.AppendLine($"Top Item: {items[0]}");
            }
            else
            {
                for (var i = 0; i < items.Count; i++)
                {
                    str.AppendLine(items[i]);
                }
            }

            if (showOriginalList)
            {
                str.AppendLine("Original List:");
                for (var i = 0; i < itemsBackup.Count; i++)
                {
                    str.AppendLine(itemsBackup[i]);
                }
            }
            
            SendMessage(messageArgs, parameters["print_as_text_file"], str.ToString());
            return true;
        }
    }
}

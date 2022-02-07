using System.Text;
using Discord.WebSocket;
using RandomizerBot.Commands.ItemListCommands.Objects;
using Velentr.Miscellaneous.CommandParsing;
using Velentr.Miscellaneous.StringHelpers;

namespace RandomizerBot.Commands.ItemListCommands
{
    public class RandomizeItemList : AbstractItemListCommand
    {
        private readonly Random _randomizer;

        public RandomizeItemList() : base("itemlist_randomize", "Randomizes an item list")
        {
            _randomizer = new Random();

            AddArgument<bool>("show_original_list", "Whether to show the original list order", false);
            AddArgument<bool>("show_first_item_only", "Whether to show only the first item in the randomized list order", true);
        }

        public override bool ExecuteInternal(ListKey key, ListKey personalKey, ListKey serverKey, bool personalExists, bool serverExists, Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            var items = Database.Instance.DB.GetItemListItems(key);
            if (items.Count == 0)
            {
                // no items :(
                SendMessage(messageArgs, parameters["print_as_text_file"], "There are no items in the specified list!");
                return true;
            }

            var enabledItems = items.Where(x => x.IsEnabledForRandomization).ToList();
            if (enabledItems.Count == 0)
            {
                SendMessage(messageArgs, parameters["print_as_text_file"], "There are no items enabled for randomization in the specified list!");
                return true;
            }

            var randomOrder = enabledItems.OrderBy(x => _randomizer.NextDouble() * x.Weight).ToList();

            var showOriginal = parameters["show_original_list"].Value<bool>();
            var showAll = !parameters["show_first_item_only"].Value<bool>();

            if (!showAll && !showOriginal)
            {
                SendMessage(messageArgs, $"Top Randomized Item: {randomOrder[0].Name}");
            }
            else
            {
                var str = new StringBuilder();

                var columns = new List<string>()
                {
                    "Name"
                };
                var randomListString = randomOrder.Select(x => x.GetText(columns)).ToList();
                str.AppendLine("Randomized List:");
                str.AppendLine(TableOutputHelper.ConvertToTable(columns, randomListString));

                if (showOriginal)
                {
                    var originalList = enabledItems.Select(x => x.GetText(columns)).ToList();
                    str.AppendLine(" ");
                    str.AppendLine("Original List:");
                    str.AppendLine(TableOutputHelper.ConvertToTable(columns, originalList, addSeparatorText: false, includeHeaders: false));
                }

                SendMessage(messageArgs, parameters["print_as_text_file"], str.ToString());
            }


            return true;
        }
    }
}

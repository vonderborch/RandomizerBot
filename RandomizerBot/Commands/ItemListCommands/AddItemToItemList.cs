using Discord.WebSocket;
using RandomizerBot.Commands.ItemListCommands.Objects;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands.ItemListCommands
{
    public class AddItemToItemList : AbstractItemListCommand
    {
        public AddItemToItemList() : base("itemlist_add_item", "Adds an item to an item list")
        {
            AddArgument<string>("new_item", "The new item to add. If it contains commas, each comma-separated value will be treated as a different item to add", string.Empty, true);
            AddArgument<int>("weight", "The weight for the item in randomization. Must be an integer, higher numbers = more chance for the item to be selected if enabled", 1, false);
        }

        public override bool ExecuteInternal(ListKey key, ListKey personalKey, ListKey serverKey, bool personalExists, bool serverExists, Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            // get and validate args
            var rawItem = parameters["new_item"].Value<string>();
            if (string.IsNullOrWhiteSpace(rawItem))
            {
                SendMessage(messageArgs, "A valid non-empty name must be provided!");
                return true;
            }
            var weight = parameters["weight"].Value<int>();
            if (weight < 1)
            {
                SendMessage(messageArgs, "A valid weight of at least 1 must be provided!");
                return true;
            }

            var items = rawItem.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                // Check if the item already exists
                if (Database.Instance.DB.ItemInItemListExists(key, item))
                {
                    SendMessage(messageArgs, $"An item with the name [{item}] already exists in the list [{key.Name}]!");
                    Thread.Sleep(250);
                    continue;
                }

                // add the item to the list
                var result = Database.Instance.DB.AddItemToItemList(key, item, weight);
                if (result == -1)
                {
                    SendMessage(messageArgs, $"An item with the name [{item}] already exists in the {(key.IsPersonal ? "personal" : "server-owned")} list with the name [{key.Name}]!");
                    Thread.Sleep(250);
                    continue;
                }
            }
            SendMessage(messageArgs, $"All requested items ({rawItem}) have been added to the {(key.IsPersonal ? "personal" : "server-owned")} list with the name [{key.Name}]!");

            return true;
        }
    }
}

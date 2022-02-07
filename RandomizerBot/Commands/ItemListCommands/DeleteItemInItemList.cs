using Discord.WebSocket;
using RandomizerBot.Commands.ItemListCommands.Objects;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands.ItemListCommands
{
    public class DeleteItemInItemList : AbstractItemListCommand
    {
        public DeleteItemInItemList() : base("itemlist_delete_item", "Deletes an item in an item list")
        {
            AddArgument<string>("item_to_delete", "The new item to add. If it contains commas, each comma-separated value will be treated as a different item to delete", string.Empty, true);
        }

        public override bool ExecuteInternal(ListKey key, ListKey personalKey, ListKey serverKey, bool personalExists, bool serverExists, Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            // get and validate args
            var rawItem = parameters["item_to_delete"].Value<string>();
            if (string.IsNullOrWhiteSpace(rawItem))
            {
                SendMessage(messageArgs, "A valid non-empty name must be provided!");
                return true;
            }

            var items = rawItem.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                // Check if the item already exists
                if (!Database.Instance.DB.ItemInItemListExists(key, item))
                {
                    SendMessage(messageArgs, $"An item with the name [{item}] does not exist in the list [{key.Name}]!");
                    Thread.Sleep(250);
                    continue;
                }

                // deletes the item to the list
                Database.Instance.DB.DeleteItemInList(key, item);
            }

            SendMessage(messageArgs, $"All requested items ({rawItem}) have been deleted from the {(key.IsPersonal ? "personal" : "server-owned")} list with the name [{key.Name}]!");

            return true;
        }
    }
}

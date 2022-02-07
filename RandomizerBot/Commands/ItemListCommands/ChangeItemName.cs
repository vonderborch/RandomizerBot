using Discord.WebSocket;
using RandomizerBot.Commands.ItemListCommands.Objects;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands.ItemListCommands
{
    public class ChangeItemName : AbstractItemListCommand
    {
        public ChangeItemName() : base("itemlist_change_item_name", "Changes the name of an item in an item list")
        {
            AddArgument<string>("item", "The item to update", string.Empty, true);
            AddArgument<string>("new_name", "The new name for the item", string.Empty, true);
        }

        public override bool ExecuteInternal(ListKey key, ListKey personalKey, ListKey serverKey, bool personalExists, bool serverExists, Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            // get and validate args
            var item = parameters["item"].Value<string>();
            if (string.IsNullOrWhiteSpace(item))
            {
                SendMessage(messageArgs, "A valid non-empty name must be provided!");
                return true;
            }
            var newName = parameters["new_name"].Value<string>();
            if (string.IsNullOrWhiteSpace(newName))
            {
                SendMessage(messageArgs, "A valid non-empty new name must be provided!");
                return true;
            }

            // Check if the item already exists
            if (!Database.Instance.DB.ItemInItemListExists(key, item))
            {
                SendMessage(messageArgs, $"An item with the name [{item}] does not exist in the list [{key.Name}]!");
                return true;
            }

            // change the item weight in the list
            Database.Instance.DB.UpdateItemInList(key, item, newName);
            SendMessage(messageArgs, $"The name of the item [{item}] in the {(key.IsPersonal ? "personal" : "server-owned")} list with the name [{key.Name}] has been updated to [{newName}]!");

            return true;
        }
    }
}

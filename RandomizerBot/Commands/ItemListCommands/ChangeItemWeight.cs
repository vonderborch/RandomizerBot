using Discord.WebSocket;
using RandomizerBot.Commands.ItemListCommands.Objects;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands.ItemListCommands
{
    public class ChangeItemWeight : AbstractItemListCommand
    {
        public ChangeItemWeight() : base("itemlist_change_item_weight", "Changes the weight of an item in an item list")
        {
            AddArgument<string>("item", "The item to update", string.Empty, true);
            AddArgument<int>("new_weight", "The weight for the item in randomization. Must be an integer, higher numbers = more chance for the item to be selected if enabled", 1, true);
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
            var weight = parameters["new_weight"].Value<int>();
            if (weight < 1)
            {
                SendMessage(messageArgs, "A valid weight of at least 1 must be provided!");
                return true;
            }
            
            // Check if the item already exists
            if (!Database.Instance.DB.ItemInItemListExists(key, item))
            {
                SendMessage(messageArgs, $"An item with the name [{item}] does not exist in the list [{key.Name}]!");
                return true;
            }

            // change the item weight in the list
            Database.Instance.DB.UpdateItemInList(key, item, item, weight);
            SendMessage(messageArgs, $"The weight of the item [{item}] in the {(key.IsPersonal ? "personal" : "server-owned")} list with the name [{key.Name}] has been updated to [{weight}]!");

            return true;
        }
    }
}

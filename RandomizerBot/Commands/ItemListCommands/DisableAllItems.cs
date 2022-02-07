using Discord.WebSocket;
using RandomizerBot.Commands.ItemListCommands.Objects;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands.ItemListCommands
{
    public class DisableAllItems : AbstractItemListCommand
    {
        public DisableAllItems() : base("itemlist_disable_all_items", "Disables all in an item list for randomization")
        {
        }

        public override bool ExecuteInternal(ListKey key, ListKey personalKey, ListKey serverKey, bool personalExists, bool serverExists, Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            // is this the best approach? no. We should be doing a single update statement.
            // does it work since it's a local DB? yes!
            // should we change it in the future? yes!
            var currentItems = Database.Instance.DB.GetItemListItems(key);
            for (var i = 0; i < currentItems.Count; i++)
            {
                // updates the item
                Database.Instance.DB.UpdateItemInList(key, currentItems[i].Name, currentItems[i].Name, isEnabled: false);
            }

            SendMessage(messageArgs, $"All items in the {(key.IsPersonal ? "personal" : "server-owned")} list with the name [{key.Name}] have been disabled for randomization!");

            return true;
        }
    }
}

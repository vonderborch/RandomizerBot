using Discord.WebSocket;
using RandomizerBot.Commands.ItemListCommands.Objects;
using Velentr.Miscellaneous.CommandParsing;
using Velentr.Miscellaneous.StringHelpers;

namespace RandomizerBot.Commands.ItemListCommands
{
    public class ViewItemListItems : AbstractItemListCommand
    {
        public ViewItemListItems() : base("itemlist_view_items", "Views all items in an item list")
        {
        }

        public override bool ExecuteInternal(ListKey key, ListKey personalKey, ListKey serverKey, bool personalExists, bool serverExists, Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            var lists = Database.Instance.DB.GetItemListItems(key);
            if (lists.Count == 0)
            {
                // no items :(
                SendMessage(messageArgs, parameters["print_as_text_file"], "There are no items in the specified list!");
                return true;
            }
            
            var columns = new List<string>()
            {
                "Name", "Randomization Enabled?", "Randomization Weight"
            };
            var rows = lists.Select(x => x.GetText(columns)).ToList();
            var table = TableOutputHelper.ConvertToTable(columns, rows);

            SendMessage(messageArgs, parameters["print_as_text_file"], table);

            return true;
        }
    }
}

using System.Text;
using Discord.WebSocket;
using RandomizerBot.Commands.Internal;
using RandomizerBot.Commands.ItemListCommands.Objects;
using Velentr.Miscellaneous.CommandParsing;
using Velentr.Miscellaneous.StringHelpers;

namespace RandomizerBot.Commands.ItemListCommands
{
    public class ViewItemLists : AbstractBotCommand
    {
        public ViewItemLists() : base("itemlist_view_all", "Views all available item lists")
        {
            AddArgument("show_personal_lists", "Whether to display personal lists", typeof(bool), true, false);
            AddArgument("show_server_lists", "Whether to display server lists", typeof(bool), true, false);
        }

        public override bool ExecuteInternal(Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            var showPersonal = parameters["show_personal_lists"].Value<bool>();
            var showServer = parameters["show_server_lists"].Value<bool>();

            var lists = Database.Instance.DB.ViewAllLists(new ListKey(messageArgs, server));
            if (lists.Count == 0)
            {
                // no lists :(
                SendMessage(messageArgs, parameters["print_as_text_file"], "There are no personal or server lists available!");
                return true;
            }
            
            var columns = new List<string>()
            {
                "Name", "Is Server List", "# Items"
            };
            var rows = lists.Select(x => x.GetText(columns)).ToList();
            var table = TableOutputHelper.ConvertToTable(columns, rows);

            SendMessage(messageArgs, parameters["print_as_text_file"], table);

            return true;
        }
    }
}

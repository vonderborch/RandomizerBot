/// <file>
/// RandomizerBot\Commands\ItemListCommands\ViewItemLists.cs
/// </file>
///
/// <copyright file="ViewItemLists.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the view item lists class.
/// </summary>
using RandomizerBot.Commands.ItemListCommands.Objects;
using SimpleDiscordBot.Commands;
using Velentr.Miscellaneous.CommandParsing;
using Velentr.Miscellaneous.StringHelpers;

namespace RandomizerBot.Commands.ItemListCommands
{
    /// <summary>
    /// A view item lists.
    /// </summary>
    ///
    /// <seealso cref="AbstractBotCommand"/>
    public class ViewItemLists : AbstractBotCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ViewItemLists() : base("itemlist_view_all", "Views all available item lists")
        {
            AddArgument("show_personal_lists", "Whether to display personal lists", typeof(bool), true, false);
            AddArgument("show_server_lists", "Whether to display server lists", typeof(bool), true, false);
        }

        /// <summary>
        /// Executes the 'internal' operation.
        /// </summary>
        ///
        /// <param name="parameters">   Options for controlling the operation. </param>
        /// <param name="messageInfo">  Information describing the message. </param>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        public override bool ExecuteInternal(Dictionary<string, IParameter> parameters, MessageInfo messageInfo)
        {
            var showPersonal = parameters["show_personal_lists"].Value<bool>();
            var showServer = parameters["show_server_lists"].Value<bool>();

            var lists = Database.Instance.DB.ViewAllLists(new ListKey(messageInfo.DiscordMessageInfo.SocketMessage, messageInfo.DiscordMessageInfo.Server));
            if (lists.Count == 0)
            {
                // no lists :(
                SendMessage("There are no personal or server lists available!", messageInfo, false);
                return true;
            }

            var columns = new List<string>()
            {
                "Name", "Is Server List", "# Items"
            };
            var rows = lists.Select(x => x.GetText(columns)).ToList();
            var table = TableOutputHelper.ConvertToTable(columns, rows);

            SendMessage(table, messageInfo, true);

            return true;
        }
    }
}
/// <file>
/// RandomizerBot\Commands\ItemListCommands\ViewItemListItems.cs
/// </file>
///
/// <copyright file="ViewItemListItems.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the view item list items class.
/// </summary>
using RandomizerBot.Commands.ItemListCommands.Objects;
using SimpleDiscordBot.Commands;
using Velentr.Miscellaneous.StringHelpers;

namespace RandomizerBot.Commands.ItemListCommands
{
    /// <summary>
    /// A view item list items.
    /// </summary>
    ///
    /// <seealso cref="AbstractItemListCommand"/>
    public class ViewItemListItems : AbstractItemListCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ViewItemListItems() : base("itemlist_view_items", "Views all items in an item list")
        {
        }

        /// <summary>
        /// Executes the 'internal' operation.
        /// </summary>
        ///
        /// <param name="itemListParameters">   Options for controlling the item list. </param>
        /// <param name="messageInfo">          Information describing the message. </param>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        ///
        /// <seealso cref="RandomizerBot.Commands.ItemListCommands.AbstractItemListCommand.ExecuteInternal(Parameters,MessageInfo)"/>
        public override bool ExecuteInternal(Parameters itemListParameters, MessageInfo messageInfo)
        {
            var lists = Database.Instance.DB.GetItemListItems(itemListParameters.Key);
            if (lists.Count == 0)
            {
                // no items :(
                SendMessage("There are no items in the specified list!", messageInfo);
                return true;
            }

            var columns = new List<string>()
            {
                "Name", "Randomization Enabled?", "Randomization Weight"
            };
            var rows = lists.Select(x => x.GetText(columns)).ToList();
            var table = TableOutputHelper.ConvertToTable(columns, rows);

            SendMessage(table, messageInfo, true);

            return true;
        }
    }
}
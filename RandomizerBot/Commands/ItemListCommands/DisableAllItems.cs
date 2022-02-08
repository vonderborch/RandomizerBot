/// <file>
/// RandomizerBot\Commands\ItemListCommands\DisableAllItems.cs
/// </file>
///
/// <copyright file="DisableAllItems.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the disable all items class.
/// </summary>
using RandomizerBot.Commands.ItemListCommands.Objects;
using SimpleDiscordBot.Commands;

namespace RandomizerBot.Commands.ItemListCommands
{
    /// <summary>
    /// A disable all items.
    /// </summary>
    ///
    /// <seealso cref="AbstractItemListCommand"/>
    public class DisableAllItems : AbstractItemListCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DisableAllItems() : base("itemlist_disable_all_items", "Disables all in an item list for randomization")
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
            // is this the best approach? no. We should be doing a single update statement.
            // does it work since it's a local DB? yes!
            // should we change it in the future? yes!
            var currentItems = Database.Instance.DB.GetItemListItems(itemListParameters.Key);
            for (var i = 0; i < currentItems.Count; i++)
            {
                // updates the item
                Database.Instance.DB.UpdateItemInList(itemListParameters.Key, currentItems[i].Name, currentItems[i].Name, isEnabled: false);
            }

            SendMessage($"All items in the {(itemListParameters.Key.IsPersonal ? "personal" : "server-owned")} list with the name [{itemListParameters.Key.Name}] have been disabled for randomization!", messageInfo);

            return true;
        }
    }
}
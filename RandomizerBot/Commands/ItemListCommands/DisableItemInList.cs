/// <file>
/// RandomizerBot\Commands\ItemListCommands\DisableItemInList.cs
/// </file>
///
/// <copyright file="DisableItemInList.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the disable item in list class.
/// </summary>
using RandomizerBot.Commands.ItemListCommands.Objects;
using SimpleDiscordBot.Commands;

namespace RandomizerBot.Commands.ItemListCommands
{
    /// <summary>
    /// List of disable item ins.
    /// </summary>
    ///
    /// <seealso cref="AbstractItemListCommand"/>
    public class DisableItemInList : AbstractItemListCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DisableItemInList() : base("itemlist_disables_item", "Disables an item in an item list for randomization")
        {
            AddArgument<string>("item", "The item to update If it contains commas, each comma-separated value will be treated as a different item to enable", string.Empty, true);
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
            // get and validate args
            var rawItem = messageInfo.CommandParameters["item"].Value<string>();
            if (string.IsNullOrWhiteSpace(rawItem))
            {
                SendMessage("A valid non-empty name must be provided!", messageInfo);
                return true;
            }

            var items = rawItem.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                // Check if the item already exists
                if (!Database.Instance.DB.ItemInItemListExists(itemListParameters.Key, item))
                {
                    SendMessage($"An item with the name [{item}] does not exist in the list [{itemListParameters.Key.Name}]!", messageInfo);
                    Thread.Sleep(250);
                    continue;
                }

                // updates the item
                Database.Instance.DB.UpdateItemInList(itemListParameters.Key, item, item, isEnabled: false);
            }

            SendMessage($"All requested items ({rawItem}) have been disabled for randomization in the {(itemListParameters.Key.IsPersonal ? "personal" : "server-owned")} list with the name [{itemListParameters.Key.Name}]!", messageInfo);

            return true;
        }
    }
}
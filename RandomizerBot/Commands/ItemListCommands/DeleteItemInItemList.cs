/// <file>
/// RandomizerBot\Commands\ItemListCommands\DeleteItemInItemList.cs
/// </file>
///
/// <copyright file="DeleteItemInItemList.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the delete item in item list class.
/// </summary>
using RandomizerBot.Commands.ItemListCommands.Objects;
using SimpleDiscordBot.Commands;

namespace RandomizerBot.Commands.ItemListCommands
{
    /// <summary>
    /// List of delete item in items.
    /// </summary>
    ///
    /// <seealso cref="AbstractItemListCommand"/>
    public class DeleteItemInItemList : AbstractItemListCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DeleteItemInItemList() : base("itemlist_delete_item", "Deletes an item in an item list", needsModPerms: true)
        {
            AddArgument<string>("item_to_delete", "The new item to add. If it contains commas, each comma-separated value will be treated as a different item to delete", string.Empty, true);
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
            var rawItem = messageInfo.CommandParameters["item_to_delete"].Value<string>();
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

                // deletes the item to the list
                Database.Instance.DB.DeleteItemInList(itemListParameters.Key, item);
            }

            SendMessage($"All requested items ({rawItem}) have been deleted from the {(itemListParameters.Key.IsPersonal ? "personal" : "server-owned")} list with the name [{itemListParameters.Key.Name}]!", messageInfo);

            return true;
        }
    }
}
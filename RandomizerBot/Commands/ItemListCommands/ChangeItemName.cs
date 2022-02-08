/// <file>
/// RandomizerBot\Commands\ItemListCommands\ChangeItemName.cs
/// </file>
///
/// <copyright file="ChangeItemName.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the change item name class.
/// </summary>
using RandomizerBot.Commands.ItemListCommands.Objects;
using SimpleDiscordBot.Commands;

namespace RandomizerBot.Commands.ItemListCommands
{
    /// <summary>
    /// A change item name.
    /// </summary>
    ///
    /// <seealso cref="AbstractItemListCommand"/>
    public class ChangeItemName : AbstractItemListCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ChangeItemName() : base("itemlist_change_item_name", "Changes the name of an item in an item list")
        {
            AddArgument<string>("item", "The item to update", string.Empty, true);
            AddArgument<string>("new_name", "The new name for the item", string.Empty, true);
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
            var item = messageInfo.CommandParameters["item"].Value<string>();
            if (string.IsNullOrWhiteSpace(item))
            {
                SendMessage("A valid non-empty name must be provided!", messageInfo);
                return true;
            }
            var newName = messageInfo.CommandParameters["new_name"].Value<string>();
            if (string.IsNullOrWhiteSpace(newName))
            {
                SendMessage("A valid non-empty new name must be provided!", messageInfo);
                return true;
            }

            // Check if the item already exists
            if (!Database.Instance.DB.ItemInItemListExists(itemListParameters.Key, item))
            {
                SendMessage($"An item with the name [{item}] does not exist in the list [{itemListParameters.Key.Name}]!", messageInfo);
                return true;
            }

            // change the item weight in the list
            Database.Instance.DB.UpdateItemInList(itemListParameters.Key, item, newName);
            SendMessage($"The name of the item [{item}] in the {(itemListParameters.Key.IsPersonal ? "personal" : "server-owned")} list with the name [{itemListParameters.Key.Name}] has been updated to [{newName}]!", messageInfo);

            return true;
        }
    }
}
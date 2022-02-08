/// <file>
/// RandomizerBot\Commands\ItemListCommands\ToggleItemInList.cs
/// </file>
///
/// <copyright file="ToggleItemInList.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the toggle item in list class.
/// </summary>
using RandomizerBot.Commands.ItemListCommands.Objects;
using SimpleDiscordBot.Commands;

namespace RandomizerBot.Commands.ItemListCommands
{
    /// <summary>
    /// List of toggle item ins.
    /// </summary>
    ///
    /// <seealso cref="AbstractItemListCommand"/>
    public class ToggleItemInList : AbstractItemListCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ToggleItemInList() : base("itemlist_toggle_item", "Toggles an item in an item list for randomization")
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
                SendMessage("A valid non-empty name must be provided!", messageInfo, false);
                return true;
            }

            var items = rawItem.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var currentItems = Database.Instance.DB.GetItemListItems(itemListParameters.Key);
            var currentItemsNames = currentItems.Select(x => x.Name).ToList();

            for (var i = 0; i < items.Length; i++)
            {
                if (!currentItemsNames.Contains(items[i]))
                {
                    SendMessage($"An item with the name [{items[i]}] does not exist in the list [{itemListParameters.Key.Name}]!", messageInfo);
                    Thread.Sleep(250);
                    continue;
                }

                // updates the item
                Database.Instance.DB.UpdateItemInList(itemListParameters.Key, currentItems[i].Name, currentItems[i].Name, isEnabled: !currentItems[i].IsEnabledForRandomization);
            }

            SendMessage($"All requested items ({rawItem}) have been toggled for randomization in the {(itemListParameters.Key.IsPersonal ? "personal" : "server-owned")} list with the name [{itemListParameters.Key.Name}]!", messageInfo, false);

            return true;
        }
    }
}
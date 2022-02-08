/// <file>
/// RandomizerBot\Commands\ItemListCommands\AddItemToItemList.cs
/// </file>
///
/// <copyright file="AddItemToItemList.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the add item to item list class.
/// </summary>
using RandomizerBot.Commands.ItemListCommands.Objects;
using SimpleDiscordBot.Commands;

namespace RandomizerBot.Commands.ItemListCommands
{
    /// <summary>
    /// List of add item to items.
    /// </summary>
    ///
    /// <seealso cref="AbstractItemListCommand"/>
    public class AddItemToItemList : AbstractItemListCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public AddItemToItemList() : base("itemlist_add_item", "Adds an item to an item list")
        {
            AddArgument<string>("new_item", "The new item to add. If it contains commas, each comma-separated value will be treated as a different item to add", string.Empty, true);
            AddArgument<int>("weight", "The weight for the item in randomization. Must be an integer, higher numbers = more chance for the item to be selected if enabled", 1, false);
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
            var rawItem = messageInfo.CommandParameters["new_item"].Value<string>();
            if (string.IsNullOrWhiteSpace(rawItem))
            {
                SendMessage("A valid non-empty name must be provided!", messageInfo);
                return true;
            }
            var weight = messageInfo.CommandParameters["weight"].Value<int>();
            if (weight < 1)
            {
                SendMessage("A valid weight of at least 1 must be provided!", messageInfo);
                return true;
            }

            var items = rawItem.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                // Check if the item already exists
                if (Database.Instance.DB.ItemInItemListExists(itemListParameters.Key, item))
                {
                    SendMessage($"An item with the name [{item}] already exists in the list [{itemListParameters.Key.Name}]!", messageInfo);
                    Thread.Sleep(250);
                    continue;
                }

                // add the item to the list
                var result = Database.Instance.DB.AddItemToItemList(itemListParameters.Key, item, weight);
                if (result == -1)
                {
                    SendMessage($"An item with the name [{item}] already exists in the {(itemListParameters.Key.IsPersonal ? "personal" : "server-owned")} list with the name [{itemListParameters.Key.Name}]!", messageInfo);
                    Thread.Sleep(250);
                    continue;
                }
            }
            SendMessage($"All requested items ({rawItem}) have been added to the {(itemListParameters.Key.IsPersonal ? "personal" : "server-owned")} list with the name [{itemListParameters.Key.Name}]!", messageInfo);

            return true;
        }
    }
}
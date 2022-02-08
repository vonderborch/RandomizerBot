/// <file>
/// RandomizerBot\Commands\ItemListCommands\ChangeItemWeight.cs
/// </file>
///
/// <copyright file="ChangeItemWeight.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the change item weight class.
/// </summary>
using RandomizerBot.Commands.ItemListCommands.Objects;
using SimpleDiscordBot.Commands;

namespace RandomizerBot.Commands.ItemListCommands
{
    /// <summary>
    /// A change item weight.
    /// </summary>
    ///
    /// <seealso cref="AbstractItemListCommand"/>
    public class ChangeItemWeight : AbstractItemListCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ChangeItemWeight() : base("itemlist_change_item_weight", "Changes the weight of an item in an item list")
        {
            AddArgument<string>("item", "The item to update", string.Empty, true);
            AddArgument<int>("new_weight", "The weight for the item in randomization. Must be an integer, higher numbers = more chance for the item to be selected if enabled", 1, true);
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
            var weight = messageInfo.CommandParameters["new_weight"].Value<int>();
            if (weight < 1)
            {
                SendMessage("A valid weight of at least 1 must be provided!", messageInfo);
                return true;
            }

            // Check if the item already exists
            if (!Database.Instance.DB.ItemInItemListExists(itemListParameters.Key, item))
            {
                SendMessage($"An item with the name [{item}] does not exist in the list [{itemListParameters.Key.Name}]!", messageInfo);
                return true;
            }

            // change the item weight in the list
            Database.Instance.DB.UpdateItemInList(itemListParameters.Key, item, item, weight);
            SendMessage($"The weight of the item [{item}] in the {(itemListParameters.Key.IsPersonal ? "personal" : "server-owned")} list with the name [{itemListParameters.Key.Name}] has been updated to [{weight}]!", messageInfo);

            return true;
        }
    }
}
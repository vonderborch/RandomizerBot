/// <file>
/// RandomizerBot\Commands\ItemListCommands\ChangeItemListName.cs
/// </file>
///
/// <copyright file="ChangeItemListName.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the change item list name class.
/// </summary>
using RandomizerBot.Commands.ItemListCommands.Objects;
using SimpleDiscordBot.Commands;

namespace RandomizerBot.Commands.ItemListCommands
{
    /// <summary>
    /// A change item list name.
    /// </summary>
    ///
    /// <seealso cref="AbstractItemListCommand"/>
    public class ChangeItemListName : AbstractItemListCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ChangeItemListName() : base("itemlist_change_list_name", "Changes the name of an item list", needsModPerms: true)
        {
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
            var newName = messageInfo.CommandParameters["new_name"].Value<string>();
            if (string.IsNullOrWhiteSpace(newName))
            {
                SendMessage("A valid non-empty new name must be provided!", messageInfo);
                return true;
            }

            // change the item list name
            Database.Instance.DB.UpdateItemListName(itemListParameters.Key, newName);
            SendMessage($"The name of the {(itemListParameters.Key.IsPersonal ? "personal" : "server-owned")} item list [{itemListParameters.Key.Name}] has been updated to [{newName}]!", messageInfo);

            return true;
        }
    }
}
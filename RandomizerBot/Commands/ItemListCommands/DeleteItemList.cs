/// <file>
/// RandomizerBot\Commands\ItemListCommands\DeleteItemList.cs
/// </file>
///
/// <copyright file="DeleteItemList.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the delete item list class.
/// </summary>
using RandomizerBot.Commands.ItemListCommands.Objects;
using SimpleDiscordBot.Commands;

namespace RandomizerBot.Commands.ItemListCommands
{
    /// <summary>
    /// List of delete items.
    /// </summary>
    ///
    /// <seealso cref="AbstractItemListCommand"/>
    public class DeleteItemList : AbstractItemListCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DeleteItemList() : base("itemlist_delete", "Deletes a list", needsModPerms: true)
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
            Database.Instance.DB.DeleteItemList(itemListParameters.Key);
            SendMessage($"The {(itemListParameters.Key.IsPersonal ? "personal" : "server-owned")} list with the name [{itemListParameters.Key.Name}] has been deleted!", messageInfo);

            return true;
        }
    }
}
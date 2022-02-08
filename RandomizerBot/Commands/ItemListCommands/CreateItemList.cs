/// <file>
/// RandomizerBot\Commands\ItemListCommands\CreateItemList.cs
/// </file>
///
/// <copyright file="CreateItemList.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the create item list class.
/// </summary>
using RandomizerBot.Commands.ItemListCommands.Objects;
using SimpleDiscordBot.Commands;

namespace RandomizerBot.Commands.ItemListCommands
{
    /// <summary>
    /// List of create items.
    /// </summary>
    ///
    /// <seealso cref="AbstractItemListCommand"/>
    public class CreateItemList : AbstractItemListCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public CreateItemList() : base("itemlist_create", "Creates a new list", listMustExist: false)
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
            if ((itemListParameters.Key.IsPersonal && itemListParameters.PersonalExists) || (!itemListParameters.Key.IsPersonal && itemListParameters.ServerExists))
            {
                SendMessage($"A {(itemListParameters.Key.IsPersonal ? "personal" : "server-owned")} list with the name [{itemListParameters.Key.Name}] already exists!", messageInfo);
                return true;
            }

            var result = Database.Instance.DB.CreateItemList(itemListParameters.Key);
            if (result == -1)
            {
                SendMessage($"A {(itemListParameters.Key.IsPersonal ? "personal" : "server-owned")} list with the name [{itemListParameters.Key.Name}] already exists!", messageInfo);
            }
            else
            {
                SendMessage($"A {(itemListParameters.Key.IsPersonal ? "personal" : "server-owned")} list with the name [{itemListParameters.Key.Name}] has been created with ID [{result}]!", messageInfo);
            }

            return true;
        }
    }
}
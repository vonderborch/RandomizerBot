/// <file>
/// RandomizerBot\Commands\ItemListCommands\Objects\AbstractItemListCommand.cs
/// </file>
///
/// <copyright file="AbstractItemListCommand.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the abstract item list command class.
/// </summary>
using RandomizerBot.Commands.ItemListCommands.Objects;
using SimpleDiscordBot.Commands;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands.ItemListCommands
{
    /// <summary>
    /// An abstract item list command.
    /// </summary>
    ///
    /// <seealso cref="AbstractBotCommand"/>
    public abstract class AbstractItemListCommand : AbstractBotCommand
    {
        /// <summary>
        /// True to list must exist.
        /// </summary>
        private bool _listMustExist = false;

        /// <summary>
        /// True to needs modifier permissions.
        /// </summary>
        private bool _needsModPerms = false;

        /// <summary>
        /// Specialized constructor for use only by derived class.
        /// </summary>
        ///
        /// <param name="name">                 The name. </param>
        /// <param name="description">          The description. </param>
        /// <param name="addDefaultArguments">  (Optional) True to add default arguments. </param>
        /// <param name="numArguments">         (Optional) Number of arguments. </param>
        /// <param name="listMustExist">        (Optional) True to list must exist. </param>
        /// <param name="needsModPerms">        (Optional) True to needs modifier permissions. </param>
        protected AbstractItemListCommand(string name, string description, bool addDefaultArguments = true, int numArguments = 4, bool listMustExist = true, bool needsModPerms = false) : base(name, description, false, numArguments, true)
        {
            _listMustExist = listMustExist;
            _needsModPerms = needsModPerms;

            if (addDefaultArguments)
            {
                AddArgument("name", "The name of the list", typeof(string), "", true);
                AddArgument("is_personal_list", "Whether the list is personal or shared with the whole server", typeof(bool), true, false);
            }
        }

        /// <summary>
        /// Executes the 'internal' operation.
        /// </summary>
        ///
        /// <param name="parameters">   Options for controlling the operation. </param>
        /// <param name="messageInfo">  Information describing the message. </param>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        public override bool ExecuteInternal(Dictionary<string, IParameter> parameters, MessageInfo messageInfo)
        {
            // Get Args...
            ListKey? rawKey;
            try
            {
                rawKey = ListKey.GetListKey(parameters, messageInfo.DiscordMessageInfo.SocketMessage, messageInfo.DiscordMessageInfo.Server);
            }
            catch (Exception)
            {
                SendMessage("A name must be specified for the game list!", messageInfo);
                return false;
            }
            var key = (ListKey)rawKey;
            var personalList = key.AsPersonalKeyList();
            var serverList = key.AsServerKeyList();

            var personalListExists = Database.Instance.DB.ItemListExists(personalList);
            var serverListExists = Database.Instance.DB.ItemListExists(serverList);

            if (!personalListExists && !serverListExists && _listMustExist)
            {
                SendMessage($"A list with the name [{key.Name}] does not exist!", messageInfo);
                return true;
            }

            key = parameters["is_personal_list"].WasProvidedByUser
                ? key
                : personalListExists
                    ? personalList
                    : serverList;

            var keys = new Parameters(key, personalList, serverList, personalListExists, serverListExists);

            // if we're executing against a server-owned list, make sure the user has at least mod-level perms or was the original creator if we need to
            if (!key.IsPersonal && _needsModPerms)
            {
                // if we're deleting a server-owned list, make sure the user has at least mod-level perms or was the original creator
                var creator = Database.Instance.DB.GetListCreator(key);

                if (creator == null)
                {
                    SendMessage($"A {(key.IsPersonal ? "personal" : "server-owned")} list with the name [{key.Name}] does not exist!", messageInfo);
                    return true;
                }
                else
                {
                    if (!messageInfo.DiscordMessageInfo.AuthorServerPermissions.ModerateMembers || messageInfo.DiscordMessageInfo.Author.Id != (ulong)creator)
                    {
                        SendMessage("You must be a moderator or original creator of the list to delete server-owned lists!", messageInfo);
                        return true;
                    }
                }
            }

            try
            {
                return ExecuteInternal(keys, messageInfo);
            }
            catch (Exception ex)
            {
                SendMessage("Failed to run command, please try again later or report this issue using !rb_report_issue", messageInfo);
                DiscordBot.Log.Error($"Failed to run command: {ex}");
                return true;
            }
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
        public abstract bool ExecuteInternal(Parameters itemListParameters, MessageInfo messageInfo);
    }
}
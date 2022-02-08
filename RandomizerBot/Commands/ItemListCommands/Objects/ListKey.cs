/// <file>
/// RandomizerBot\Commands\ItemListCommands\Objects\ListKey.cs
/// </file>
///
/// <copyright file="ListKey.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the list key class.
/// </summary>
using Discord.WebSocket;
using Microsoft.Data.Sqlite;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands.ItemListCommands.Objects
{
    /// <summary>
    /// A list key.
    /// </summary>
    public struct ListKey
    {
        /// <summary>
        /// The name.
        /// </summary>
        private string _name;

        /// <summary>
        /// Identifier for the server.
        /// </summary>
        private ulong _serverId;

        /// <summary>
        /// Identifier for the user.
        /// </summary>
        private ulong _userId;

        /// <summary>
        /// True if is personal, false if not.
        /// </summary>
        private bool _isPersonal;

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="parameters">   Options for controlling the operation. </param>
        /// <param name="messageArgs">  The message arguments. </param>
        /// <param name="server">       The server. </param>
        public ListKey(Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            // Get Args...
            _name = parameters["name"].Value<string>();
            if (string.IsNullOrWhiteSpace(_name))
            {
                throw new Exception("There must be a name for a list!");
            }
            _isPersonal = parameters["is_personal_list"].GetValue<bool>();
            _serverId = server.Id;
            _userId = messageArgs.Author.Id;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="messageArgs">  The message arguments. </param>
        /// <param name="server">       The server. </param>
        public ListKey(SocketMessage messageArgs, SocketGuild server)
        {
            // Get Args...
            _name = string.Empty;
            _isPersonal = false;
            _serverId = server.Id;
            _userId = messageArgs.Author.Id;
        }

        /// <summary>
        /// Gets a key.
        /// </summary>
        ///
        /// <param name="isPersonal">   True if this object is personal, false if not. </param>
        ///
        /// <returns>
        /// The key.
        /// </returns>
        public (string, ulong, bool) GetKey(bool isPersonal)
        {
            return (_name, (isPersonal ? _userId : _serverId), isPersonal);
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        ///
        /// <returns>
        /// The keys.
        /// </returns>
        public ((string, ulong, bool), (string, ulong, bool)) GetKeys()
        {
            return (GetKey(true), GetKey(false));
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        ///
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary>
        /// Gets the identifier of the server.
        /// </summary>
        ///
        /// <value>
        /// The identifier of the server.
        /// </value>
        public ulong ServerId => _serverId;

        /// <summary>
        /// Gets the identifier of the user.
        /// </summary>
        ///
        /// <value>
        /// The identifier of the user.
        /// </value>
        public ulong UserId => _userId;

        /// <summary>
        /// Gets or sets a value indicating whether this object is personal.
        /// </summary>
        ///
        /// <value>
        /// True if this object is personal, false if not.
        /// </value>
        public bool IsPersonal
        {
            get => _isPersonal;
            set => _isPersonal = value;
        }

        /// <summary>
        /// Gets a value indicating whether this object is server owned.
        /// </summary>
        ///
        /// <value>
        /// True if this object is server owned, false if not.
        /// </value>
        public bool IsServerOwned => !_isPersonal;

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        ///
        /// <value>
        /// The identifier.
        /// </value>
        public ulong Id => _isPersonal ? _userId : _serverId;

        /// <summary>
        /// Attach parameters.
        /// </summary>
        ///
        /// <param name="command">              The command. </param>
        /// <param name="serverAndUserLists">   True to server and user lists. </param>
        ///
        /// <returns>
        /// A SqliteCommand.
        /// </returns>
        public SqliteCommand AttachParameters(SqliteCommand command, bool serverAndUserLists)
        {
            command.Parameters.Add(new SqliteParameter("Name", Name));
            command.Parameters.Add(new SqliteParameter("ServerOwned", IsServerOwned));

            if (serverAndUserLists)
            {
                command.Parameters.Add(new SqliteParameter("ServerID", UserId.ToString()));
                command.Parameters.Add(new SqliteParameter("UserID", ServerId.ToString()));
            }
            else
            {
                command.Parameters.Add(new SqliteParameter("ID", Id.ToString()));
            }

            return command;
        }

        /// <summary>
        /// Gets list key.
        /// </summary>
        ///
        /// <param name="parameters">   Options for controlling the operation. </param>
        /// <param name="messageArgs">  The message arguments. </param>
        /// <param name="server">       The server. </param>
        ///
        /// <returns>
        /// The list key.
        /// </returns>
        public static ListKey GetListKey(Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            return new ListKey(parameters, messageArgs, server);
        }

        /// <summary>
        /// Converts this object to a personal key list.
        /// </summary>
        ///
        /// <returns>
        /// A ListKey.
        /// </returns>
        public ListKey AsPersonalKeyList()
        {
            var newKey = this;
            newKey.IsPersonal = true;
            return newKey;
        }

        /// <summary>
        /// Converts this object to a server key list.
        /// </summary>
        ///
        /// <returns>
        /// A ListKey.
        /// </returns>
        public ListKey AsServerKeyList()
        {
            var newKey = this;
            newKey.IsPersonal = false;
            return newKey;
        }
    }
}
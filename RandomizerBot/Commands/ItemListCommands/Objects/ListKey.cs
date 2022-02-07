using Discord.WebSocket;
using Microsoft.Data.Sqlite;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands.ItemListCommands.Objects
{
    public struct ListKey
    {
        private string _name;

        private ulong _serverId;

        private ulong _userId;

        private bool _isPersonal;

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

        public ListKey(SocketMessage messageArgs, SocketGuild server)
        {
            // Get Args...
            _name = string.Empty;
            _isPersonal = false;
            _serverId = server.Id;
            _userId = messageArgs.Author.Id;
        }

        public (string, ulong, bool) GetKey(bool isPersonal)
        {
            return (_name, (isPersonal ? _userId : _serverId), isPersonal);
        }

        public ((string, ulong, bool), (string, ulong, bool)) GetKeys()
        {
            return (GetKey(true), GetKey(false));
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public ulong ServerId => _serverId;

        public ulong UserId => _userId;

        public bool IsPersonal
        {
            get => _isPersonal;
            set => _isPersonal = value;
        }

        public bool IsServerOwned => !_isPersonal;

        public ulong Id => _isPersonal ? _userId : _serverId;

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

        public static ListKey GetListKey(Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            return new ListKey(parameters, messageArgs, server);
        }

        public ListKey AsPersonalKeyList()
        {
            var newKey = this;
            newKey.IsPersonal = true;
            return newKey;
        }

        public ListKey AsServerKeyList()
        {
            var newKey = this;
            newKey.IsPersonal = false;
            return newKey;
        }
    }
}

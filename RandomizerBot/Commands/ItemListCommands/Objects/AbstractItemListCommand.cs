using Discord.WebSocket;
using RandomizerBot.Commands.Internal;
using RandomizerBot.Commands.ItemListCommands.Objects;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands.ItemListCommands
{
    public abstract class AbstractItemListCommand : AbstractBotCommand
    {
        private bool _listMustExist = false;

        protected AbstractItemListCommand(string name, string description, bool addDefaultArguments = true, int numArguments = 4, bool listMustExist = true) : base(name, description, false, numArguments, true)
        {
            _listMustExist = listMustExist;

            if (addDefaultArguments)
            {
                AddArgument("name", "The name of the list", typeof(string), "", true);
                AddArgument("is_personal_list", "Whether the list is personal or shared with the whole server", typeof(bool), true, false);
            }
        }

        public override bool ExecuteInternal(Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            // Get Args...
            ListKey? rawKey;
            try
            {
                rawKey = ListKey.GetListKey(parameters, messageArgs, server);
            }
            catch (Exception)
            {
                SendMessage(messageArgs, "A name must be specified for the game list!");
                return false;
            }
            var key = (ListKey)rawKey;
            var personalList = key.AsPersonalKeyList();
            var serverList = key.AsServerKeyList();

            var personalListExists = Database.Instance.DB.ItemListExists(personalList);
            var serverListExists = Database.Instance.DB.ItemListExists(serverList);

            if (!personalListExists && !serverListExists && _listMustExist)
            {
                SendMessage(messageArgs, $"A list with the name [{key.Name}] does not exist!");
                return true;
            }

            key = parameters["is_personal_list"].WasProvidedByUser
                ? key
                : personalListExists
                    ? personalList
                    : serverList;

            return ExecuteInternal(key, personalList, serverList, personalListExists, serverListExists, parameters, messageArgs, server);
        }

        public abstract bool ExecuteInternal(ListKey key, ListKey personalKey, ListKey serverKey, bool personalExists, bool serverExists, Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server);
    }
}

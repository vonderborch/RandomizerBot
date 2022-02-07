using Discord.WebSocket;
using RandomizerBot.Commands.ItemListCommands.Objects;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands.ItemListCommands
{
    public class CreateItemList : AbstractItemListCommand
    {
        public CreateItemList() : base("itemlist_create", "Creates a new list", listMustExist: false)
        {
        }

        public override bool ExecuteInternal(ListKey key, ListKey personalKey, ListKey serverKey, bool personalExists, bool serverExists, Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            var exists = parameters["is_personal_list"].Value<bool>() ? personalExists : serverExists;
            key = parameters["is_personal_list"].Value<bool>() ? personalKey : serverKey;

            if (exists)
            {
                SendMessage(messageArgs, $"A {(key.IsPersonal ? "personal" : "server-owned")} list with the name [{key.Name}] already exists!");
                return true;
            }

            var result = Database.Instance.DB.CreateItemList(key);
            if (result == -1)
            {
                SendMessage(messageArgs, $"A {(key.IsPersonal ? "personal" : "server-owned")} list with the name [{key.Name}] already exists!");
            }
            else
            {
                SendMessage(messageArgs, $"A {(key.IsPersonal ? "personal" : "server-owned")} list with the name [{key.Name}] has been created with ID [{result}]!");
            }

            return true;
        }
    }
}

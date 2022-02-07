using Discord.WebSocket;
using RandomizerBot.Commands.ItemListCommands.Objects;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands.ItemListCommands
{
    public class DeleteItemList : AbstractItemListCommand
    {
        public DeleteItemList() : base("itemlist_delete", "Deletes a list", listMustExist: true)
        {
        }

        public override bool ExecuteInternal(ListKey key, ListKey personalKey, ListKey serverKey, bool personalExists, bool serverExists, Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            var keyToDelete = parameters["is_personal_list"].WasProvidedByUser
                ? key
                : personalExists
                    ? personalKey
                    : serverKey;

            // Try to delete the specified list
            if (!(keyToDelete.IsPersonal ? personalExists : serverExists))
            {
                SendMessage(messageArgs, $"A {(keyToDelete.IsPersonal ? "personal" : "server-owned")} list with the name [{keyToDelete.Name}] does not exist!");
                return true;
            }

            if (!keyToDelete.IsPersonal)
            {
                // if we're deleting a server-owned list, make sure the user has at least mod-level perms or was the original creator
                var creator = Database.Instance.DB.GetListCreator(keyToDelete);

                if (creator == null)
                {
                    SendMessage(messageArgs, $"A {(keyToDelete.IsPersonal ? "personal" : "server-owned")} list with the name [{keyToDelete.Name}] does not exist!");
                    return true;
                }
                else
                {
                    var author = server.GetUser(messageArgs.Author.Id);
                    var perms = author.GuildPermissions;

                    if (!perms.ModerateMembers || author.Id != (ulong)creator)
                    {
                        SendMessage(messageArgs, $"You must be a moderator or original creator of the list to delete server-owned lists!");
                        return true;
                    }
                }
            }

            Database.Instance.DB.DeleteItemList(keyToDelete);
            SendMessage(messageArgs, $"The {(keyToDelete.IsPersonal ? "personal" : "server-owned")} list with the name [{keyToDelete.Name}] has been deleted!");

            return true;
        }
    }
}

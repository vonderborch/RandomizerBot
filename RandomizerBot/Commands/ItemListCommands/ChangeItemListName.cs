using Discord.WebSocket;
using RandomizerBot.Commands.ItemListCommands.Objects;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands.ItemListCommands
{
    public class ChangeItemListName : AbstractItemListCommand
    {
        public ChangeItemListName() : base("itemlist_change_list_name", "Changes the name of an item list")
        {
            AddArgument<string>("new_name", "The new name for the item", string.Empty, true);
        }

        public override bool ExecuteInternal(ListKey key, ListKey personalKey, ListKey serverKey, bool personalExists, bool serverExists, Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            var newName = parameters["new_name"].Value<string>();
            if (string.IsNullOrWhiteSpace(newName))
            {
                SendMessage(messageArgs, "A valid non-empty new name must be provided!");
                return true;
            }

            // Check if the item list already exists
            var newListKey = key;
            newListKey.Name = newName;
            if (Database.Instance.DB.ItemListExists(newListKey))
            {
                SendMessage(messageArgs, $"An item list with the name [{newName}] already exists!");
                return true;
            }

            if (!key.IsPersonal)
            {
                // if we're deleting a server-owned list, make sure the user has at least mod-level perms or was the original creator
                var creator = Database.Instance.DB.GetListCreator(key);

                if (creator == null)
                {
                    SendMessage(messageArgs, $"A {(key.IsPersonal ? "personal" : "server-owned")} list with the name [{key.Name}] does not exist!");
                    return true;
                }
                else
                {
                    var author = server.GetUser(messageArgs.Author.Id);
                    var perms = author.GuildPermissions;

                    if (!perms.ModerateMembers || author.Id != (ulong)creator)
                    {
                        SendMessage(messageArgs, $"You must be a moderator or original creator of the list to change the name of server-owned lists!");
                        return true;
                    }
                }
            }

            // change the item list name
            Database.Instance.DB.UpdateItemListName(key, newName);
            SendMessage(messageArgs, $"The name of the {(key.IsPersonal ? "personal" : "server-owned")} item list [{key.Name}] has been updated to [{newName}]!");

            return true;
        }
    }
}

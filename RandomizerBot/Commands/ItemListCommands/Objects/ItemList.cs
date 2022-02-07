namespace RandomizerBot.Commands.ItemListCommands.Objects
{
    public class ItemList
    {
        public string Name;

        public string UserNameOrServerName;

        public bool IsPersonalList;

        public List<Item> Games;

        public ItemList(string name, string userNameOrServername, bool isPersonalList)
        {
            Name = name;
            UserNameOrServerName = userNameOrServername;
            IsPersonalList = isPersonalList;
            Games = new List<Item>();
        }
    }
}

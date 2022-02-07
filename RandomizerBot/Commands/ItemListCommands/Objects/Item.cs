namespace RandomizerBot.Commands.ItemListCommands.Objects
{
    public class Item
    {
        public string Name;

        public bool IsEnabled;

        public Item(string name, bool isEnabled)
        {
            Name = name;
            IsEnabled = isEnabled;
        }
    }
}

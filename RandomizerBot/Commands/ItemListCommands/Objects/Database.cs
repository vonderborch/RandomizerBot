namespace RandomizerBot.Commands.ItemListCommands.Objects
{
    public class Database
    {
        private ItemListDatabase _database;

        private static readonly Database _instance = new Database();

        static Database() { }

        private Database()
        {
            _database = new ItemListDatabase();
        }

        public static Database Instance => _instance;

        public ItemListDatabase DB => _database;
    }
}
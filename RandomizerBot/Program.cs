namespace RandomizerBot
{
    class Program
    {
        // Program entry point
        static void Main(string[] args)
        {
            var bot = new Bot();
            bot.RunBot().Wait(-1);
        }
    }
}

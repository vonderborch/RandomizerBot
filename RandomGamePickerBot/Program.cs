using System;

namespace RandomGamePickerBot
{
    public class Program
    {
        static void Main(string[] args)
        {
            var bot = new Bot();
            bot.BotAsync().Wait(-1);
        }
    }
}

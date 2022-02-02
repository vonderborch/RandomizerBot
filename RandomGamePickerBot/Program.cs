using System;
using RandomGamePickerBot;

namespace RandomGamePickerBot
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

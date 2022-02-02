using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomizerBot.Commands.GameListCommands.Objects
{
    public class Game
    {
        public string Name;

        public bool IsEnabled;

        public Game(string name, bool isEnabled)
        {
            Name = name;
            IsEnabled = isEnabled;
        }
    }
}

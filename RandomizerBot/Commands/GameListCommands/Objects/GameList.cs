using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomizerBot.Commands.GameListCommands.Objects
{
    public class GameList
    {
        public string Name;

        public string UserNameOrServerName;

        public bool IsPersonalList;

        public List<Game> Games;

        public GameList(string name, string userNameOrServername, bool isPersonalList)
        {
            Name = name;
            UserNameOrServerName = userNameOrServername;
            IsPersonalList = isPersonalList;
            Games = new List<Game>();
        }
    }
}

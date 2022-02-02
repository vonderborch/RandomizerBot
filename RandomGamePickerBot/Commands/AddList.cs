using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Discord.WebSocket;
using RandomGamePickerBot.Commands.Internal;

namespace RandomGamePickerBot.Commands
{
    public class AddList : AbstractCommand
    {
        public AddList() : base("addlist", "Creates a new games list")
        {
        }

        public override void BuildParameterHelper()
        {
            Arguments.Add(new Argument("name", "The name of the list"));
        }

        public override void ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs)
        {
            messageArgs.Channel.SendMessageAsync($"Created a new list");
        }
    }
}

using Discord.WebSocket;
using RandomizerBot.Commands.Internal;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands
{
    public class HelloWorld : AbstractBotCommand
    {
        public HelloWorld() : base("hello_world", "Tells the user that they ran Hello World!", true, 1, false)
        {
            AddArgument("noun", "What to say hello world too", typeof(string), string.Empty);
        }

        public override bool ExecuteInternal(Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            var noun = parameters["noun"].RawValue;
            if (string.IsNullOrWhiteSpace(noun))
            {
                noun = messageArgs.Author.Username;
            }

            SendMessage(messageArgs, $"Hello {noun}!");
            return true;
        }
    }
}

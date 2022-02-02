using Discord.WebSocket;
using RandomGamePickerBot.Commands.Internal;

namespace RandomGamePickerBot.Commands
{
    public class HelloWorld : AbstractCommand
    {
        public HelloWorld() : base("helloworld", "Tells the user that they ran Hello World!")
        {

        }

        public override void BuildParameterHelper()
        {
        }

        public override void ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs)
        {
            messageArgs.Channel.SendMessageAsync($"User '{messageArgs.Author.Username}' successfully ran helloworld!");
        }
    }
}

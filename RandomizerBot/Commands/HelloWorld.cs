using Discord.WebSocket;
using RandomizerBot.Commands.Internal;

namespace RandomizerBot.Commands;

public class HelloWorld : AbstractCommand
{
    public HelloWorld() : base("helloworld", "Tells the user that they ran Hello World!")
    {
    }

    public override void BuildParameterHelper()
    {
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs)
    {
        SendMessage(messageArgs, $"User '{messageArgs.Author.Username}' successfully ran helloworld!");

        return true;
    }
}
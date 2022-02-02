using Discord.WebSocket;
using RandomizerBot.Commands.Internal;

namespace RandomizerBot.Commands;

public class CreateList : AbstractCommand
{
    public CreateList() : base("createList", "Creates a new games list")
    {
    }

    public override void BuildParameterHelper()
    {
        Arguments.Add(new Argument("name", "The name of the list"));
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs)
    {
        SendMessage(messageArgs, "Created a new list");
        return true;
    }
}
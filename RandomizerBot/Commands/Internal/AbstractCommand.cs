using System.Text;
using Discord.WebSocket;

namespace RandomizerBot.Commands.Internal;

public abstract class AbstractCommand
{
    public List<Argument> Arguments;

    public string Command;

    public string HelpMessage;

    protected AbstractCommand(string command, string helpMessage)
    {
        Command = command;
        HelpMessage = helpMessage;
        Arguments = new List<Argument>();
        BuildParameterHelper();
    }

    public int RequiredArgumentCount => Arguments.Count(x => x.IsRequired);

    public abstract void BuildParameterHelper();

    public string GetErrorMessage()
    {
        var str = new StringBuilder();
        str.Append($"!rb_{Command} ");
        for (var i = 0; i < Arguments.Count; i++) str.Append($"{Arguments[i].Parameter} ");
        str.AppendLine(" ");
        for (var i = 0; i < Arguments.Count; i++) str.Append($"{Arguments[i].HelpMessage} ");

        return str.ToString();
    }

    public bool Execute(Dictionary<string, string> args, SocketMessage messageArgs)
    {
        Console.WriteLine($"Executing command [{Command}] for user [{messageArgs.Author.Username}]");
        return ExecuteInternal(args, messageArgs);
    }

    public abstract bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs);

    public void SendMessage(SocketMessage messageArgs, string message)
    {
        messageArgs.Channel.SendMessageAsync(message);
    }
}
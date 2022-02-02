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
        for (var i = 0; i < Arguments.Count; i++) str.AppendLine($"{Arguments[i].HelpMessage} ");

        return str.ToString();
    }

    public bool Execute(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        CommandParser.Log($"Executing command [{Command}] for user [{messageArgs.Author.Username}] on server [{server.Name}] with parameters: {(string.Join(", ", args))}");
        return ExecuteInternal(args, messageArgs, server);
    }

    public abstract bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server);

    public void SendMessage(SocketMessage messageArgs, string message, bool tts = false)
    {
        messageArgs.Channel.SendMessageAsync(message, tts);
    }
}
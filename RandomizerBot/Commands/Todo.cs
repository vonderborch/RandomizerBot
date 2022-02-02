using System.Text;
using Discord.WebSocket;
using RandomizerBot.Commands.Internal;

namespace RandomizerBot.Commands;

public class Todo : AbstractCommand
{
    public Todo() : base("todo", "Displays a to-do list for the bot to the user")
    {
    }

    public override void BuildParameterHelper()
    {
    }

    public override bool ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs, SocketGuild server)
    {
        if (!File.Exists("Todo.txt"))
        {
            SendMessage(messageArgs, "Could not find the to-do list! Please let the developer know of this issue!");
        }
        else
        {
            var str = new StringBuilder();
            str.AppendLine("```");
            str.Append(File.ReadAllText("Todo.txt"));
            str.AppendLine("```");
            str.AppendLine("A full to-do list can be found on the github at ");

            SendMessage(messageArgs, str.ToString());
        }




        return true;
    }
}
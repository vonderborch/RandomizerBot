using Discord.WebSocket;
using RandomizerBot.Commands.Internal;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands
{
    public class DevToDo : AbstractBotCommand
    {
        public DevToDo() : base("dev_todo", "Prints out the bot's development todo list", numArguments: 2)
        {
            AddArgument("git_link", "Whether to send the github todo list link", typeof(bool), false);
            AddArgument("local_changelog", "Whether to show the local todo list", typeof(bool), true);
        }

        public override bool ExecuteInternal(Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            if (parameters["git_link"].Value<bool>())
            {
                SendMessage(messageArgs, "A full to-do list can be found on the github at https://github.com/vonderborch/RandomizerBot/issues");
            }

            if (parameters["local_changelog"].Value<bool>())
            {
                if (!File.Exists("Todo.txt"))
                {
                    SendMessage(messageArgs, "Could not find the todo list! Please let the developer know of this issue!");
                }
                else
                {
                    var text = File.ReadAllText("Todo.txt");
                    SendMessage(messageArgs, parameters["print_as_text_file"], text);
                }
            }

            return true;
        }
    }
}

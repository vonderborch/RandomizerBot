using System.Text;
using Discord.WebSocket;

namespace RandomGamePickerBot.Commands.Internal
{
    public abstract class AbstractCommand
    {
        protected AbstractCommand(string command, string helpMessage)
        {
            Command = command;
            HelpMessage = helpMessage;
            Arguments = new List<Argument>();
            BuildParameterHelper();
        }

        public string Command;

        public string HelpMessage;

        public List<Argument> Arguments;

        public int RequiredArgumentCount => Arguments.Count(x => x.IsRequired);

        public abstract void BuildParameterHelper();

        public string GetErrorMessage()
        {
            var str = new StringBuilder();
            str.Append($"!rb_{Command} ");
            for (var i = 0; i < Arguments.Count; i++)
            {
                str.Append($"{Arguments[i].Parameter} ");
            }
            str.AppendLine(" ");
            for (var i = 0; i < Arguments.Count; i++)
            {
                str.Append($"{Arguments[i].HelpMessage} ");
            }

            return str.ToString();
        }

        public void Execute(Dictionary<string, string> args, SocketMessage messageArgs)
        {
            Console.WriteLine($"Executing command [{Command}] for user [{messageArgs.Author.Username}]");
            ExecuteInternal(args, messageArgs);
        }

        public abstract void ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs);
    }
}

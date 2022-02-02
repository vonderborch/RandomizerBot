using System.Text;
using Discord.WebSocket;
using RandomGamePickerBot.Commands.Internal;

namespace RandomGamePickerBot.Commands
{
    public class Help : AbstractCommand
    {
        public Help() : base("help", "Displays a list of all available commands to the user")
        {

        }

        public override void BuildParameterHelper()
        {
        }

        public override void ExecuteInternal(Dictionary<string, string> args, SocketMessage messageArgs)
        {
            var maxCommandNameLength = int.MinValue;
            foreach (var cmd in CommandRegister.Instance.Commands)
            {
                if (cmd.Key.Length > maxCommandNameLength)
                {
                    maxCommandNameLength = $"!rb_{cmd.Key}".Length;
                }
            }
            
            var str = new StringBuilder();
            str.AppendLine("Available Commands:```");
            foreach (var cmd in CommandRegister.Instance.Commands)
            {
                var actualName = $"!rb_{cmd.Key}";
                str.AppendLine($"{actualName.PadRight(maxCommandNameLength)} - {cmd.Value.HelpMessage}");
            }
            str.AppendLine("```");

            messageArgs.Channel.SendMessageAsync(str.ToString());
        }
    }
}

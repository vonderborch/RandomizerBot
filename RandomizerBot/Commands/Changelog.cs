using Discord.WebSocket;
using RandomizerBot.Commands.Internal;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands
{
    public class Changelog : AbstractBotCommand
    {
        public Changelog() : base("changelog", "Prints out the bot's changelog", numArguments: 2)
        {
            AddArgument("git_link", "Whether to send the github changes link", typeof(bool), false);
            AddArgument("local_changelog", "Whether to show the local changelog", typeof(bool), true);
        }

        public override bool ExecuteInternal(Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server)
        {
            if (parameters["git_link"].Value<bool>())
            {
                SendMessage(messageArgs, "A full changelog can be found on the github at https://github.com/vonderborch/Velentr.Miscellaneous/commits/");
            }

            if (parameters["local_changelog"].Value<bool>())
            {
                if (!File.Exists("Changelog.txt"))
                {
                    SendMessage(messageArgs, "Could not find the changelog! Please let the developer know of this issue!");
                }
                else
                {
                    var text = File.ReadAllText("Changelog.txt");
                    SendMessage(messageArgs, parameters["print_as_text_file"], text);
                }
            }

            return true;
        }
    }
}

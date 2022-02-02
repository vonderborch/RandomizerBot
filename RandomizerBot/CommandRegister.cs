using RandomizerBot.Commands;
using RandomizerBot.Commands.Internal;

namespace RandomizerBot
{
    public sealed class CommandRegister
    {
        private static readonly CommandRegister _instance = new CommandRegister();

        private Dictionary<string, AbstractCommand> _commands = new Dictionary<string, AbstractCommand>();

        static CommandRegister()
        {
        }

        private CommandRegister()
        {
            RegisterCommand(new HelloWorld());
            RegisterCommand(new Help());
            RegisterCommand(new CreateList());
            RegisterCommand(new FlipCoin());
        }

        public static CommandRegister Instance => _instance;

        public Dictionary<string, AbstractCommand> Commands => _commands;

        public void RegisterCommand(AbstractCommand command)
        {
            _commands.Add(command.Command, command);
        }
    }
}

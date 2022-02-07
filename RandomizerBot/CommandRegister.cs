using RandomizerBot.Commands;
using RandomizerBot.Commands.GameListCommands;
using RandomizerBot.Commands.Internal;

namespace RandomizerBot
{
    public sealed class CommandRegister
    {
        private static readonly CommandRegister _instance = new CommandRegister();

        private Dictionary<string, AbstractBotCommand> _commands = new Dictionary<string, AbstractBotCommand>();

        static CommandRegister()
        {
        }

        private CommandRegister()
        {
            RegisterCommand(new Help());
            RegisterCommand(new Changelog());
            RegisterCommand(new Commands.Version());
            RegisterCommand(new Todo());
            RegisterCommand(new FlipCoin());
            RegisterCommand(new D4());
            RegisterCommand(new D6());
            RegisterCommand(new D8());
            RegisterCommand(new D10());
            RegisterCommand(new D12());
            RegisterCommand(new D20());
            RegisterCommand(new RandomList());
            RegisterCommand(new ViewAllGameListGames());
            RegisterCommand(new CreateGameList());
            RegisterCommand(new DeleteGameList());
            RegisterCommand(new ViewGameListGames());
            RegisterCommand(new AddGameToGameList());
            RegisterCommand(new RemoveGameFromGameList());
            RegisterCommand(new ToggleGameEnablement());
            RegisterCommand(new DisableAllGamesInGameList());
            RegisterCommand(new EnableAllGamesInGameList());
            RegisterCommand(new RandomizeGameListGames());
        }

        public static CommandRegister Instance => _instance;

        public Dictionary<string, AbstractBotCommand> Commands => _commands;

        public void RegisterCommand(AbstractBotCommand command)
        {
            _commands.Add(command.Command, command);
        }
    }
}

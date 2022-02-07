using Discord;
using Discord.WebSocket;
using RandomizerBot.Commands;
using RandomizerBot.Commands.Internal;
using RandomizerBot.Commands.ItemListCommands;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot
{
    public class BotCommandParser
    {
        private CommandParser _parser;

        private static int[] _diceToImplement = new int[] { 4, 6, 8, 10, 12, 20 };
        private static int[] _hiddenDiceToImplement = new int[] { 1, 100 };

        public BotCommandParser()
        {
            _parser = new CommandParser("!rb_", false, false, false);

            // Register all commands
            _parser.RegisterCommand(new Changelog());
            _parser.RegisterCommand(new DevToDo());
            _parser.RegisterCommand(new Commands.Version());
            _parser.RegisterCommand(new ReportIssue());

            _parser.RegisterCommand(new FlipCoin());
            _parser.RegisterCommand(new GenericDice());
            for (var i = 0; i < _diceToImplement.Length; i++)
            {
                _parser.RegisterCommand(new Dice(_diceToImplement[i]));
            }
            _parser.RegisterCommand(new RandomList());

            // Item List Commands
            _parser.RegisterCommand(new ViewItemLists());
            _parser.RegisterCommand(new CreateItemList());
            _parser.RegisterCommand(new DeleteItemList());
            _parser.RegisterCommand(typeof(ChangeItemListName));
            _parser.RegisterCommand(new ViewItemListItems());
            _parser.RegisterCommand(new AddItemToItemList());
            _parser.RegisterCommand(new DeleteItemInItemList());
            _parser.RegisterCommand(new ChangeItemName());
            _parser.RegisterCommand(new ChangeItemWeight());
            _parser.RegisterCommand(new EnableItemInList());
            _parser.RegisterCommand(new DisableItemInList());
            _parser.RegisterCommand(new ToggleItemInList());
            _parser.RegisterCommand(new EnableAllItems());
            _parser.RegisterCommand(new DisableAllItems());
            _parser.RegisterCommand(new ToggleAllItems());
            _parser.RegisterCommand(typeof(RandomizeItemList));

            // Easter Eggs
            _parser.RegisterCommand(new HelloWorld());
            for (var i = 0; i < _hiddenDiceToImplement.Length; i++)
            {
                _parser.RegisterCommand(new Dice(_hiddenDiceToImplement[i], isHidden: true));
            }

            // Help Command
            _parser.RegisterHelpCommand(new Help());
        }

        public void ParseCommand(string message, SocketMessage messageArgs)
        {
            // ignore my own messages!
            if (messageArgs.Author.Id == Constants.BotUserID)
            {
                return;
            }

            // help message for default cases...
            if (message.ToLowerInvariant() == "!rb_" || message.ToLowerInvariant() == "!rb")
            {
                message = "!rb_help";
            }

            Log($"Parsing message [{message}]");
            var result = _parser.ParseCommand(message);

            if (result != null)
            {
                if (result.Command != null)
                {
                    var args = new Dictionary<string, object>()
                    {
                        { "SocketMessage", messageArgs },
                        { "Server", ((SocketGuildChannel)messageArgs.Channel).Guild }
                    };

                    var good = result.Command.ExecuteCommand(result.Parameters, args);

                    if (good)
                    {
                        Log("Succeeded in executing command!");
                    }
                    else
                    {
                        messageArgs.Channel.SendMessageAsync("Failed to execute command!");

                        var helpCommand = _parser.ParseCommand($"!rb_help {result.Command.CommandName}");

                        helpCommand.Command.ExecuteCommand(helpCommand.Parameters, args);
                        Log("Failed to execute command!");
                    }
                }
            }
        }

        public static void Log(string message, LogSeverity severity = LogSeverity.Info, Exception? exception = null)
        {
            Console.WriteLine(new LogMessage(severity, "CommandParser", message, exception).ToString());
        }
    }
}

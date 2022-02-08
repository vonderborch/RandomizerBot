/// <file>
/// RandomizerBot\Program.cs
/// </file>
///
/// <copyright file="Program.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the program class.
/// </summary>
using RandomizerBot.Commands;
using RandomizerBot.Commands.ItemListCommands;
using SimpleDiscordBot;

namespace RandomizerBot
{
    /// <summary>
    /// A program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The dice to implement.
        /// </summary>
        private static int[] _diceToImplement = new int[] { 4, 6, 8, 10, 12, 20 };

        /// <summary>
        /// The hidden dice to implement.
        /// </summary>
        private static int[] _hiddenDiceToImplement = new int[] { 1, 3, 100 };

        /// <summary>
        /// Program entry point.
        /// </summary>
        ///
        /// <param name="args"> An array of command-line argument strings. </param>
        private static void Main(string[] args)
        {
            var token = File.ReadAllText("Token.txt");
            var config = new BotConfiguration("RandomizerBot", "A simple bot that can be used to randomize things.", "https://github.com/vonderborch/RandomizerBot/issues/new", Constants.BotUserID, token, "!rb_", ignoreOwnMessages: true);

            config.AddBotHelpCommand = new BuiltInCommandOptions(true, false, false, new List<string>() { "!rb", "!rb_", "!rb!" });
            config.AddBotInfoCommand = new BuiltInCommandOptions(true, false, false);
            config.AddBotVersionCommand = new BuiltInCommandOptions(true, false, false);
            config.AddBotReportIssueCommand = new BuiltInCommandOptions(true, false, false);

            var bot = new Bot(config);
            RegisterCommands(bot);

            bot.RunBot().Wait(-1);
        }

        /// <summary>
        /// Registers the commands described by bot.
        /// </summary>
        ///
        /// <param name="bot">  The bottom. </param>
        private static void RegisterCommands(Bot bot)
        {
            // extra core commands
            bot.RegisterCommand(new Changelog());
            bot.RegisterCommand(new DevToDo());

            // simple random commands
            bot.RegisterCommand(new FlipCoin());
            bot.RegisterCommand(new GenericDice());
            for (var i = 0; i < _diceToImplement.Length; i++)
            {
                bot.RegisterCommand(new Dice(_diceToImplement[i]));
            }
            bot.RegisterCommand(new RandomList());

            // Item List Commands
            bot.RegisterCommand(new ViewItemLists());
            bot.RegisterCommand(new CreateItemList());
            bot.RegisterCommand(new DeleteItemList());
            bot.RegisterCommand(new ChangeItemListName());
            bot.RegisterCommand(new ViewItemListItems());
            bot.RegisterCommand(new AddItemToItemList());
            bot.RegisterCommand(new DeleteItemInItemList());
            bot.RegisterCommand(new ChangeItemName());
            bot.RegisterCommand(new ChangeItemWeight());
            bot.RegisterCommand(new EnableItemInList());
            bot.RegisterCommand(new DisableItemInList());
            bot.RegisterCommand(new ToggleItemInList());
            bot.RegisterCommand(new EnableAllItems());
            bot.RegisterCommand(new DisableAllItems());
            bot.RegisterCommand(new ToggleAllItems());
            bot.RegisterCommand(new RandomizeItemList());

            // Easter Eggs
            bot.RegisterCommand(new HelloWorld());
            for (var i = 0; i < _hiddenDiceToImplement.Length; i++)
            {
                bot.RegisterCommand(new Dice(_hiddenDiceToImplement[i], isHidden: true));
            }
        }
    }
}
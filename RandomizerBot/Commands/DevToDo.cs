/// <file>
/// RandomizerBot\Commands\DevToDo.cs
/// </file>
///
/// <copyright file="DevToDo.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the development to do class.
/// </summary>
using SimpleDiscordBot.Commands;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands
{
    /// <summary>
    /// A development to do.
    /// </summary>
    ///
    /// <seealso cref="AbstractBotCommand"/>
    public class DevToDo : AbstractBotCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DevToDo() : base("dev_todo", "Prints out the bot's development todo list", numArguments: 2)
        {
            AddArgument("git_link", "Whether to send the github todo list link", typeof(bool), false);
            AddArgument("local_changelog", "Whether to show the local todo list", typeof(bool), true);
        }

        /// <summary>
        /// Executes the 'internal' operation.
        /// </summary>
        ///
        /// <param name="parameters">   Options for controlling the operation. </param>
        /// <param name="messageInfo">  Information describing the message. </param>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        public override bool ExecuteInternal(Dictionary<string, IParameter> parameters, MessageInfo messageInfo)
        {
            if (parameters["git_link"].Value<bool>())
            {
                SendMessage("A full to-do list can be found on the github at https://github.com/vonderborch/RandomizerBot/issues", messageInfo);
            }

            if (parameters["local_changelog"].Value<bool>())
            {
                if (!File.Exists("Todo.txt"))
                {
                    SendMessage("Could not find the todo list! Please let the developer know of this issue!", messageInfo);
                }
                else
                {
                    var text = File.ReadAllText("Todo.txt");
                    SendMessage(text, messageInfo, true);
                }
            }

            return true;
        }
    }
}
/// <file>
/// RandomizerBot\Commands\Changelog.cs
/// </file>
///
/// <copyright file="Changelog.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the changelog class.
/// </summary>
using SimpleDiscordBot.Commands;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands
{
    /// <summary>
    /// A changelog.
    /// </summary>
    ///
    /// <seealso cref="AbstractBotCommand"/>
    public class Changelog : AbstractBotCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Changelog() : base("changelog", "Prints out the bot's changelog", numArguments: 2)
        {
            AddArgument("git_link", "Whether to send the github changes link", typeof(bool), false);
            AddArgument("local_changelog", "Whether to show the local changelog", typeof(bool), true);
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
                SendMessage("A full changelog can be found on the github at https://github.com/vonderborch/Velentr.Miscellaneous/commits/", messageInfo);
            }

            if (parameters["local_changelog"].Value<bool>())
            {
                if (!File.Exists("Changelog.txt"))
                {
                    SendMessage("Could not find the changelog! Please let the developer know of this issue!", messageInfo);
                }
                else
                {
                    var text = File.ReadAllText("Changelog.txt");
                    SendMessage(text, messageInfo, true);
                }
            }

            return true;
        }
    }
}
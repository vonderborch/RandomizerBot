/// <file>
/// RandomizerBot\Commands\HelloWorld.cs
/// </file>
///
/// <copyright file="HelloWorld.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the hello world class.
/// </summary>
using SimpleDiscordBot.Commands;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands
{
    /// <summary>
    /// A hello world.
    /// </summary>
    ///
    /// <seealso cref="AbstractBotCommand"/>
    public class HelloWorld : AbstractBotCommand
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public HelloWorld() : base("hello_world", "Says hello to something", true, 1, false)
        {
            AddArgument("noun", "What to say hello too", typeof(string), string.Empty);
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
            var noun = parameters["noun"].RawValue;
            if (string.IsNullOrWhiteSpace(noun))
            {
                noun = messageInfo.DiscordMessageInfo.Author.Username;
            }

            SendMessage($"Hello {noun}!", messageInfo);
            return true;
        }
    }
}
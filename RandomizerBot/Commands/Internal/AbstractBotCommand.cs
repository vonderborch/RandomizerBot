using System.Text;
using Discord.WebSocket;
using RandomizerBot.Helpers;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands.Internal
{
    public abstract class AbstractBotCommand : AbstractCommand
    {
        protected AbstractBotCommand(string name, string description, bool isHidden = false, int numArguments = 2, bool autoRegisterCommand = true) : base(name, description, isHidden, numArguments, autoRegisterCommand)
        {
            AddArgument("print_as_text_file", "Whether to print the whole message in a single message by attaching the results as a text file or not. Defaults to true if the commands response exceeds the max message length.", typeof(bool), true, false, true);
        }

        public override bool ExecuteCommand(Dictionary<string, IParameter> parameters, Dictionary<string, object> args)
        {
            var messageArgs = (SocketMessage)args["SocketMessage"];
            var server = (SocketGuild)args["Server"];
            var parameterMessages = new StringBuilder();
            foreach (var parameter in parameters)
            {
                parameterMessages.Append($"({parameter.Key},{parameter.Value.RawValue}), ");
            }
            parameterMessages.Length -= 2;

            BotCommandParser.Log($"Executing command [{Name}] for user [{messageArgs.Author.Username}] on server [{server.Name}] with parameters: {parameterMessages}");
            return ExecuteInternal(parameters, messageArgs, server);
        }

        public abstract bool ExecuteInternal(Dictionary<string, IParameter> parameters, SocketMessage messageArgs, SocketGuild server);

        public void SendMessage(SocketMessage messageArgs, string message, bool tts = false)
        {
            messageArgs.Channel.SendMessageAsync(message, tts);
        }

        public void SendMessage(SocketMessage messageArgs, List<string> messages, bool tts = false)
        {
            for (var i = 0; i < messages.Count; i++)
            {
                messageArgs.Channel.SendMessageAsync(messages[i], tts);
                Thread.Sleep(500);
            }
        }

        public void SendMessage(SocketMessage messageArgs, IParameter printAsTextFile, string message, bool tts = false)
        {
            var printAsText = printAsTextFile.GetValue<bool>();

            if ((printAsText && printAsTextFile.WasProvidedByUser) || (message.Length > Constants.MaxCodeFormattedMessageLength && !printAsTextFile.WasProvidedByUser))
            {
                using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(message)))
                {
                    messageArgs.Channel.SendFileAsync(stream, $"{Name}.txt", "");
                }
            }
            else
            {
                var outputText = MessageHelper.GetCodeFormattedMessages(message);
                SendMessage(messageArgs, outputText);
            }
        }
    }
}

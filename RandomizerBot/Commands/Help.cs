using System.Text;
using Discord.WebSocket;
using RandomizerBot.Helpers;
using Velentr.Miscellaneous.CommandParsing;

namespace RandomizerBot.Commands
{
    public class Help : DefaultHelpCommand
    {
        public Help() : base()
        {
            AddArgument("print_as_text_file", "Whether to print the whole message in a single message by attaching the results as a text file or not. Defaults to true if the commands response exceeds the max message length.", typeof(bool), true);
        }

        public override StringBuilder ExecutePreCommand(StringBuilder str, Dictionary<string, IParameter> parameters, Dictionary<string, object> args)
        {
            var messageArgs = (SocketMessage)args["SocketMessage"];
            var server = (SocketGuild)args["Server"];
            var parameterMessages = new StringBuilder();

            BotCommandParser.Log($"Executing command [{Name}] for user [{messageArgs.Author.Username}] on server [{server.Name}] with parameters: {parameterMessages}");

            return str;
        }

        public override void ExecutePostCommand(StringBuilder str, Dictionary<string, IParameter> parameters, Dictionary<string, object> args)
        {
            var messageArgs = (SocketMessage)args["SocketMessage"];

            SendMessage(messageArgs, parameters["print_as_text_file"], str.ToString());

            var commandToExecuteOn = (parameters["command"].RawValue).ToLowerInvariant();
            var failureCase = GetParameterValueIfExistsAsString("failure_case", parameters);
            if (string.IsNullOrWhiteSpace(commandToExecuteOn) && string.IsNullOrWhiteSpace(failureCase))
            {
                SendMessage(messageArgs, "To get more help about a specific method, use the following command: `!rb_help <methodname>` (Example: `!rb_help !rb_changelog`)");
            }
        }

        public void SendMessage(SocketMessage messageArgs, IParameter printAsTextFile, string message, bool tts = false)
        {
            var printAsText = printAsTextFile.Value<bool>();


            if ((printAsText && printAsTextFile.WasProvidedByUser) || (message.Length > Constants.MaxCodeFormattedMessageLength && !printAsTextFile.WasProvidedByUser))
            {
                using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(message)))
                {
                    messageArgs.Channel.SendFileAsync(stream, "help.txt", "Available Commands:");
                }
            }
            else
            {
                var outputText = MessageHelper.GetCodeFormattedMessages(message);
                SendMessage(messageArgs, outputText);
            }
        }

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
    }
}

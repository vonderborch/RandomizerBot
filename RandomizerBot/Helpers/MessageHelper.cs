using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Velentr.Miscellaneous.StringHelpers;

namespace RandomizerBot.Helpers
{
    public static class MessageHelper
    {
        public static List<string> SplitMessageByMaxSize(string message, int maxSize = 2000)
        {
            // if we're under the max message size, return the message as-is
            if (message.Length < maxSize)
            {
                return new List<string> { message };
            }

            // otherwise, lets split it. Let's first try to split by new lines...
            var lines = StringSplitters.SplitStringByNewLines(message);
            var output = new List<string>();
            var currentMessage = new StringBuilder();
            for (var i = 0; i < lines.Count; i++)
            {
                // try to add it to the current message...
                if (currentMessage.Length + lines[i].Length < maxSize)
                {
                    currentMessage.AppendLine(lines[i]);
                }
                else
                {
                    // if it is too big for the current message (and the current message has something on it), clear the current message and try to add it to a new message
                    if (currentMessage.Length > 0)
                    {
                        output.Add(currentMessage.ToString());
                        currentMessage.Clear();
                    }

                    if (lines[i].Length < maxSize)
                    {
                        currentMessage.AppendLine(lines[i]);
                    }
                    else
                    {
                        // if it is too big for any single message, split it into multiple...
                        var chunks = StringSplitters.SplitStringByChunkSize(lines[i], maxSize);
                        for (var j = 0; j < chunks.Count; j++)
                        {
                            output.Add(chunks[j]);
                        }
                    }
                }
            }

            if (currentMessage.Length > 0)
            {
                output.Add(currentMessage.ToString());
            }

            return output;
        }

        public static List<string> GetCodeFormattedMessages(List<string> messages)
        {
            var str = new StringBuilder();
            var output = new List<string>();
            for (var i = 0; i < messages.Count; i++)
            {
                str.AppendLine("```");
                str.AppendLine(messages[i]);
                str.AppendLine("```");

                output.Add(str.ToString());
                str.Clear();
            }

            return output;
        }

        public static List<string> GetCodeFormattedMessages(string message)
        {
            var messages = SplitMessageByMaxSize(message, Constants.MaxCodeFormattedMessageLength);
            var str = new StringBuilder();
            var output = new List<string>();
            for (var i = 0; i < messages.Count; i++)
            {
                str.AppendLine("```");
                str.AppendLine(messages[i]);
                str.AppendLine("```");

                output.Add(str.ToString());
                str.Clear();
            }

            return output;
        }
    }
}

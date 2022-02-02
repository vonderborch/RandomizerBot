using Discord.WebSocket;

namespace RandomizerBot.Helpers
{
    public static class NameHelpers
    {
        public static string GetListFileName(string listName, bool isPersonalList, SocketMessage messageArgs, SocketGuild server)
        {
            var dir = isPersonalList
                ? messageArgs.Author.Username
                : server.Name;

            var baseDir = Path.GetDirectoryName(listName);
            if (!string.IsNullOrEmpty(baseDir))
            {
                dir = baseDir;
                listName = Path.GetFileNameWithoutExtension(listName);
            }

            dir = dir.Replace(" ", "_");
            listName = listName.Replace(" ", "_");

            return Path.Combine(dir, $"{listName}.rbl");
        }
    }
}

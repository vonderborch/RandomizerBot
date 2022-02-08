/// <file>
/// RandomizerBot\Helpers\NameHelpers.cs
/// </file>
///
/// <copyright file="NameHelpers.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the name helpers class.
/// </summary>
using Discord.WebSocket;

namespace RandomizerBot.Helpers
{
    /// <summary>
    /// A name helpers.
    /// </summary>
    public static class NameHelpers
    {
        /// <summary>
        /// Gets list file name.
        /// </summary>
        ///
        /// <param name="listName">         Name of the list. </param>
        /// <param name="isPersonalList">   True if is personal list, false if not. </param>
        /// <param name="messageArgs">      The message arguments. </param>
        /// <param name="server">           The server. </param>
        ///
        /// <returns>
        /// The list file name.
        /// </returns>
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
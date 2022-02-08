/// <file>
/// RandomizerBot\Commands\ItemListCommands\Objects\ItemList.cs
/// </file>
///
/// <copyright file="ItemList.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the item list class.
/// </summary>
namespace RandomizerBot.Commands.ItemListCommands.Objects
{
    /// <summary>
    /// List of items.
    /// </summary>
    public class ItemList
    {
        /// <summary>
        /// The name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Name of the user name or server.
        /// </summary>
        public string UserNameOrServerName;

        /// <summary>
        /// True if is personal list, false if not.
        /// </summary>
        public bool IsPersonalList;

        /// <summary>
        /// The games.
        /// </summary>
        public List<Item> Games;

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="name">                 The name. </param>
        /// <param name="userNameOrServername"> The user name or servername. </param>
        /// <param name="isPersonalList">       True if is personal list, false if not. </param>
        public ItemList(string name, string userNameOrServername, bool isPersonalList)
        {
            Name = name;
            UserNameOrServerName = userNameOrServername;
            IsPersonalList = isPersonalList;
            Games = new List<Item>();
        }
    }
}
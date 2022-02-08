/// <file>
/// RandomizerBot\Commands\ItemListCommands\Objects\Item.cs
/// </file>
///
/// <copyright file="Item.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the item class.
/// </summary>
namespace RandomizerBot.Commands.ItemListCommands.Objects
{
    /// <summary>
    /// An item.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// The name.
        /// </summary>
        public string Name;

        /// <summary>
        /// True if is enabled, false if not.
        /// </summary>
        public bool IsEnabled;

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="name">         The name. </param>
        /// <param name="isEnabled">    True if is enabled, false if not. </param>
        public Item(string name, bool isEnabled)
        {
            Name = name;
            IsEnabled = isEnabled;
        }
    }
}
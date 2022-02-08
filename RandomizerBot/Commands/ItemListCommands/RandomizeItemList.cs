/// <file>
/// RandomizerBot\Commands\ItemListCommands\RandomizeItemList.cs
/// </file>
///
/// <copyright file="RandomizeItemList.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the randomize item list class.
/// </summary>
using RandomizerBot.Commands.ItemListCommands.Objects;
using SimpleDiscordBot.Commands;
using System.Text;
using Velentr.Miscellaneous.StringHelpers;

namespace RandomizerBot.Commands.ItemListCommands
{
    /// <summary>
    /// List of randomize items.
    /// </summary>
    ///
    /// <seealso cref="AbstractItemListCommand"/>
    public class RandomizeItemList : AbstractItemListCommand
    {
        /// <summary>
        /// (Immutable) the randomizer.
        /// </summary>
        private readonly Random _randomizer;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RandomizeItemList() : base("itemlist_randomize", "Randomizes an item list")
        {
            _randomizer = new Random();

            AddArgument<bool>("show_original_list", "Whether to show the original list order", false);
            AddArgument<bool>("show_first_item_only", "Whether to show only the first item in the randomized list order", true);
        }

        /// <summary>
        /// Executes the 'internal' operation.
        /// </summary>
        ///
        /// <param name="itemListParameters">   Options for controlling the item list. </param>
        /// <param name="messageInfo">          Information describing the message. </param>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        ///
        /// <seealso cref="RandomizerBot.Commands.ItemListCommands.AbstractItemListCommand.ExecuteInternal(Parameters,MessageInfo)"/>
        public override bool ExecuteInternal(Parameters itemListParameters, MessageInfo messageInfo)
        {
            var items = Database.Instance.DB.GetItemListItems(itemListParameters.Key);
            if (items.Count == 0)
            {
                // no items :(
                SendMessage("There are no items in the specified list!", messageInfo);
                return true;
            }

            var enabledItems = items.Where(x => x.IsEnabledForRandomization).ToList();
            if (enabledItems.Count == 0)
            {
                SendMessage("There are no items enabled for randomization in the specified list!", messageInfo);
                return true;
            }

            var randomOrder = enabledItems.OrderBy(x => _randomizer.NextDouble() * x.Weight).ToList();

            var showOriginal = messageInfo.CommandParameters["show_original_list"].Value<bool>();
            var showAll = !messageInfo.CommandParameters["show_first_item_only"].Value<bool>();

            if (!showAll && !showOriginal)
            {
                SendMessage($"Top Randomized Item: {randomOrder[0].Name}", messageInfo);
            }
            else
            {
                var str = new StringBuilder();

                var columns = new List<string>()
                {
                    "Name"
                };
                var randomListString = randomOrder.Select(x => x.GetText(columns)).ToList();
                str.AppendLine("Randomized List:");
                str.AppendLine(TableOutputHelper.ConvertToTable(columns, randomListString, includeHeaders: false));

                if (showOriginal)
                {
                    var originalList = enabledItems.Select(x => x.GetText(columns)).ToList();
                    str.AppendLine(" ");
                    str.AppendLine("Original List:");
                    str.AppendLine(TableOutputHelper.ConvertToTable(columns, originalList, includeHeaders: false));
                }

                SendMessage(str.ToString(), messageInfo, true);
            }

            return true;
        }
    }
}
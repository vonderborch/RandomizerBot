/// <file>
/// RandomizerBot\Commands\ItemListCommands\Objects\Database.cs
/// </file>
///
/// <copyright file="Database.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the database class.
/// </summary>
namespace RandomizerBot.Commands.ItemListCommands.Objects
{
    /// <summary>
    /// A database.
    /// </summary>
    public class Database
    {
        /// <summary>
        /// The database.
        /// </summary>
        private ItemListDatabase _database;

        /// <summary>
        /// (Immutable) the instance.
        /// </summary>
        private static readonly Database _instance = new Database();

        /// <summary>
        /// Static constructor.
        /// </summary>
        static Database()
        {
        }

        /// <summary>
        /// Constructor that prevents a default instance of this class from being created.
        /// </summary>
        private Database()
        {
            _database = new ItemListDatabase();
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        ///
        /// <value>
        /// The instance.
        /// </value>
        public static Database Instance => _instance;

        /// <summary>
        /// Gets the database.
        /// </summary>
        ///
        /// <value>
        /// The database.
        /// </value>
        public ItemListDatabase DB => _database;
    }
}
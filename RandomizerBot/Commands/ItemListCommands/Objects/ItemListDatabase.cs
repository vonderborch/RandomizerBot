/// <file>
/// RandomizerBot\Commands\ItemListCommands\Objects\ItemListDatabase.cs
/// </file>
///
/// <copyright file="ItemListDatabase.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the item list database class.
/// </summary>
using Microsoft.Data.Sqlite;
using System.Data;
using System.Text;
using Velentr.Miscellaneous.Sqlite;

namespace RandomizerBot.Commands.ItemListCommands.Objects
{
    /// <summary>
    /// An item list database.
    /// </summary>
    ///
    /// <seealso cref="Database"/>
    public class ItemListDatabase : Velentr.Miscellaneous.Sqlite.Database
    {
        /// <summary>
        /// The database version.
        /// </summary>
        private static string DatabaseVersion = "1";

        /// <summary>
        /// The database file.
        /// </summary>
        private static string DatabaseFile = "randomizer-bot.db";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ItemListDatabase() : base(DatabaseFile)
        {
            // if the database exists, delete it if the version doesn't match what we want
            // longer term, we'll probably want to be able to migrate the database to the new schema instead
            // and the same basic code sequence below can be utilized
            if (File.Exists(DatabaseFile))
            {
                var sql = @"SELECT Value AS Result FROM SaveInfo WHERE Key = 'DatabaseVersion';";

                try
                {
                    var results = ExecuteQuery(sql);

                    if (results.Count == 0)
                    {
                        RemoveConnection();
                        File.Delete(DatabaseFile);
                    }
                    else if (results[0]["Result"].ToString() != DatabaseVersion)
                    {
                        RemoveConnection();
                        File.Delete(DatabaseFile);
                    }
                }
                catch (Exception)
                {
                    RemoveConnection();
                    File.Delete(DatabaseFile);
                }
            }

            if (!File.Exists(DatabaseFile))
            {
                ResetConnection();
                var creationSql = @$"
                CREATE TABLE SaveInfo(
                    [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [Key] nvarchar(64) NOT NULL,
                    [Value] nvarchar(128) NOT NULL,
                    [CreatedAt] TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );

                CREATE TABLE ItemLists(
                    [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [Name] nvarchar(128) NOT NULL,
                    [IsLinkedToServer] bit NOT NULL,
                    [OwnerID] nvarchar(512) NOT NULL,
                    [CreatedAt] TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    [CreatedBy] nvarchar(512) NOT NULL
                );

                CREATE TABLE Items (
                    [ID] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [ListID] INTEGER NOT NULL,
                    [Name] nvarchar(256) NOT NULL,
                    [IsEnabledForRandomization] bit NOT NULL,
                    [Weight] INTEGER NOT NULL,
                    [CreatedAt] TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    [CreatedBy] nvarchar(512) NOT NULL
                );

                INSERT INTO SaveInfo
                    (Key, Value)
                VALUES
                    ('DatabaseVersion', '{DatabaseVersion}')
                ;
                ";
                ResetConnection();
                ExecuteNonQuery(creationSql);
            }
        }

        /// <summary>
        /// Queries if a given item list exists.
        /// </summary>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        public bool ItemListExists(ListKey key)
        {
            var command = new SqliteCommand("SELECT 1 AS ListExists FROM ItemLists AS gl WHERE gl.Name = @Name AND gl.OwnerID = @ID AND gl.IsLinkedToServer = @ServerOwned LIMIT 1;");
            key.AttachParameters(command, false);

            var result = ExecuteQuery(command);

            return result.Count > 0;
        }

        /// <summary>
        /// Creates item list.
        /// </summary>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>
        /// The new item list.
        /// </returns>
        public long CreateItemList(ListKey key)
        {
            var sql = @"
            INSERT INTO ItemLists
                (Name, IsLinkedToServer, OwnerID, CreatedBy)
            VALUES
                (@Name, @ServerOwned, @ID, @UserID);

            SELECT ID FROM ItemLists ORDER BY ID DESC LIMIT 1;
            ";

            var command = new SqliteCommand(sql);
            key.AttachParameters(command, false);
            command.Parameters.Add(new SqliteParameter("UserID", key.UserId.ToString()));

            var result = ExecuteQuery(command);

            return result.Count == 0 ? -1 : (long)result[0]["ID"];
        }

        /// <summary>
        /// Gets list creator.
        /// </summary>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>
        /// The list creator.
        /// </returns>
        public ulong? GetListCreator(ListKey key)
        {
            var sql = @"
            SELECT CreatedBy FROM ItemLists
            WHERE Name = @Name
                AND OwnerID = @ID
                AND IsLinkedToServer = @ServerOwned
            ;
            ";

            var command = new SqliteCommand(sql);
            key.AttachParameters(command, false);

            var result = ExecuteQuery(command);

            return result.Count == 0 ? null : (ulong)result[0]["CreatedBy"];
        }

        /// <summary>
        /// Encapsulates the result of a view all lists.
        /// </summary>
        ///
        /// <seealso cref="IModelParser"/>
        public class ViewAllListsResult : IModelParser
        {
            /// <summary>
            /// The identifier.
            /// </summary>
            public long ID;

            /// <summary>
            /// The name.
            /// </summary>
            public string Name = string.Empty;

            /// <summary>
            /// True if is linked to server, false if not.
            /// </summary>
            public bool IsLinkedToServer;

            /// <summary>
            /// The identifier that owns this item.
            /// </summary>
            public ulong OwnerID;

            /// <summary>
            /// The created at Date/Time.
            /// </summary>
            public DateTime CreatedAt;

            /// <summary>
            /// Amount to created by.
            /// </summary>
            public ulong CreatedBy;

            /// <summary>
            /// Number of items.
            /// </summary>
            public long ItemCount;

            /// <summary>
            /// Parses the given row.
            /// </summary>
            ///
            /// <param name="row">  The row. </param>
            public void Parse(IDataReader row)
            {
                ID = Convert.ToInt64(row["ID"]);
#pragma warning disable CS8601 // Possible null reference assignment.
                Name = Convert.ToString(row["Name"]);
#pragma warning restore CS8601 // Possible null reference assignment.
                IsLinkedToServer = Convert.ToBoolean(row["IsLinkedToServer"]);
                OwnerID = (ulong)Convert.ToInt64(row["OwnerID"]);
                CreatedAt = Convert.ToDateTime(row["CreatedAt"]);
                CreatedBy = (ulong)Convert.ToInt64(row["CreatedBy"]);
                ItemCount = Convert.ToInt64(row["ItemCount"]);
            }

            /// <summary>
            /// Gets a text.
            /// </summary>
            ///
            /// <param name="columns">  The columns. </param>
            ///
            /// <returns>
            /// The text.
            /// </returns>
            public List<string> GetText(List<string> columns)
            {
                // the below is terrible and should _not_ be done, but it was simple, so...
                var output = new List<string>();

                for (var i = 0; i < columns.Count; i++)
                {
                    switch (columns[i].ToLowerInvariant())
                    {
                        case "id":
                            output.Add(ID.ToString());
                            break;

                        case "name":
                            output.Add(Name.ToString());
                            break;

                        case "is server list":
                            output.Add(IsLinkedToServer.ToString());
                            break;

                        case "# items":
                            output.Add(ItemCount.ToString());
                            break;
                    }
                }

                return output;
            }
        }

        /// <summary>
        /// View all lists.
        /// </summary>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>
        /// A List&lt;ViewAllListsResult&gt;
        /// </returns>
        public List<ViewAllListsResult> ViewAllLists(ListKey key)
        {
            var sql = @"
            SELECT il.ID
                , il.Name
                , il.IsLinkedToServer
                , il.OwnerID
                , il.CreatedAt
                , il.CreatedBy
                , COUNT(DISTINCT i.Name) AS ItemCount
            FROM ItemLists AS il
            LEFT JOIN Items AS i
                ON il.ID = i.ListID
            WHERE (
                    il.OwnerID = @UserID
                    AND il.IsLinkedToServer = 0
                )
                OR (
                    il.OwnerID = @ServerID
                    AND il.IsLinkedToServer = 1
                )
            GROUP BY il.ID
                , il.Name
                , il.IsLinkedToServer
                , il.OwnerID
                , il.CreatedAt
                , il.CreatedBy
            ORDER BY il.IsLinkedToServer, il.Name
            ";

            var command = new SqliteCommand(sql);
            command.Parameters.Add(new SqliteParameter("UserID", key.UserId.ToString()));
            command.Parameters.Add(new SqliteParameter("ServerID", key.ServerId.ToString()));

            return ExecuteQuery<ViewAllListsResult>(command);
        }

        /// <summary>
        /// Deletes the item list described by key.
        /// </summary>
        ///
        /// <param name="key">  The key. </param>
        public void DeleteItemList(ListKey key)
        {
            var sql = @"
            DELETE FROM Items WHERE ListID = ( SELECT ID FROM ItemLists WHERE Name = @Name AND OwnerID = @ID AND IsLinkedToServer = @ServerOwned );

            DELETE FROM ItemLists WHERE Name = @Name AND OwnerID = @ID AND IsLinkedToServer = @ServerOwned;
            ";

            var command = new SqliteCommand(sql);
            key.AttachParameters(command, false);

            ExecuteNonQuery(command);
        }

        /// <summary>
        /// An item list items.
        /// </summary>
        ///
        /// <seealso cref="IModelParser"/>
        public class ItemListItems : IModelParser
        {
            /// <summary>
            /// The identifier.
            /// </summary>
            public long ID;

            /// <summary>
            /// The name.
            /// </summary>
            public string Name = string.Empty;

            /// <summary>
            /// True if is enabled for randomization, false if not.
            /// </summary>
            public bool IsEnabledForRandomization;

            /// <summary>
            /// The weight.
            /// </summary>
            public int Weight;

            /// <summary>
            /// Parses the given row.
            /// </summary>
            ///
            /// <param name="row">  The row. </param>
            public void Parse(IDataReader row)
            {
                ID = Convert.ToInt64(row["ID"]);
#pragma warning disable CS8601 // Possible null reference assignment.
                Name = Convert.ToString(row["Name"]);
#pragma warning restore CS8601 // Possible null reference assignment.
                Weight = Convert.ToInt32(row["Weight"]);
                IsEnabledForRandomization = Convert.ToBoolean(row["IsEnabledForRandomization"]);
            }

            /// <summary>
            /// Gets a text.
            /// </summary>
            ///
            /// <param name="columns">  The columns. </param>
            ///
            /// <returns>
            /// The text.
            /// </returns>
            public List<string> GetText(List<string> columns)
            {
                // the below is terrible and should _not_ be done, but it was simple, so...
                var output = new List<string>();

                for (var i = 0; i < columns.Count; i++)
                {
                    switch (columns[i].ToLowerInvariant())
                    {
                        case "id":
                            output.Add(ID.ToString());
                            break;

                        case "name":
                            output.Add(Name.ToString());
                            break;

                        case "randomization enabled?":
                            output.Add(IsEnabledForRandomization.ToString());
                            break;

                        case "randomization weight":
                            output.Add(Weight.ToString());
                            break;
                    }
                }

                return output;
            }
        }

        /// <summary>
        /// Queries if a given item in item list exists.
        /// </summary>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="itemName"> Name of the item. </param>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        public bool ItemInItemListExists(ListKey key, string itemName)
        {
            var sql = @"
            SELECT 1
            FROM Items AS i
            INNER JOIN ItemLists AS il
                ON il.ID = i.ListID
            WHERE il.Name = @Name
                AND il.OwnerID = @ID
                AND il.IsLinkedToServer = @ServerOwned
                AND i.Name = @ItemName
            ";

            var command = new SqliteCommand(sql);
            key.AttachParameters(command, false);
            command.Parameters.Add(new SqliteParameter("ItemName", itemName));

            var result = ExecuteQuery(command);

            return result.Count > 0;
        }

        /// <summary>
        /// Adds an item to item list.
        /// </summary>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="name">     The name. </param>
        /// <param name="weight">   The weight. </param>
        ///
        /// <returns>
        /// A long.
        /// </returns>
        public long AddItemToItemList(ListKey key, string name, int weight)
        {
            var sql = @"
            INSERT INTO Items
                (Name, IsEnabledForRandomization, Weight, CreatedBy, ListID)
            VALUES
                (@ItemName, @Randomize, @Weight, @UserID, (SELECT ID FROM ItemLists AS il WHERE il.Name = @Name AND il.OwnerID = @ID AND il.IsLinkedToServer = @ServerOwned));

            SELECT ID FROM Items ORDER BY ID DESC LIMIT 1;
            ";

            var command = new SqliteCommand(sql);

            key.AttachParameters(command, false);
            command.Parameters.Add(new SqliteParameter("ItemName", name));
            command.Parameters.Add(new SqliteParameter("Randomize", true));
            command.Parameters.Add(new SqliteParameter("Weight", weight));
            command.Parameters.Add(new SqliteParameter("UserID", key.UserId.ToString()));

            var result = ExecuteQuery(command);

            return result.Count == 0 ? -1 : (long)result[0]["ID"];
        }

        /// <summary>
        /// Gets item list items.
        /// </summary>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>
        /// The item list items.
        /// </returns>
        public List<ItemListItems> GetItemListItems(ListKey key)
        {
            var sql = @"
            SELECT i.ID
                , i.Name
                , i.IsEnabledForRandomization
                , i.Weight
            FROM Items AS i
            INNER JOIN ItemLists AS il
                ON il.ID = i.ListID
            WHERE il.Name = @Name
                AND il.OwnerID = @ID
                AND il.IsLinkedToServer = @ServerOwned
            ;
            ";

            var command = new SqliteCommand(sql);
            key.AttachParameters(command, false);

            return ExecuteQuery<ItemListItems>(command);
        }

        /// <summary>
        /// Deletes the item in list.
        /// </summary>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="itemName"> Name of the item. </param>
        public void DeleteItemInList(ListKey key, string itemName)
        {
            var sql = @"
            DELETE FROM Items WHERE ListID = ( SELECT ID FROM ItemLists WHERE Name = @Name AND OwnerID = @ID AND IsLinkedToServer = @ServerOwned ) AND Name = @ItemName;
            ";

            var command = new SqliteCommand(sql);
            key.AttachParameters(command, false);
            command.Parameters.Add(new SqliteParameter("ItemName", itemName));

            ExecuteNonQuery(command);
        }

        /// <summary>
        /// Updates the item in list.
        /// </summary>
        ///
        /// <param name="key">          The key. </param>
        /// <param name="oldName">      Name of the old. </param>
        /// <param name="name">         The name. </param>
        /// <param name="weight">       (Optional) The weight. </param>
        /// <param name="isEnabled">    (Optional) The is enabled. </param>
        public void UpdateItemInList(ListKey key, string oldName, string name, int weight = 0, bool? isEnabled = null)
        {
            var sql = new StringBuilder();
            sql.AppendLine("UPDATE Items SET Name = @ItemName");
            var parametersToAdd = new List<SqliteParameter>();
            if (weight >= 1)
            {
                sql.AppendLine(", Weight = @Weight");
                parametersToAdd.Add(new SqliteParameter("Weight ", weight));
            }
            if (isEnabled != null)
            {
                sql.AppendLine(", IsEnabledForRandomization = @Randomize ");
                parametersToAdd.Add(new SqliteParameter("Randomize", (bool)isEnabled));
            }
            sql.AppendLine(@"WHERE ListID = ( SELECT ID FROM ItemLists WHERE Name = @Name AND OwnerID = @ID AND IsLinkedToServer = @ServerOwned ) AND Name = @OldItemName;");

            var command = new SqliteCommand(sql.ToString());
            key.AttachParameters(command, false);
            command.Parameters.Add(new SqliteParameter("ItemName", name));
            command.Parameters.Add(new SqliteParameter("OldItemName", oldName));
            for (var i = 0; i < parametersToAdd.Count; i++)
            {
                command.Parameters.Add(parametersToAdd[i]);
            }

            ExecuteNonQuery(command);
        }

        /// <summary>
        /// Updates the item list name.
        /// </summary>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="newName">  Name of the new. </param>
        public void UpdateItemListName(ListKey key, string newName)
        {
            var sql = @"
            UPDATE ItemLists
            SET Name = @NewName
            WHERE Name = @Name
                AND OwnerID = @ID
                AND IsLinkedToServer = @ServerOwned
            ";

            var command = new SqliteCommand(sql);
            key.AttachParameters(command, false);
            command.Parameters.Add(new SqliteParameter("NewName", newName));

            ExecuteNonQuery(command);
        }
    }
}
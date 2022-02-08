/// <file>
/// RandomizerBot\Commands\ItemListCommands\Objects\Parameters.cs
/// </file>
///
/// <copyright file="Parameters.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the parameters class.
/// </summary>
namespace RandomizerBot.Commands.ItemListCommands.Objects
{
    /// <summary>
    /// A parameters.
    /// </summary>
    public struct Parameters
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="key">              The key. </param>
        /// <param name="personalKey">      The personal key. </param>
        /// <param name="serverKey">        The server key. </param>
        /// <param name="personalExists">   True to personal exists. </param>
        /// <param name="serverExists">     True to server exists. </param>
        public Parameters(ListKey key, ListKey personalKey, ListKey serverKey, bool personalExists, bool serverExists)
        {
            Key = key;
            PersonalKey = personalKey;
            ServerKey = serverKey;
            PersonalExists = personalExists;
            ServerExists = serverExists;
        }

        /// <summary>
        /// The key.
        /// </summary>
        public ListKey Key;

        /// <summary>
        /// The personal key.
        /// </summary>
        public ListKey PersonalKey;

        /// <summary>
        /// The server key.
        /// </summary>
        public ListKey ServerKey;

        /// <summary>
        /// True to personal exists.
        /// </summary>
        public bool PersonalExists;

        /// <summary>
        /// True to server exists.
        /// </summary>
        public bool ServerExists;
    }
}
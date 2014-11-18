// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Parser.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014.
//   This software is licenced like https://github.com/twitter/twemoji :
//   Code licensed under the MIT License: http://opensource.org/licenses/MIT
//   Graphics licensed under CC-BY 4.0: https://creativecommons.org/licenses/by/4.0/ and created by Twitter
// </copyright>
// <summary>
//   The Main Parser
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FrwTwemoji
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// The Main Parser
    /// </summary>
    public static class Parser
    {
        /// <summary>
        /// Parses the string and replaces emoji emoji.
        /// </summary>
        /// <param name="fromString">From string.</param>
        /// <returns>a string that contains the originale string with emoji replaced by image</returns>
        public static string WebParseEmoji(string fromString)
        {
            return WebParseEmoji(
                fromString,
                new ParserOptions
                    {
                        AssetSize = ParserOptions.AssetSizes.Render16Px,
                        RessourcesProvider = ParserOptions.RessourcesProviders.Localhost
                    });
        }

        /// <summary>
        /// Parses the string and replaces emoji emoji.
        /// </summary>
        /// <param name="fromString">From string.</param>
        /// <param name="options">The options to use.</param>
        /// <returns>
        /// a string that contains the originale string with emoji replaced by image
        /// </returns>
        public static string WebParseEmoji(string fromString, ParserOptions options)
        {
            Match match = Regex.Match(fromString, RegEx.EmojiSearchPattern);
            if (match.Success)
            {
                var caps = match.Captures;
            }
            
            
            
            return fromString;
        }
    }
}

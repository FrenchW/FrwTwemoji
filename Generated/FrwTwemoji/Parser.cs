// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Parser.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014.
//   This software is licenced like https://github.com/twitter/twemoji :
//   Code licensed under the MIT License: http://opensource.org/licenses/MIT
//   Graphics licensed under CC-BY 4.0: https://creativecommons.org/licenses/by/4.0/ and created by Twitter
// </copyright>
// <summary>
//   Parser is where the magic happens. It parses the input string into html emoji
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FrwTwemoji
{
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;

    /// <summary>
    /// Parser is where the magic happens. It parses the input string into html emoji
    /// </summary>
    public class Parser
    {
        /// <summary>Internal File Size memory.</summary>
        private Helpers.AssetSizes internFileSize;

        /// <summary>Internal File Type memory.</summary>
        private Helpers.AssetTypes internFileType;

        /// <summary>Internal provider memory.</summary>
        private Helpers.RessourcesProviders internProvider;

        /// <summary>Initializes a new instance of the <see cref="Parser"/> class.</summary>
        /// <param name="size">The size.</param>
        /// <param name="type">The type.</param>
        /// <param name="provider">The provider.</param>
        public Parser(
            Helpers.AssetSizes size = Helpers.AssetSizes.Render16Px,
            Helpers.AssetTypes type = Helpers.AssetTypes.Png,
            Helpers.RessourcesProviders provider = Helpers.RessourcesProviders.Localhost)
        {
            this.internFileSize = size;
            this.internFileType = type;
            this.internProvider = provider;
        }

        /// <summary>Parses the string and renders emoji.</summary>
        /// <param name="input">The input string.</param>
        /// <param name="size">The size  to use for the emoji images.</param>
        /// <param name="type">The type of images to use.</param>
        /// <param name="provider">The provider for the resources.</param>
        /// <returns>the whole html output</returns>
        public static string ParseEmoji(
            string input,
            Helpers.AssetSizes size = Helpers.AssetSizes.Render16Px,
            Helpers.AssetTypes type = Helpers.AssetTypes.Png,
            Helpers.RessourcesProviders provider = Helpers.RessourcesProviders.Localhost)
        {
            Parser p = new Parser();
            return p.WebParseEmoji(input);
        }

        /// <summary>Gets the name of the web resource.</summary>
        /// <param name="emoji">The emoji.</param>
        /// <returns>the name of the webresource</returns>
        protected internal string GetWebResourceName(string emoji)
        {
            if (internProvider == Helpers.RessourcesProviders.Localhost)
            {
                if (internFileType == Helpers.AssetTypes.Svg)
                {
                    return Helpers.GetEmojiAssemblyName(emoji, Helpers.AssetPackFromTwemoji.PackSvg);
                }

                if (internFileSize == Helpers.AssetSizes.Render36Px)
                {
                    return Helpers.GetEmojiAssemblyName(emoji, Helpers.AssetPackFromTwemoji.Pack36X36);
                }

                if (internFileSize == Helpers.AssetSizes.Render72Px)
                {
                    return Helpers.GetEmojiAssemblyName(emoji, Helpers.AssetPackFromTwemoji.Pack72X72);
                }

                return Helpers.GetEmojiAssemblyName(emoji, Helpers.AssetPackFromTwemoji.Pack16X16);
            }

            string output = "http";
            emoji = emoji.ToLowerInvariant();

            HttpContext context = HttpContext.Current;
            if (context != null && context.Request.Url.Scheme == "https")
            {
                output = "https";
            }

            output += "://twemoji.maxcdn.com/";
            if (internFileType == Helpers.AssetTypes.Svg)
            {
                return output
                    + "svg/"
                    + emoji
                    + ".svg";
            }

            return output + Helpers.GetAssetPackFolderName(internFileSize) + "/" + emoji + ".png";
        }

        /// <summary>
        /// Parses the string and replaces emoji emoji.
        /// </summary>
        /// <param name="input">From string.</param>
        /// <returns>
        /// a string that contains the originale string with emoji replaced by image
        /// </returns>
        internal string WebParseEmoji(string input)
        {
            return Regex.Replace(input, RegEx.EmojiSearchPattern, this.WebParseEmojiRegExMatchEvaluator);
        }

        /// <summary>Evaluator for each Regex match.</summary>
        /// <param name="match">The match.</param>
        /// <returns>the string after evaluation of the match</returns>
        private string WebParseEmojiRegExMatchEvaluator(Match match)
        {
            int codepoint = Helpers.ConvertUtf16ToCodePoint(match.Value);
            string emoji = string.Format("{0:x}", codepoint).ToUpperInvariant();
            string url;
            if (this.internProvider == Helpers.RessourcesProviders.Localhost)
            {
                string resourceName = this.GetWebResourceName(emoji);
                url = new Page().ClientScript.GetWebResourceUrl(this.GetType(), resourceName);
            }
            else
            {
                url = this.GetWebResourceName(emoji);
            }

            return string.Format("<img class=\"emoji FrwTwemoji\" draggable=\"false\" alt=\"{1}\"  src=\"{0}\"/>", url, match.Value);
        }
    }
}

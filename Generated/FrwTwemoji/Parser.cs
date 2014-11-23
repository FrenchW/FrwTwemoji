namespace FrwTwemoji
{
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;

    /// <summary>
    /// Parser is where the magic heppens. It parses the input string into html emoji
    /// </summary>
    public class Parser
    {
        public static string ParseEmoji(
            string input,
            Helpers.AssetSizes size = Helpers.AssetSizes.Render16Px,
            Helpers.AssetTypes type = Helpers.AssetTypes.Png,
            Helpers.RessourcesProviders provider = Helpers.RessourcesProviders.Localhost)
        {
            Parser p = new Parser();
            return p.WebParseEmoji(input);
        }

        public Parser(
            Helpers.AssetSizes size = Helpers.AssetSizes.Render16Px,
            Helpers.AssetTypes type = Helpers.AssetTypes.Png,
            Helpers.RessourcesProviders provider = Helpers.RessourcesProviders.Localhost)
        {
            internFileSize = size;
            internFileType = type;
            internProvider = provider;

        }

        private Helpers.AssetSizes internFileSize;

        private Helpers.AssetTypes internFileType;

        private Helpers.RessourcesProviders internProvider;


        protected internal string GetWebResourceName(string emoji)
        {
            if (internProvider == Helpers.RessourcesProviders.Localhost)
            {

                Helpers.AssetPackFromTwemoji pack = Helpers.AssetPackFromTwemoji.Pack16X16;
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
        /// <param name="fromString">From string.</param>
        /// <returns>
        /// a string that contains the originale string with emoji replaced by image
        /// </returns>
        internal protected string WebParseEmoji(string input)
        {
            return Regex.Replace(input, RegEx.EmojiSearchPattern, this.WebParseEmojiRegExMatchEvaluator);
        }

        private string WebParseEmojiRegExMatchEvaluator(Match match)
        {
            int codepoint = Helpers.ConvertUtf16ToCodePoint(match.Value);
            string emoji = string.Format("{0:x}", codepoint).ToUpperInvariant();
            string url;
            if (internProvider == Helpers.RessourcesProviders.Localhost)
            {

            string resourceName = this.GetWebResourceName(emoji);
             url = new Page().ClientScript.GetWebResourceUrl(this.GetType(), resourceName);
            }
            else
            {
                url = this.GetWebResourceName(emoji);
            }
            return string.Format("<img src=\"{0}\"/>", url);
        }
    }
}

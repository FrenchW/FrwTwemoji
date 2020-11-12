// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Parser.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014-2020.
//   FrwTwemoji Project page : http://github.frenchw.net/FrwTwemoji/
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
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
#if DOTNETFRAMEWORK
    using System.Web;
    using System.Web.UI;
#endif 

    /// <summary>
    /// Parser is where the magic happens. It parses the input string into html emoji
    /// </summary>
    public class Parser
    {
        const char u200D = '\u200D'; // 8205
        const char uFE0F = '\uFE0F'; // 65039
        const char uD83C = '\uD83C'; // 55356
        const char uDFFC = '\uDFFC'; // 57340
        const char uDFFD = '\uDFFD'; // 57341
        const char uDFFE = '\uDFFE'; // 57342
        const char uDFFF = '\uDFFF'; // 57343


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
            Helpers.AssetSizes size = Helpers.AssetSizes.Render72Px,
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
            Helpers.AssetSizes size = Helpers.AssetSizes.Render72Px,
            Helpers.AssetTypes type = Helpers.AssetTypes.Png
#if DOTNETFRAMEWORK
            , Helpers.RessourcesProviders provider = Helpers.RessourcesProviders.Localhost
#endif
            )
        {
#if DOTNETFRAMEWORK
            return new Parser(size, type, provider).WebParseEmoji(input);
#endif
            return new Parser(size, type, Helpers.RessourcesProviders.MaxCdn).WebParseEmoji(input);
        }

        /// <summary>Gets the name of the web resource.</summary>
        /// <param name="emoji">The emoji.</param>
        /// <returns>the name of the webresource</returns>
        protected internal string GetWebResourceName(string emoji)
        {
            if (this.internProvider == Helpers.RessourcesProviders.Localhost)
            {
                if (this.internFileType == Helpers.AssetTypes.Svg)
                {
                    return Helpers.GetEmojiAssemblyName(emoji, Helpers.AssetPackFromTwemoji.PackSvg);
                }

                return Helpers.GetEmojiAssemblyName(emoji, Helpers.AssetPackFromTwemoji.Pack72X72);
            }

            string output = "https";
            emoji = emoji.ToLowerInvariant();
#if DOTNETFRAMEWORK
            HttpContext context = HttpContext.Current;
            if (context != null && context.Request.Url.Scheme == "http")
            {
                output = "http";
            }

#endif
            output += "://twemoji.maxcdn.com/2/";
            if (this.internFileType == Helpers.AssetTypes.Svg)
            {
                return output + "svg/" + emoji + ".svg";
            }

            return output + Helpers.GetAssetPackFolderName(this.internFileSize) + "/" + emoji + ".png";
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
            string emoji = string.Empty;
            char[] s = match.Value.ToCharArray();
            int upperboundOfS = s.GetUpperBound(0);
            int codepoint = 0;
            try
            {
                if (upperboundOfS < 2)
                {
                    if (upperboundOfS == 1 && s[1] == uFE0F)
                    {
                        codepoint = Helpers.ConvertUtf16ToCodePoint(new string(new[] { s[0] }));
                    }
                    else
                    {
                        codepoint = Helpers.ConvertUtf16ToCodePoint(match.Value);
                    }
                    emoji = string.Format("{0:x}", codepoint).ToUpperInvariant();
                }
                else
                {
                    int i = 0;
                    while (i <= upperboundOfS)
                    {
                        if (emoji.Length > 0)
                        {
                            emoji += "-";
                        }

                        if (s[i] != u200D)
                        {
                            if (i + 1 <= upperboundOfS && s[i + 1] != u200D)
                            {
                                if (i + 2 <= upperboundOfS && s[i + 1] == uD83C) // XXXX - 55356 - 57343
                                {
                                    codepoint = Helpers.ConvertUtf16ToCodePoint(new string(new char[] { s[i] }));
                                    emoji += $"{codepoint:x}".ToUpperInvariant();
                                    if (s[i + 2] == uDFFC)// XXXX - 55356 - 57340
                                    {
                                        emoji += "-1F3FC";
                                    }
                                    if (s[i + 2] == uDFFD)// XXXX - 55356 - 57341
                                    {
                                        emoji += "-1F3FD";
                                    }
                                    if (s[i + 2] == uDFFE)// XXXX - 55356 - 57342
                                    {
                                        emoji += "-1F3FE";
                                    }
                                    if (s[i + 2] == uDFFF)// XXXX - 55356 - 57343
                                    {
                                        emoji += "-1F3FF";
                                    }

                                    i += 3;
                                }
                                else
                                {
                                    if (s[i + 1] == uFE0F)
                                    {
                                        // Issue #10 when there is 2️⃣ in the text : https://github.com/FrenchW/FrwTwemoji/issues/10
                                        // s[0]: 50
                                        // s[1]: 65039
                                        // s[2]: 8419
                                        codepoint = Helpers.ConvertUtf16ToCodePoint(new string(new char[] { s[i + 2] }));
                                        int codepoint0 = Helpers.ConvertUtf16ToCodePoint(new string(new char[] { s[i] }));
                                        emoji += $"{codepoint0:x}-".ToUpperInvariant() + $"{codepoint:x}".ToUpperInvariant();
                                        i += 3;
                                    }
                                    else
                                    {
                                        if (i + 2 <= upperboundOfS && s[i + 2] == uFE0F)
                                        {
                                            // Issue when there is {🅰️}	 in the text
                                            // s[0]: 55356
                                            // s[1]: 56688
                                            // s[2]: 65039
                                            codepoint = Helpers.ConvertUtf16ToCodePoint(new string(new char[] { s[i], s[i + 1] }));
                                            emoji += $"{codepoint:x}".ToUpperInvariant();
                                            i += 3;

                                        }
                                        else
                                        {
                                            codepoint = Helpers.ConvertUtf16ToCodePoint(new string(new char[] { s[i], s[i + 1] }));
                                            emoji += $"{codepoint:x}".ToUpperInvariant();
                                            i += 2;
                                        }
                                    }
                                }

                            }
                            else
                            {
                                codepoint = Helpers.ConvertUtf16ToCodePoint(new string(new char[] { s[i], s[i + 1] }));
                                emoji += $"{codepoint:x}".ToUpperInvariant();
                                i += 1;
                            }
                        }
                        else
                        {
                            if (i + 2 <= upperboundOfS && s[i + 2] == uFE0F)
                            {
                                codepoint = Helpers.ConvertUtf16ToCodePoint(new string(new char[] { s[i + 1] }));
                                emoji += "200D-" + $"{codepoint:x}".ToUpperInvariant() + "-FE0F";
                                i += 3;
                            }
                            else
                            {
                                if (i + 2 <= upperboundOfS && s[i + 2] != u200D)
                                {
                                    codepoint = Helpers.ConvertUtf16ToCodePoint(new string(new char[] { s[i + 1], s[i + 2] }));
                                    emoji += "200D-" + $"{codepoint:x}".ToUpperInvariant();
                                    i += 3;

                                }
                                else
                                {
                                    codepoint = Helpers.ConvertUtf16ToCodePoint(new string(new char[] { s[i + 1] }));
                                    emoji += "200D-" + $"{codepoint:x}".ToUpperInvariant();
                                    i += 2;
                                }

                            }
                        }


                    }
                }
            }
            catch
            {
                codepoint = Helpers.ConvertUtf16ToCodePoint("🆘");
                emoji = string.Format("{0:x}", codepoint).ToUpperInvariant();
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }



            string url = this.GetWebResourceName(emoji);
#if DOTNETFRAMEWORK
            if (this.internProvider == Helpers.RessourcesProviders.Localhost)
            {
                string resourceName = this.GetWebResourceName(emoji);
                url = new Page().ClientScript.GetWebResourceUrl(this.GetType(), resourceName);
            }

#endif
            string pixelSize = "72px";
            switch (this.internFileSize)
            {
                case Helpers.AssetSizes.Render16Px:
                    pixelSize = "16px";
                    break;
                case Helpers.AssetSizes.Render36Px:
                    pixelSize = "36px";
                    break;
                case Helpers.AssetSizes.Render72Px:
                    pixelSize = "72px";
                    break;
                case Helpers.AssetSizes.Render128Px:
                    pixelSize = "128px";
                    break;
                case Helpers.AssetSizes.Render256Px:
                    pixelSize = "256px";
                    break;
                case Helpers.AssetSizes.Render512Px:
                    pixelSize = "512px";
                    break;
                case Helpers.AssetSizes.Render1Em:
                    pixelSize = "1em";
                    break;

            }

            return $"<img class=\"emoji FrwTwemoji\" style=\"height:{pixelSize};Width:{pixelSize};\" draggable=\"false\" alt=\"{match.Value}\"  src=\"{url}\"/>";
        }
    }
}

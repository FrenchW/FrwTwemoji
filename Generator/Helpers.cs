// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helpers.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014-2020.
//   FrwTwemoji Project page : http://github.frenchw.net/FrwTwemoji/
//   This software is licenced like https://github.com/twitter/twemoji :
//   Code licensed under the MIT License: http://opensource.org/licenses/MIT
//   Graphics licensed under CC-BY 4.0: https://creativecommons.org/licenses/by/4.0/ and created by Twitter
// </copyright>
// <summary>
//   Handy Tools
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace

namespace FrwTwemoji
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Handy Tools
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// The base known asset names.
        /// </summary>
        internal static readonly AssetPackFromTwemoji[] BaseKnownAssetNames =
            {
                AssetPackFromTwemoji.Pack72X72,
                AssetPackFromTwemoji.PackSvg
            };

        /// <summary>
        /// The size to render the icons
        /// </summary>
        public enum AssetSizes
        {
            /// <summary>
            /// Render in 16 pixels
            /// </summary>
            Render16Px,

            /// <summary>
            /// Render in 36 pixels
            /// </summary>
            Render36Px,

            /// <summary>
            /// Render in 72 pixels
            /// </summary>
            Render72Px,

            /// <summary>
            /// Render in 128 pixels
            /// </summary>
            Render128Px,

            /// <summary>
            /// Render in 256 pixels
            /// </summary>
            Render256Px,

            /// <summary>
            /// Render in 512 pixels
            /// </summary>
            Render512Px,

            /// <summary>
            /// Render in 1 em
            /// </summary>
            Render1Em,
        }

        /// <summary>
        /// Emoji provider: local or a CDN
        /// </summary>
        public enum RessourcesProviders
        {
            /// <summary>
            /// Local Resource are used
            /// </summary>
            Localhost,

            /// <summary>
            /// CDN : MaxCDN
            /// </summary>
            MaxCdn
        }

        /// <summary>
        /// Folders provided by twitter in Twemoji
        /// </summary>
        public enum AssetPackFromTwemoji
        {
            /// <summary>
            /// 72px X 72px png images
            /// </summary>
            Pack72X72,

            /// <summary>
            /// Svg images
            /// </summary>
            PackSvg
        }

        /// <summary>
        /// Emoji provider: local or a CDN
        /// </summary>
        public enum AssetTypes
        {
            /// <summary>
            /// Assets in png format
            /// </summary>
            Png,

            /// <summary>
            /// Assets in Svg format
            /// </summary>
            Svg
        }

        /// <summary>
        /// Get the folder name associated with a Twemoji pack provided bu Twitter
        /// </summary>
        /// <param name="emojiName">Name of the emoji.</param>
        /// <param name="assetPack">The asset pack.</param>
        /// <returns>The name of the folder, not the entire path</returns>
        public static string GetEmojiAssemblyName(string emojiName, AssetPackFromTwemoji assetPack)
        {
            return string.Format(
                "FrwTwemoji.{0}.{1}.{2}",
                GetAssetPackCompilationConstant(assetPack),
                emojiName.ToUpperInvariant(),
                GetAssetPackImageExtension(assetPack));
        }

        /// <summary>Get the folder name associated with a Twemoji pack provided bu Twitter
        /// </summary>
        /// <param name="assetPack">The asset pack.</param>
        /// <returns>The name of the folder, not the entire path</returns>
        public static string GetAssetPackFolderName(AssetPackFromTwemoji assetPack)
        {
            switch (assetPack)
            {
                case AssetPackFromTwemoji.Pack72X72:
                    return "72x72";
                // ReSharper disable once RedundantCaseLabel
                case AssetPackFromTwemoji.PackSvg:
                default:
                    return "Svg";
            }
        }

        /// <summary>Get the folder name associated with a Twemoji pack provided bu Twitter
        /// </summary>
        /// <param name="assetSize">The asset size.</param>
        /// <returns>The name of the folder, not the entire path</returns>
        public static string GetAssetPackFolderName(AssetSizes assetSize)
        {
            switch (assetSize)
            {
                case AssetSizes.Render16Px:
                case AssetSizes.Render36Px:
                case AssetSizes.Render72Px:
                    return "72x72";
                // ReSharper disable once RedundantCaseLabel
                default:
                    throw new ArgumentException("No folder for this size : " + assetSize.ToString());
            }
        }

        /// <summary>Get the folder name associated with a Twemoji pack provided bu Twitter
        /// </summary>
        /// <param name="assetPack">The asset pack.</param>
        /// <returns>The name of the folder, not the entire path</returns>
        public static string GetAssetPackPath(AssetPackFromTwemoji assetPack)
        {
            return GetRootPath() + GetAssetPackFolderName(assetPack) + "\\";
        }

        /// <summary>Get the folder name associated with a Twemoji pack provided bu Twitter
        /// </summary>
        /// <param name="assetPack">The asset pack.</param>
        /// <returns>The name of the folder, not the entire path</returns>
        public static string GetAssetPackCompilationConstant(AssetPackFromTwemoji assetPack)
        {
            // "Icons" + 16x(16) >> Icons16x
            return "Icons" + GetAssetPackName(assetPack).Substring(0, 3);
        }

        /// <summary>Get the folder name associated with a Twemoji pack provided bu Twitter
        /// </summary>
        /// <param name="assetPack">The asset pack.</param>
        /// <returns>The name of the folder, not the entire path</returns>
        public static string GetAssetPackMimeType(AssetPackFromTwemoji assetPack)
        {
            // "Icons" + 16x(16) >> Icons16x
            return GetAssetPackName(assetPack).ToLowerInvariant().Equals("svg") ? "image/svg+xml" : "image/png";
        }

        /// <summary>Get the folder name associated with a Twemoji pack provided bu Twitter
        /// </summary>
        /// <param name="assetPack">The asset pack.</param>
        /// <returns>The name of the folder, not the entire path</returns>
        public static string GetAssetPackName(AssetPackFromTwemoji assetPack)
        {
            // "16x16" >> "16x"
            return GetAssetPackFolderName(assetPack).Substring(0, 3);
        }

        /// <summary>Get the folder name associated with a Twemoji pack provided bu Twitter
        /// </summary>
        /// <param name="assetPack">The asset pack.</param>
        /// <returns>The name of the folder, not the entire path</returns>
        public static string GetAssetPackImageExtension(AssetPackFromTwemoji assetPack)
        {
            switch (assetPack)
            {
                case AssetPackFromTwemoji.Pack72X72:
                    return "png";
                // ReSharper disable once RedundantCaseLabel
                case AssetPackFromTwemoji.PackSvg:
                default:
                    return "svg";
            }
        }

        /// <summary>
        /// Gets the root path of the repository to help build local file structure.
        /// </summary>
        /// <returns>Path to the root</returns>
        internal static string GetRootPath()
        {
            string rootPath = Assembly.GetExecutingAssembly().Location;

#if DEBUG
            {
                return rootPath.Replace("Generator\\bin\\Debug\\Generator.exe", string.Empty);
            }
#else
            {
                return rootPath.Replace("Generator\\bin\\Release\\Generator.exe", string.Empty);
            }
#endif
        }

        #region CodePoint Tools

        /// <summary>
        /// Converts UTF16 to CodePoint.
        /// </summary>
        /// <param name="utf16">The UTF16 caracter.</param>
        /// <returns>An int representing the CodePoint from the Utf16convertion </returns>
        internal static int ConvertUtf16ToCodePoint(string utf16)
        {
            char[] s = utf16.ToCharArray();
            int retval;

            if (s.GetUpperBound(0) == 0)
            {
                retval = char.ConvertToUtf32(utf16, 0);
            }
            else
            {
                retval = char.ConvertToUtf32(s[0], s[1]);
            }

            // Console.WriteLine(@"ConvertUtf16ToCodePoint) {1} => 0x{0:X}", retval, Show(utf16));
            return retval;
        }

        /// <summary>
        /// Converts CodePoint to UTF16.
        /// </summary>
        /// <param name="codePoint">The CodePoint.</param>
        /// <returns>A string from the CodePoint conversion</returns>
        internal static string ConvertCodePointToUtf16(int codePoint)
        {
            return char.ConvertFromUtf32(codePoint);
        }

        /// <summary>
        /// Shows Utf-16 value in plain text
        /// </summary>
        /// <param name="s">Utf-16 string</param>
        /// <returns>A visual representation of the caracters</returns>
        internal static string Show(string s)
        {
            string retval = string.Empty;
            for (int x = 0; x < s.Length; x++)
            {
                retval += string.Format(
                    "0x{0:X}{1}",
                    (int)s[x],
                    (x == s.Length - 1) ? string.Empty : ", ");
            }

            return retval;
        }

        /// <summary>
        /// Shows Utf-16 value in plain text
        /// </summary>
        /// <param name="s">Utf-16 string</param>
        /// <returns>A visual representation of the caracters</returns>
        internal static string ShowU(string s)
        {
            string retval = string.Empty;
            for (int x = 0; x < s.Length; x++)
            {
                string t = string.Format(
                    "\\\\u{0,4:X}{1}",
                    (int)s[x],
                    string.Empty);

                retval += t.Replace(" ", "0");
            }

            return retval;
        }

        #endregion
    }
}

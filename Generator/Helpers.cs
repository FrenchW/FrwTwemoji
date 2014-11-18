// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helpers.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014.
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
    internal static class Helpers
    {
        /// <summary>The base known asset names.</summary>
        internal static readonly string[] BaseKnownAssetNames = { "16x16", "36x36", "72x72", "Svg" };

        /// <summary>
        /// Gets the root path of the repository to help build local file structure.
        /// </summary>
        /// <returns>Path to the root</returns>
        internal static string GetRootPath()
        {
            string rootPath = Assembly.GetExecutingAssembly().Location;

#if DEBUG
            {
                return rootPath.Replace("Generator\\bin\\Debug\\Generator.exe", String.Empty);
            }
#else
            {
                return rootPath.Replace("Generator\\bin\\Release\\Generator.exe", string.Empty);
            }
#endif
        }

        /// <summary>
        /// Converts UTF16 to CodePoint.
        /// </summary>
        /// <param name="utf16">The UTF16 caracter.</param>
        /// <returns>An int representing the CodePoint from the Utf16convertion </returns>
        internal static int ConvertUtf16ToCodePoint(string utf16)
        {
            char[] s = utf16.ToCharArray();
            int retval = Char.ConvertToUtf32(s[1], s[2]);
            Console.WriteLine(@"ConvertUtf16ToCodePoint) {1} => 0x{0:X}", retval, Show(utf16));
            return retval;
        }

        /// <summary>
        /// Converts CodePoint to UTF16.
        /// </summary>
        /// <param name="codePoint">The CodePoint.</param>
        /// <returns>A string from the CodePoint conversion</returns>
        internal static string ConvertCodePointToUtf16(int codePoint)
        {
            string retval = Char.ConvertFromUtf32(codePoint);
            Console.WriteLine(@"ConvertCodePointToUtf16) 0x{0:X} => {1}", codePoint, Show(retval));
            return retval;
        }

        /// <summary>
        /// Shows Utf-16 value in plain text
        /// </summary>
        /// <param name="s">Utf-16 string</param>
        /// <returns>A visual representation of the caracters</returns>
        internal static string Show(string s)
        {
            string retval = String.Empty;
            for (int x = 0; x < s.Length; x++)
            {
                retval += String.Format(
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
            string retval = String.Empty;
            for (int x = 0; x < s.Length; x++)
            {
                string t = String.Format(
                    "\\\\u{0,4:X}{1}",
                    (int)s[x],
                    (x == s.Length - 1) ? string.Empty : string.Empty);
                
                retval += t.Replace(" ","0");
            }

            return retval;
        }
    }
}

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

namespace Generator
{
    using System.Reflection;

    /// <summary>
    /// Handy Tools
    /// </summary>
    internal static class Helpers
    {
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
    }
}

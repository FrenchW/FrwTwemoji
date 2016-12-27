// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014-2016.
//   FrwTwemoji Project page : http://github.frenchw.net/FrwTwemoji/
//   This software is licenced like https://github.com/twitter/twemoji :
//   Code licensed under the MIT License: http://opensource.org/licenses/MIT
//   Graphics licensed under CC-BY 4.0: https://creativecommons.org/licenses/by/4.0/ and created by Twitter
// </copyright>
// <summary>
//   Main startup program
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Generator
{
    using System;
    using System.Linq;

    using FrwTwemoji;

    /// <summary>
    /// Main startup program
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args">Startup arguments arguments.</param>
        public static void Main(string[] args)
        {
            bool forceRebuild = args.Contains("-ForceRebuild");

            // Load and save assets to disk
            var localAssets = new AssetCollection(Helpers.BaseKnownAssetNames);

            localAssets.LoadLocalAssets();

            var previousAssets = new AssetCollection(Helpers.GetRootPath() + Paths.FilePreviousAssetsBackup);

            if (forceRebuild || !localAssets.Equals(previousAssets))
            {
                new SolutionBuilder(localAssets).ReBuild();
            }
            else
            {
                Console.WriteLine(Strings.Program_Main_Nothing_changed_quitting);
            }

            Console.WriteLine(Strings.Program_Main_Press_a_key_to_end);
            Console.ReadKey();
        }
    }
}

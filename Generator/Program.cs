// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014.
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Main startup program
    /// </summary>
   public static class Program
    {
        /// <summary>The base known asset names.</summary>
        private static readonly string[] BaseKnownAssetNames = { "16x16", "36x36", "72x72", "svg" };

        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args">Startup arguments arguments.</param>
        public static void Main(string[] args)
        {
            bool forceRebuild = args.Contains("-ForceRebuild");

            // Load and save assets to disk
            var assets = new AssetCollection(BaseKnownAssetNames);

            assets.LoadLocalAssets();

            var previousAssets = new AssetCollection(Helpers.GetRootPath() + Paths.FilePreviousAssetsBackup);

            if (forceRebuild || !assets.Equals(previousAssets))
            {
                // Assets have changed. We must rebuild each project
                LoadDistantAssets(assets);
                RebuildSolutions(assets);
                assets.Backup(Helpers.GetRootPath() + Paths.FilePreviousAssetsBackup);
            }
            else
            {
                Console.WriteLine(Strings.Program_Main_Nothing_changed_quitting);
            }

            Console.WriteLine(Strings.Program_Main_Press_a_key_to_end);
            Console.ReadKey();
        }

        /// <summary>
        /// Loads information from unicode.org, creates assets with it and compare to the local assets
        /// </summary>
        /// <param name="localAssets">The local assets.</param>
       private static void LoadDistantAssets(AssetCollection localAssets)
        {
            //// Load unicode site info and compare

            var assetsMissing = new AssetCollection(BaseKnownAssetNames);
           
            var missingGrouped = new List<string>();

            var webClient = new WebClient();

            Console.WriteLine(Strings.Program_DoRebuild_fetching_EmojiSources_txt);

            // Loading unicode reference
            webClient.DownloadFile(Paths.UrlEmojiSources, Helpers.GetRootPath() + Paths.FileEmojiSources);

            // read emojisource file
            string emojiSource = File.ReadAllText(Helpers.GetRootPath() + Paths.FileEmojiSources);

            // list to store valid emoji entries in the file

            // split in lines
            string[] emojiSourceLines = emojiSource.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine(Strings.Program_DoRebuild_analizing_EmojiSources_VS_our_assets);

            // try read emoji for each line
            var emojiSourceEntries = (
                from line 
                    in emojiSourceLines 
                where Regex.IsMatch(line, "^[0-9A-F]") 
                select line.Substring(0, line.IndexOf(";", StringComparison.InvariantCulture)).ToUpperInvariant() into entry 
                select Regex.Replace(entry, "\\s+", "-") into entry 
                select Regex.Replace(entry, "^0+", string.Empty)).ToList();

            Console.WriteLine(Strings.Program_DoRebuild_INFO_parsed_0_standard_emoji, emojiSourceEntries.Count);
           
            string[] ignoreMissing = { "2002", "2003", "2005" };

            //// now that all is loaded, perform some crossing

            foreach (var entry in emojiSourceEntries)
            {
                if (!ignoreMissing.Contains(entry))
                {
                    foreach (var asset in localAssets)
                    {
                        if (!asset.Emoji.Contains(entry))
                        {
                            if (assetsMissing[asset.Name] != null)
                            {
                                assetsMissing[asset.Name].Emoji.Add(entry);
                            }

                            if (!missingGrouped.Contains(entry))
                            {
                                missingGrouped.Add(entry);
                            }
                        }
                    }
                }
            }

            foreach (var missingItem in assetsMissing)
            {
                if (missingItem.Emoji.Count > 0)
                {
                    Console.WriteLine(
                        Strings.Program_DoRebuild_WARNING_missing_emoji_for_assets_X_X,
                        missingItem.Name,
                        string.Join(", ", missingItem.Emoji));
                }
            }

            ignoreMissing = ignoreMissing.Concat(missingGrouped).ToArray();

            Console.WriteLine(Strings.Program_DoRebuild_fetching_StandardizedVariants_txt);

            // Loading unicode reference
            webClient.DownloadFile(Paths.UrlStandardizedVariants, Helpers.GetRootPath() + Paths.FileStandardizedVariants);

            // read emojiVariants file
            string emojiVariants = File.ReadAllText(Helpers.GetRootPath() + Paths.FileStandardizedVariants);

            // list to store valid emoji entries in the file
            List<string> emojiVariantsEntries = new List<string>();

            // split in lines
            string[] emojiVariantsLines = emojiVariants.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine(Strings.Program_DoRebuild_analizing_StandardizedVariants_VS_our_assets);

            // try read emoji for each line
            foreach (string line in emojiVariantsLines)
            {
                if (Regex.IsMatch(line, " FE0E; text style"))
                {
                    Match match = Regex.Match(line, "^([0-9A-F]{4,}) FE0E;");

                    if (match.Success)
                    {
                        string entry = match.Value.Replace(" FE0E;", string.Empty)
                            .ToUpperInvariant();

                        // drop 0 prefix
                        entry = Regex.Replace(entry, "^0+", string.Empty);

                        // finally, add to entries
                        emojiVariantsEntries.Add(entry);
                    }
                }
            }

            Console.WriteLine(Strings.Program_DoRebuild_INFO_parsed_X_variant_sensitive_emoji, emojiVariantsEntries.Count);
        }

       /// <summary>
       /// Rebuilds the solutions based on the local assets
       /// </summary>
       /// <param name="localAssets">The local assets.</param>
        private static void RebuildSolutions(AssetCollection localAssets)
        {
            // buildCsProj
            var stbFrwTwemojiCsproj = new StringBuilder();
            var stbFrwTwemojiAssemblyInfoCs = new StringBuilder();

            stbFrwTwemojiCsproj.AppendLine(Templates.Twemoji_csproj_start);
            stbFrwTwemojiAssemblyInfoCs.AppendLine(
                string.Format(
                Templates.Twemoji_assembly_nfo_start,
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version));

            foreach (Asset asset in localAssets)
            {
                stbFrwTwemojiCsproj.AppendLine("  <ItemGroup>");
                foreach (string emoji in asset.Emoji)
                {
                    stbFrwTwemojiCsproj.AppendFormat(
                        "    <EmbeddedResource Include=\"..\\..\\Twitter-twemoji\\{0}\\{1}.{2}\">\r\n      <Link>{3}\\{4}.{2}</Link>\r\n    </EmbeddedResource>\r\n",
                        asset.Name,
                        emoji.ToLowerInvariant(),
                        asset.Extension.ToLowerInvariant(),
                        asset.Name.ToUpperInvariant(),
                        emoji.ToUpperInvariant());

                    stbFrwTwemojiAssemblyInfoCs.AppendFormat(
                        "[assembly: WebResource(\"FrwTwemoji.{0}.{1}.{2}\", \"{3}\")]\r\n",
                        asset.Name.ToUpperInvariant(),
                        emoji.ToLowerInvariant(),
                        asset.Extension.ToLowerInvariant(),
                        asset.MimeType);
                }

                stbFrwTwemojiCsproj.AppendLine("  </ItemGroup>");
            }

            stbFrwTwemojiCsproj.AppendLine(Templates.Twemoji_csproj_end);

            File.WriteAllText(Helpers.GetRootPath() + Paths.File_FrwTwemoji_csproj, stbFrwTwemojiCsproj.ToString());
            File.WriteAllText(Helpers.GetRootPath() + Paths.File_FrwTwemoji_AssemblyInfo_cs, stbFrwTwemojiAssemblyInfoCs.ToString());
        }
    }
}

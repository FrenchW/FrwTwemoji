﻿namespace Generator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    class Program
    {
        static void Main(string[] args)
        {
            bool forceRebuild = args.Contains("-ForceRebuild");

            // Load and save assets to disk
            AssetCollection assets = new AssetCollection(new[] { "16x16", "36x36", "72x72", "svg", });

            assets.LoadLocalAssets();

            AssetCollection previousAssets = new AssetCollection(Helpers.GetRootPath() + Paths.FilePreviousAssetsBackup);

            if (forceRebuild || !assets.Equals(previousAssets))
            {
                // Assets have changed. We must rebuild each project
                LoadDistantAssets(assets);
                RebuildSolutions(assets);
                assets.Backup(Helpers.GetRootPath() + Paths.FilePreviousAssetsBackup);
            }

            Console.WriteLine(Strings.Program_Main_Press_a_key_to_end);
            Console.ReadKey();
        }


        private static void LoadDistantAssets(AssetCollection localAssets)
        {
            // Load unicode site info and compare

            AssetCollection assetsMissing = new AssetCollection(new[] { "16x16", "36x36", "72x72", "svg", });


            List<string> missingGrouped = new List<string>();


            WebClient webClient = new WebClient();

            Console.WriteLine(Strings.Program_DoRebuild_fetching_EmojiSources_txt);

            // Loading unicode reference
            webClient.DownloadFile(Paths.UrlEmojiSources, Helpers.GetRootPath() + Paths.FileEmojiSources);

            // read emojisource file
            string emojiSource = File.ReadAllText(Helpers.GetRootPath() + Paths.FileEmojiSources);

            // list to store valid emoji entries in the file
            List<string> emojiSourceEntries = new List<string>();

            // split in lines
            string[] emojiSourceLines = emojiSource.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine(Strings.Program_DoRebuild_analizing_EmojiSources_VS_our_assets);

            // try read emoji for each line
            foreach (string line in emojiSourceLines)
            {
                if (Regex.IsMatch(line, "^[0-9A-F]"))
                {
                    // set first part to unicode
                    string entry = line.Substring(0, line.IndexOf(";", StringComparison.InvariantCulture))
                        .ToUpperInvariant();

                    // replace spaces
                    entry = Regex.Replace(entry, "\\s+", "-");

                    // drop 0 prefix
                    entry = Regex.Replace(entry, "^0+", string.Empty);

                    // finally, add to entries
                    emojiSourceEntries.Add(entry);
                }
            }

            Console.WriteLine(Strings.Program_DoRebuild_INFO_parsed_0_standard_emoji, emojiSourceEntries.Count);


            string[] ignoreMissing = { "2002", "2003", "2005" };

            // now that all is loaded, perform some crossing

            foreach (var entry in emojiSourceEntries)
            {
                if (!ignoreMissing.Contains(entry))
                {
                    foreach (var asset in localAssets)
                    {
                        if (!asset.Value.Contains(entry))
                        {
                            assetsMissing[asset.Key].Add(entry);
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
                if (missingItem.Value.Count > 0)
                {
                    Console.WriteLine(Strings.Program_DoRebuild_WARNING_missing_emoji_for_assets_X_X,
                        missingItem.Key,
                        string.Join(", ", missingItem.Value));
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

        private static void RebuildSolutions(AssetCollection localAssets)
        {
            // buildCsProj
            StringBuilder sbFrwTwemojiCsproj = new StringBuilder();
            StringBuilder sbFrwTwemojiAssemblyInfoCs = new StringBuilder();

            sbFrwTwemojiCsproj.AppendLine(Templates.Twemoji_csproj_start);
            sbFrwTwemojiAssemblyInfoCs.AppendLine(
                string.Format(
                Templates.Twemoji_assembly_nfo_start,
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version));

            foreach (KeyValuePair<string, Asset> asset in localAssets)
            {
                string extension = asset.Key.Equals("svg") ? "svg" : "png";
                string mimeType = asset.Key.Equals("svg") ? "image/svg+xml" : "image/png";
                sbFrwTwemojiCsproj.AppendLine("  <ItemGroup>");
                foreach (string emoji in asset.Value)
                {
                    sbFrwTwemojiCsproj.AppendFormat(
                        "    <EmbeddedResource Include=\"..\\..\\Twitter-twemoji\\{0}\\{1}.{2}\">\r\n      <Link>{3}\\{4}.{2}</Link>\r\n    </EmbeddedResource>\r\n",
                        asset.Key,
                        emoji.ToLowerInvariant(),
                        extension.ToLowerInvariant(),
                        asset.Key.ToUpperInvariant(),
                        emoji.ToUpperInvariant());

                    sbFrwTwemojiAssemblyInfoCs.AppendFormat(
                        "[assembly: WebResource(\"FrwTwemoji.{0}.{1}.{2}\", \"{3}\")]\r\n",
                        asset.Key.ToUpperInvariant(),
                        emoji.ToLowerInvariant(),
                        extension.ToLowerInvariant(),
                        mimeType);
                }

                sbFrwTwemojiCsproj.AppendLine("  </ItemGroup>");
            }
            sbFrwTwemojiCsproj.AppendLine(Templates.Twemoji_csproj_end);

            File.WriteAllText(Helpers.GetRootPath() + Paths.File_FrwTwemoji_csproj, sbFrwTwemojiCsproj.ToString());
            File.WriteAllText(Helpers.GetRootPath() + Paths.File_FrwTwemoji_AssemblyInfo_cs, sbFrwTwemojiAssemblyInfoCs.ToString());
        }
    }
}

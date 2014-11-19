namespace Generator
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    using FrwTwemoji;

    internal class SolutionBuilder
    {
        private AssetCollection LocalAssets { get; set; }

        private string[] assetsToIgnore { get; set; }

        public SolutionBuilder(AssetCollection localAssets)
        {
            LocalAssets = localAssets;

        }

        internal void ReBuild()
        {
            LoadDistantAssets();
            RebuildSolutions();
            LocalAssets.Backup(Helpers.GetRootPath() + Paths.FilePreviousAssetsBackup);
            if (LocalAssets.Any())
            {
                BuildParser();
            }
        }

        // list to store valid emoji entries in the file
        private List<string> emojiVariantsEntries = new List<string>();
        private List<string> emojiSourceEntries = new List<string>();

        AssetCollection assetsMissing = new AssetCollection(Helpers.BaseKnownAssetNames);

        private List<string> missingGrouped = new List<string>();

        private string[] ignoreMissing = { "2002", "2003", "2005" };
        /// <summary>
        /// Loads information from unicode.org, creates assets with it and compare to the local assets
        /// </summary>
        /// <param name="localAssets">The local assets.</param>
        /// <returns>An array of string containing all emoji names to be ignored</returns>
        private void LoadDistantAssets()
        {
            //// Load unicode site info and compare

            var webClient = new WebClient();

            Console.WriteLine(Strings.Program_DoRebuild_fetching_EmojiSources_txt);

            // Loading unicode reference
            webClient.DownloadFile(Paths.UrlEmojiSources, Helpers.GetRootPath() + Paths.FileEmojiSources);

            // read emojisource file
            string emojiSource = File.ReadAllText(Helpers.GetRootPath() + Paths.FileEmojiSources);

            // split in lines
            string[] emojiSourceLines = emojiSource.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine(Strings.Program_DoRebuild_analizing_EmojiSources_VS_our_assets);

            // try read emoji for each line
            emojiSourceEntries = (
                from line
                    in emojiSourceLines
                where Regex.IsMatch(line, "^[0-9A-F]")
                select line.Substring(0, line.IndexOf(";", StringComparison.InvariantCulture)).ToUpperInvariant() into entry
                select Regex.Replace(entry, "\\s+", "-") into entry
                select Regex.Replace(entry, "^0+", string.Empty)).ToList();

            Console.WriteLine(Strings.Program_DoRebuild_INFO_parsed_0_standard_emoji, emojiSourceEntries.Count);


            //// now that all is loaded, perform some crossing

            foreach (var entry in emojiSourceEntries)
            {
                if (!ignoreMissing.Contains(entry))
                {
                    foreach (var asset in LocalAssets)
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
        private void RebuildSolutions()
        {
            // buildCsProj
            var stbFrwTwemojiCsproj = new StringBuilder();
            var stbFrwTwemojiAssemblyInfoCs = new StringBuilder();

            stbFrwTwemojiCsproj.AppendLine(Templates.Twemoji_csproj_start);
            stbFrwTwemojiAssemblyInfoCs.AppendLine(
                string.Format(
                Templates.Twemoji_assembly_nfo_start,
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version));

            foreach (Asset asset in LocalAssets)
            {
                var stbFrwTwemojiXxxCsProj = new StringBuilder();
                stbFrwTwemojiXxxCsProj.AppendLine(
                    string.Format(
                        Templates.TwemojiXXX_csproj_start,
                        asset.CompilationConstant,
                        asset.Name.Substring(0, 3)));
                stbFrwTwemojiXxxCsProj.AppendLine("  <ItemGroup>");

                stbFrwTwemojiCsproj.AppendLine("  <ItemGroup>");
                stbFrwTwemojiAssemblyInfoCs.AppendLine("#if " + asset.CompilationConstant);
                foreach (string emoji in asset.Emoji)
                {
                    stbFrwTwemojiCsproj.AppendFormat(
                        "    <EmbeddedResource Include=\"..\\..\\Twitter-twemoji\\{0}\\{1}.{2}\">\r\n      <Link>{3}\\{4}.{2}</Link>\r\n    </EmbeddedResource>\r\n",
                        asset.Name,
                        emoji.ToLowerInvariant(),
                        asset.Extension.ToLowerInvariant(),
                        asset.CompilationConstant,
                        emoji.ToUpperInvariant());

                    stbFrwTwemojiXxxCsProj.AppendFormat(
                        "    <EmbeddedResource Include=\"..\\..\\Twitter-twemoji\\{0}\\{1}.{2}\">\r\n      <Link>{3}\\{4}.{2}</Link>\r\n    </EmbeddedResource>\r\n",
                        asset.Name.ToLowerInvariant(),
                        emoji.ToLowerInvariant(),
                        asset.Extension.ToLowerInvariant(),
                        asset.CompilationConstant,
                        emoji.ToUpperInvariant());

                    stbFrwTwemojiAssemblyInfoCs.AppendFormat(
                        "[assembly: WebResource(\"FrwTwemoji.{0}.{1}.{2}\", \"{3}\")]\r\n",
                        asset.CompilationConstant,
                        emoji.ToUpperInvariant(),
                        asset.Extension.ToLowerInvariant(),
                        asset.MimeType);
                }

                stbFrwTwemojiAssemblyInfoCs.AppendLine("#endif");
                stbFrwTwemojiCsproj.AppendLine("  </ItemGroup>");

                stbFrwTwemojiXxxCsProj.AppendLine("  </ItemGroup>");
                stbFrwTwemojiXxxCsProj.AppendLine(Templates.Twemoji_csproj_end);
                File.WriteAllText(
                    Helpers.GetRootPath() + string.Format(
                        Paths.File_FrwTwemojiXXX_csproj,
                        asset.Name.Substring(0, 3)),
                    stbFrwTwemojiXxxCsProj.ToString());
            }

            stbFrwTwemojiCsproj.AppendLine(Templates.Twemoji_csproj_end);

            File.WriteAllText(Helpers.GetRootPath() + Paths.File_FrwTwemoji_csproj, stbFrwTwemojiCsproj.ToString());
            File.WriteAllText(Helpers.GetRootPath() + Paths.File_FrwTwemoji_AssemblyInfo_cs, stbFrwTwemojiAssemblyInfoCs.ToString());
        }

        private void BuildParser()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Templates.Twemoji_RegEx_cs_start);

            List<string> sensitive = new List<string>();
            List<string> regular = new List<string>();
            // if we enter BuildParser, LocalAssts has at least one asset

            foreach (string emoji in LocalAssets[0].Emoji)
            {
                if (!ignoreMissing.Contains(emoji))
                {
                    //// * @example
                    //// *  twemoji.convert.fromCodePoint('1f1e8');
                    //// *  // "\ud83c\udde8"
                    //// *
                    //// *  '1f1e8-1f1f3'.split('-').map(twemoji.convert.fromCodePoint).join('')
                    //// *  // "\ud83c\udde8\ud83c\uddf3"
                    string[] u = emoji.Split('-');
                    string stringToAdd = Helpers.ShowU(Helpers.ConvertCodePointToUtf16(int.Parse(u[0], NumberStyles.AllowHexSpecifier)));
                    if (u.GetUpperBound(0) == 0)
                    {       
                            regular.Add(stringToAdd);
                        
                    }
                    else
                    {
                        stringToAdd += Helpers.ShowU(Helpers.ConvertCodePointToUtf16(int.Parse(u[1], NumberStyles.AllowHexSpecifier)));
                        sensitive.Add(stringToAdd);
                    }
                }
            }

            string regExp = "((?:"
                + string.Join("|", regular.ToArray())
                + ")|(?:?:"
                + string.Join("|", sensitive.ToArray())
                + ")([\\uFE0E\\uFE0F]?)))";

            regExp = "(?:"
                + string.Join("|", regular.ToArray())
                + ")";


            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// The emoji search pattern");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        internal static string EmojiSearchPattern = \""
                + regExp + "\";");

            sb.AppendLine(Templates.Twemoji_RegEx_cs_end);
            File.WriteAllText(
                Helpers.GetRootPath() + Paths.File_FrwTwemoji_RegEx_cs,
                sb.ToString());

        }

    }
}

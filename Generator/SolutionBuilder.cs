namespace Generator
{
    using FrwTwemoji;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class SolutionBuilder
    {
        private AssetCollection LocalAssets { get; set; }

        private string[] assetsToIgnore { get; set; }

        public SolutionBuilder(AssetCollection localAssets)
        {
            this.LocalAssets = localAssets;
        }

        internal void ReBuild()
        {
            this.LoadDistantAssets();
            this.RebuildSolutions();
            this.LocalAssets.Backup(Helpers.GetRootPath() + Paths.FilePreviousAssetsBackup);
            if (this.LocalAssets.Any())
            {
                this.BuildParser();
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

            try
            {
                // Loading unicode reference
                webClient.DownloadFile(Paths.UrlEmojiSources, Helpers.GetRootPath() + Paths.FileEmojiSources);

                // Keep a backup of the downloaded file
                if (File.Exists(Helpers.GetRootPath() + Paths.FileEmojiSources))
                {
                    File.Delete(Helpers.GetRootPath() + "Local-" + Paths.FileEmojiSources);
                    File.Copy(
                        Helpers.GetRootPath() + Paths.FileEmojiSources,
                        Helpers.GetRootPath() + "Local-" + Paths.FileEmojiSources);
                }

            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
                // read local file from project
                if (!File.Exists(Helpers.GetRootPath() + Paths.FileEmojiSources))
                {
                    File.Copy(
                        Helpers.GetRootPath() + "Local-" + Paths.FileEmojiSources,
                        Helpers.GetRootPath() + Paths.FileEmojiSources);
                }
            }

            try
            {
                // Loading unicode reference
                webClient.DownloadFile(Paths.UrlStandardizedVariants, Helpers.GetRootPath() + Paths.FileStandardizedVariants);

                // Keep a backup of the downloaded file
                if (File.Exists(Helpers.GetRootPath() + Paths.FileStandardizedVariants))
                {
                    File.Delete(Helpers.GetRootPath() + "Local-" + Paths.FileStandardizedVariants);
                    File.Copy(
                        Helpers.GetRootPath() + Paths.FileStandardizedVariants,
                        Helpers.GetRootPath() + "Local-" + Paths.FileStandardizedVariants);
                }
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                // read local file from project
                if (!File.Exists(Helpers.GetRootPath() + Paths.FileStandardizedVariants))
                {
                    File.Copy(
                        Helpers.GetRootPath() + "Local-" + Paths.FileStandardizedVariants,
                        Helpers.GetRootPath() + Paths.FileStandardizedVariants);
                }
            }

            // read emojisource file
            string emojiSource = File.ReadAllText(Helpers.GetRootPath() + Paths.FileEmojiSources);

            // split in lines
            string[] emojiSourceLines = emojiSource.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine(Strings.Program_DoRebuild_analizing_EmojiSources_VS_our_assets);

            // try read emoji for each line
            this.emojiSourceEntries = (
                from line
                    in emojiSourceLines
                where Regex.IsMatch(line, "^[0-9A-F]")
                select line.Substring(0, line.IndexOf(";", StringComparison.InvariantCulture)).ToUpperInvariant() into entry
                select Regex.Replace(entry, "\\s+", "-") into entry
                select Regex.Replace(entry, "^0+", string.Empty)).ToList();

            Console.WriteLine(Strings.Program_DoRebuild_INFO_parsed_0_standard_emoji, this.emojiSourceEntries.Count);


            //// now that all is loaded, perform some crossing

            foreach (var entry in this.emojiSourceEntries)
            {
                if (!this.ignoreMissing.Contains(entry))
                {
                    foreach (var asset in this.LocalAssets)
                    {
                        if (!asset.Emoji.Contains(entry))
                        {
                            if (this.assetsMissing[asset.Name] != null)
                            {
                                this.assetsMissing[asset.Name].Emoji.Add(entry);
                            }

                            if (!this.missingGrouped.Contains(entry))
                            {
                                this.missingGrouped.Add(entry);
                            }
                        }
                    }
                }
            }

            foreach (var missingItem in this.assetsMissing)
            {
                if (missingItem.Emoji.Count > 0)
                {
                    Console.WriteLine(
                        Strings.Program_DoRebuild_WARNING_missing_emoji_for_assets_X_X,
                        missingItem.Name,
                        string.Join(", ", missingItem.Emoji));
                }
            }

            this.ignoreMissing = this.ignoreMissing.Concat(this.missingGrouped).ToArray();

            Console.WriteLine(Strings.Program_DoRebuild_fetching_StandardizedVariants_txt);


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
                        this.emojiVariantsEntries.Add(entry);
                    }
                }
            }

            Console.WriteLine(Strings.Program_DoRebuild_INFO_parsed_X_variant_sensitive_emoji, this.emojiVariantsEntries.Count);
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

            // Add twemoji.js and twemoji.min.js to the csproj
            stbFrwTwemojiCsproj.AppendLine("  <ItemGroup>");
            stbFrwTwemojiCsproj.Append(
                "    <EmbeddedResource Include=\"..\\..\\Twitter-twemoji\\2\\twemoji.min.js\">\r\n      <Link>Js\\twemoji.min.js</Link>\r\n    </EmbeddedResource>\r\n");
            stbFrwTwemojiCsproj.Append(
                "    <EmbeddedResource Include=\"..\\..\\Twitter-twemoji\\2\\twemoji.js\">\r\n      <Link>Js\\twemoji.js</Link>\r\n    </EmbeddedResource>\r\n");
            stbFrwTwemojiAssemblyInfoCs.Append(
                "[assembly: WebResource(\"FrwTwemoji.Js.twemoji.js\", \"application/javascript\")]\r\n");
            stbFrwTwemojiAssemblyInfoCs.Append(
                "[assembly: WebResource(\"FrwTwemoji.Js.twemoji.min.js\", \"application/javascript\")]\r\n");
            stbFrwTwemojiCsproj.AppendLine("  </ItemGroup>");

            foreach (Asset asset in this.LocalAssets)
            {
                var stbFrwTwemojiXxxCsProj = new StringBuilder();
                stbFrwTwemojiXxxCsProj.AppendLine(
                    string.Format(
                        Templates.TwemojiXXX_csproj_start,
                        asset.CompilationConstant,
                        asset.Name.Substring(0, 3)));
                stbFrwTwemojiXxxCsProj.AppendLine("  <ItemGroup>");
                stbFrwTwemojiXxxCsProj.Append(
                    "    <EmbeddedResource Include=\"..\\..\\Twitter-twemoji\\2\\twemoji.min.js\">\r\n      <Link>Js\\twemoji.min.js</Link>\r\n    </EmbeddedResource>\r\n");
                stbFrwTwemojiXxxCsProj.Append(
                    "    <EmbeddedResource Include=\"..\\..\\Twitter-twemoji\\2\\twemoji.js\">\r\n      <Link>Js\\twemoji.js</Link>\r\n    </EmbeddedResource>\r\n");
                stbFrwTwemojiXxxCsProj.AppendLine("  </ItemGroup>");

                stbFrwTwemojiXxxCsProj.AppendLine("  <ItemGroup>");
                stbFrwTwemojiCsproj.AppendLine("  <ItemGroup>");
                stbFrwTwemojiAssemblyInfoCs.AppendLine("#if " + asset.CompilationConstant);
                foreach (string emoji in asset.Emoji)
                {
                    stbFrwTwemojiCsproj.AppendFormat(
                        "    <EmbeddedResource Include=\"..\\..\\Twitter-twemoji\\2\\{0}\\{1}.{2}\">\r\n      <Link>{3}\\{4}.{2}</Link>\r\n    </EmbeddedResource>\r\n",
                        asset.Name,
                        emoji.ToLowerInvariant(),
                        asset.Extension.ToLowerInvariant(),
                        asset.CompilationConstant,
                        emoji.ToUpperInvariant());

                    stbFrwTwemojiXxxCsProj.AppendFormat(
                        "    <EmbeddedResource Include=\"..\\..\\Twitter-twemoji\\2\\{0}\\{1}.{2}\">\r\n      <Link>{3}\\{4}.{2}</Link>\r\n    </EmbeddedResource>\r\n",
                        asset.Name.ToLowerInvariant(),
                        emoji.ToLowerInvariant(),
                        asset.Extension.ToLowerInvariant(),
                        asset.CompilationConstant,
                        emoji.ToUpperInvariant());

                    stbFrwTwemojiAssemblyInfoCs.AppendFormat(
                        "[assembly: WebResource(\"{0}\", \"{1}\")]\r\n",
                        Helpers.GetEmojiAssemblyName(emoji, asset.Pack),
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

            stbFrwTwemojiCsproj.Append(Templates.Twemoji_csproj_end);

            File.WriteAllText(Helpers.GetRootPath() + Paths.File_FrwTwemoji_csproj, stbFrwTwemojiCsproj.ToString());
            File.WriteAllText(Helpers.GetRootPath() + Paths.File_FrwTwemoji_AssemblyInfo_cs, stbFrwTwemojiAssemblyInfoCs.ToString());
        }

        private void BuildParser()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Templates.Twemoji_RegEx_cs_start);
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// The emoji search pattern");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <remarks>WARINING : This file is generated by Generator.SolutionBuilder.BuildParser()");
            sb.AppendLine("        /// INFO : Get it from twemoji.js (edit begining and end, uppercase, and replace \\U by \\\\u</remarks>");
            sb.AppendLine("        internal static string EmojiSearchPattern = \""+ Templates.Twemoji_RegEx + "\";");

            sb.AppendLine(Templates.Twemoji_RegEx_cs_end);
            File.WriteAllText(
                Helpers.GetRootPath() + Paths.File_FrwTwemoji_RegEx_cs,
                sb.ToString());
            return;

            List<string> sensitive = new List<string>();
            List<string> regular = new List<string>();
            // if we enter BuildParser, LocalAssts has at least one asset

            foreach (string emoji in this.LocalAssets[0].Emoji)
            {
                if (!this.ignoreMissing.Contains(emoji))
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
        }

    }
}

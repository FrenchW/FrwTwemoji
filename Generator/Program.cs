namespace Generator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Text.RegularExpressions;

    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = Assembly.GetExecutingAssembly().Location;

#if DEBUG
            {
                rootPath = rootPath.Replace("Generator\\bin\\Debug\\Generator.exe", string.Empty);
            }
#else
            {
                rootPath = rootPath.Replace("Generator\\bin\\Release\\Generator.exe", string.Empty);
            }
#endif
            test(rootPath);
            Console.WriteLine("Press a key to end");
            Console.ReadLine();
        }

        private static void test(string rootPath)
        {
            // Creating local Assets arrays
            Dictionary<string, List<string>> assets = new Dictionary<string, List<string>>
                                                          {
                                                              {
                                                                  "16x16",
                                                                  new List<string>()
                                                              },
                                                              {
                                                                  "36x36",
                                                                  new List<string>()
                                                              },
                                                              {
                                                                  "72x72",
                                                                  new List<string>()
                                                              },
                                                              {
                                                                  "svg",
                                                                  new List<string>()
                                                              }
                                                          };

            Dictionary<string, List<string>> missing = new Dictionary<string, List<string>>
                                                          {
                                                              {
                                                                  "16x16",
                                                                  new List<string>()
                                                              },
                                                              {
                                                                  "36x36",
                                                                  new List<string>()
                                                              },
                                                              {
                                                                  "72x72",
                                                                  new List<string>()
                                                              },
                                                              {
                                                                  "svg",
                                                                  new List<string>()
                                                              }
                                                          };

            List<string> missingGrouped = new List<string>();

            string[] ignoreMissing = { "2002", "2003", "2005" };

            Console.WriteLine("Analyzing all assets ...");

            // foreach asset type
            foreach (var asset in assets)
            {
                // get folder path
                string assetFolder = rootPath + "Twitter-twemoji\\" + asset.Key + "\\";

                // foreach file in path
                foreach (var file in Directory.GetFiles(assetFolder))
                {
                    // remove path from filename
                    string filename = file.Replace(assetFolder, string.Empty).ToUpperInvariant();

                    // remove extension and set to uppercase
                    filename = filename.Substring(0, filename.LastIndexOf(".", StringComparison.InvariantCulture));

                    // finally, populate array
                    asset.Value.Add(filename);
                }

                Console.WriteLine("[INFO] asset {0} contains {1} emoji", asset.Key, asset.Value.Count);
            }

            string rootEmojiUrl = "http://www.unicode.org/Public/UNIDATA/";
            WebClient webClient = new WebClient();

            Console.WriteLine("fetching EmojiSources.txt ... ");

            // Loading unicode reference
            webClient.DownloadFile(rootEmojiUrl + "EmojiSources.txt", rootPath + "EmojiSources.txt");

            // read emojisource file
            string emojiSource = File.ReadAllText(rootPath + "EmojiSources.txt");

            // list to store valid emoji entries in the file
            List<string> emojiSourceEntries = new List<string>();

            // split in lines
            string[] emojiSourceLines = emojiSource.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine("analizing EmojiSources VS our assets ... ");

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

            Console.WriteLine("[INFO] parsed {0} standard emoji.", emojiSourceEntries.Count);

            // now that all is loaded, perform some crossing

            foreach (var entry in emojiSourceEntries)
            {
                if (!ignoreMissing.Contains(entry))
                {
                    foreach (var asset in assets)
                    {
                        if (!asset.Value.Contains(entry))
                        {
                            missing[asset.Key].Add(entry);
                            if (!missingGrouped.Contains(entry))
                            {
                                missingGrouped.Add(entry);
                            }
                        }
                    }
                }
            }

            foreach (var missingItem in missing)
            {
                if (missingItem.Value.Count > 0)
                {
                    Console.WriteLine("[WARNING] missing emoji for assets {0}:{1}",
                        missingItem.Key,
                        string.Join(", ", missingItem.Value));
                }
            }

            ignoreMissing = ignoreMissing.Concat(missingGrouped).ToArray();

            Console.WriteLine("fetching StandardizedVariants.txt ... ");

            // Loading unicode reference
            webClient.DownloadFile(rootEmojiUrl + "StandardizedVariants.txt", rootPath + "StandardizedVariants.txt");

            // read emojiVariants file
            string emojiVariants = File.ReadAllText(rootPath + "StandardizedVariants.txt");

            // list to store valid emoji entries in the file
            List<string> emojiVariantsEntries = new List<string>();

            // split in lines
            string[] emojiVariantsLines = emojiVariants.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine("analizing StandardizedVariants VS our assets ... ");

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

            Console.WriteLine("[INFO] parsed {0} variant sensitive emoji.", emojiVariantsEntries.Count);

            












        }


    }
}

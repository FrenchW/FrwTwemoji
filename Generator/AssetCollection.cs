using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generator
{
    using System.Collections.Specialized;
    using System.IO;

    using Newtonsoft.Json;

    class AssetCollection : Dictionary<string, Asset>, IEquatable<AssetCollection>
    {
        public AssetCollection()
        {
        }

        public AssetCollection(string[] assetNames)
        {
            for (int i = 0; i <= assetNames.GetUpperBound(0); i++)
            {
                this.Add(assetNames[i], new Asset());
            }
        }

        public AssetCollection(string backupFilename)
        {
            AssetCollection retval;
            if (!File.Exists(backupFilename))
            {
                return;
            }

            try
            {
                retval = JsonConvert.DeserializeObject<AssetCollection>(
                    File.ReadAllText(backupFilename));
            }
            catch (Exception)
            {
                return;
            }

            foreach (KeyValuePair<string, Asset> keyValuePair in retval)
            {
                this.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public void LoadLocalAssets()
        {
            Console.WriteLine(Strings.AssetCollection_AnalizeAllAsset_Analyzing_all_assets);

            // foreach asset type
            foreach (var asset in this)
            {
                // get folder path
                string assetFolder = Helpers.GetRootPath() + Paths.FolderTwitterTwemoji + asset.Key + "\\";

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

                Console.WriteLine(Strings.AssetCollection_AnalizeAllAsset_INFO_asset_X_contains_X_emoji, asset.Key, asset.Value.Count);
            }
        }

        public void Backup(string filePath)
        {
            var json = JsonConvert.SerializeObject(this);
            File.WriteAllText(filePath, json);
        }

        public bool Equals(AssetCollection other)
        {
            if (this.Count != other.Count)
            {
                return false;
            }

            if (!this.OrderBy(pair => pair.Key).SequenceEqual(other.OrderByDescending(pair => pair.Key)))
            {
                return false;
            }

            foreach (KeyValuePair<string, Asset> pair in this)
            {
                if (pair.Value.Count != other[pair.Key].Count)
                {
                    return false;
                }

                pair.Value.Sort();
                other[pair.Key].Sort();

                if (!pair.Value.SequenceEqual(other[pair.Key]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

namespace Generator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    /// <summary>Collection of Assets
    /// </summary>
    public class AssetCollection : List<Asset>, IEquatable<AssetCollection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetCollection"/> class.
        /// </summary>
        public AssetCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetCollection" /> class.
        /// </summary>
        /// <param name="assetNames">Name of each asset to create when initializing.</param>
        public AssetCollection(string[] assetNames)
        {
            for (int i = 0; i <= assetNames.GetUpperBound(0); i++)
            {
                this.Add(new Asset(assetNames[i]));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetCollection"/> class.
        /// </summary>
        /// <param name="backupFilePath">The path of the backup file to use as data intializer.</param>
        public AssetCollection(string backupFilePath)
        {
            if (!File.Exists(backupFilePath))
            {
                return;
            }

            try
            {
                TextReader reader = new StreamReader(backupFilePath);
                try
                {
                    var obj = serializer.Deserialize(reader);

                    if (obj.GetType().IsAssignableFrom(typeof(AssetCollection)))
                    {
                        this.AddRange((AssetCollection)obj);
                    }

                    reader.Close();
                    reader.Dispose();
                }
                catch (Exception)
                {
                    // Wrong file, deleting it
                    reader.Close();
                    reader.Dispose();
                    File.Delete(backupFilePath);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private XmlSerializer serializer
        {
            get
            {
                XmlSerializer retval = new XmlSerializer(typeof(AssetCollection));
                return retval;
            }
        }

        public Asset this[string Name]
        {
            get
            {
                return (from a in this where a.Name.Equals("Name") select a).FirstOrDefault();
            }
        }

        public void LoadLocalAssets()
        {
            Console.WriteLine(Strings.AssetCollection_AnalizeAllAsset_Analyzing_all_assets);

            // foreach asset type
            foreach (var asset in this)
            {
                // get folder path
                string assetFolder = Helpers.GetRootPath() + Paths.FolderTwitterTwemoji + asset.Name + "\\";

                // foreach file in path
                foreach (var file in Directory.GetFiles(assetFolder))
                {
                    // remove path from filename
                    string filename = file.Replace(assetFolder, string.Empty).ToUpperInvariant();

                    // remove extension and set to uppercase
                    filename = filename.Substring(0, filename.LastIndexOf(".", StringComparison.InvariantCulture));

                    // finally, populate array
                    asset.Emoji.Add(filename);
                }

                Console.WriteLine(Strings.AssetCollection_AnalizeAllAsset_INFO_asset_X_contains_X_emoji, asset.Name, asset.Emoji.Count);
            }
        }

        public void Backup(string filePath)
        {
            TextWriter writer = new StringWriter();
            serializer.Serialize(writer, this);
            File.WriteAllText(filePath, writer.ToString());
        }

        /// <summary>
        /// Indique si l'objet actuel est égal à un autre objet du même type.
        /// </summary>
        /// <param name="other">Objet à comparer avec cet objet.</param>
        /// <returns>
        /// true si l'objet en cours est égal au paramètre <paramref name="other" /> ; sinon, false.
        /// </returns>
        public bool Equals(AssetCollection other)
        {
            if (this.Count != other.Count)
            {
                return false;
            }

            var thisSorted = this.OrderBy(a => a.Name);
            var otherSorted = this.OrderBy(a => a.Name);

            if (!thisSorted.SequenceEqual(otherSorted))
            {
                return false;
            }

            foreach (Asset asset in thisSorted)
            {
                Asset otherAsset = (from a in otherSorted where a.Name.Equals(asset.Name) select a).First();
                if (otherAsset == default(Asset))
                {
                    return false;
                }

                if (asset.Emoji.Count != otherAsset.Emoji.Count)
                {
                    return false;
                }

                asset.Emoji.Sort();
                otherAsset.Emoji.Sort();

                for (int i = 0; i < asset.Emoji.Count; i++)
                {
                    if (asset.Emoji[i] != otherAsset.Emoji[i])
                    {

                        return false;
                    }
                }
            }

            return true;
        }
    }
}

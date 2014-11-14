namespace Generator
{
    using System.Collections.Generic;

    /// <summary>
    /// List of the emoji names 
    /// </summary>
    public class Asset
    {
        public Asset()
        {
            this.Emoji = new List<string>();
        }

        public Asset(string name)
        {
            this.Name = name;
            this.Emoji = new List<string>();
        }

        public Asset(string name, List<string> emoji)
        {
            this.Name = name;
            this.Emoji = emoji;
        }
        
        public string Name{ get; set; }

        public List<string> Emoji { get; set; }
        public string Extension { get {return this.Name.Equals("svg") ? "svg" : "png";} }

        public string MimeType { get {return this.Name.Equals("svg") ? "image/svg+xml" : "image/png"; } }
    }
}

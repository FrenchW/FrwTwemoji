// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Asset.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014.
//   This software is licenced like https://github.com/twitter/twemoji :
//   Code licensed under the MIT License: http://opensource.org/licenses/MIT
//   Graphics licensed under CC-BY 4.0: https://creativecommons.org/licenses/by/4.0/ and created by Twitter
// </copyright>
// <summary>
//   List of the emoji names
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Generator
{
    using System.Collections.Generic;

    /// <summary>
    /// List of the emoji names 
    /// </summary>
    public class Asset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Asset"/> class.
        /// </summary>
        public Asset()
        {
            this.Emoji = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Asset"/> class and name if.
        /// </summary>
        /// <param name="name">The name to set for the asset</param>
        public Asset(string name)
        {
            this.Name = name;
            this.Emoji = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Asset"/> class.
        /// </summary>
        /// <param name="name">The name to set for the asset</param>
        /// <param name="emoji">The list of emoji to set for initialization</param>
        public Asset(string name, List<string> emoji)
        {
            this.Name = name;
            this.Emoji = emoji;
        }

        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the emoji list.
        /// </summary>
        /// <value>
        /// The emoji.
        /// </value>
        public List<string> Emoji { get; set; }

        /// <summary>
        /// Gets the default file extension from the asset name
        /// </summary>
        /// <value>
        /// The file extension.
        /// </value>
        public string Extension
        {
            get
            {
                return this.Name.Equals("Svg") ? "svg" : "png";
            }
        }

        /// <summary>
        /// Gets the default file extension from the asset name
        /// </summary>
        /// <value>
        /// The file extension.
        /// </value>
        public string CompilationConstant
        {
            get
            {
                return "Icons" + this.Name.Substring(0, 3);
            }
        }

        /// <summary>
        /// Gets the default mime type from the asset files.
        /// </summary>
        /// <value>
        /// The mime type.
        /// </value>
        public string MimeType
        {
            get
            {
                return this.Name.Equals("svg") ? "image/svg+xml" : "image/png";
            }
        }
    }
}

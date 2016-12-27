// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Asset.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014-2016.
//   FrwTwemoji Project page : http://github.frenchw.net/FrwTwemoji/
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

    using FrwTwemoji;

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
        /// Initializes a new instance of the <see cref="Asset" /> class and name if.
        /// </summary>
        /// <param name="assetPack">The asset pack.</param>
        public Asset(Helpers.AssetPackFromTwemoji assetPack)
        {
            this.Pack = assetPack;
            this.Emoji = new List<string>();
        }

        /// <summary>Gets the pack name.</summary>
        public string Name
        {
            get
            {
                return Helpers.GetAssetPackFolderName(this.Pack);
            }
        }
        /// <summary>
        /// Base Pack
        /// </summary>
        /// <value>
        /// The pack.
        /// </value>
        public Helpers.AssetPackFromTwemoji Pack { get; set; }

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
                return Helpers.GetAssetPackImageExtension(this.Pack);
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
                return Helpers.GetAssetPackCompilationConstant(this.Pack);
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
                return Helpers.GetAssetPackMimeType(this.Pack);
            }
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParserOptions.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014.
//   This software is licenced like https://github.com/twitter/twemoji :
//   Code licensed under the MIT License: http://opensource.org/licenses/MIT
//   Graphics licensed under CC-BY 4.0: https://creativecommons.org/licenses/by/4.0/ and created by Twitter
// </copyright>
// <summary>
//   Options that tell parser how to behave
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FrwTwemoji
{
    /// <summary>
    /// Options that tell parser how to behave
    /// </summary>
    public class ParserOptions
    {
        /// <summary>
        /// The size to render the icons
        /// </summary>
        public enum AssetSizes
        {
            /// <summary>
            /// Render in 16 pixels
            /// </summary>
            Render16Px,

            /// <summary>
            /// Render in 36 pixels
            /// </summary>
            Render36Px,

            /// <summary>
            /// Render in 72 pixels
            /// </summary>
            Render72Px,
        }

        /// <summary>
        /// Emoji provider: local or a CDN
        /// </summary>
        public enum RessourcesProviders
        {
            /// <summary>
            /// Local Resource are used
            /// </summary>
            Localhost,

            /// <summary>
            /// CDN : MaxCDN
            /// </summary>
            MaxCdn
        }

        /// <summary>
        /// Gets or sets the image resolution
        /// </summary>
        /// <value>
        /// The size of the asset.
        /// </value>
        public AssetSizes AssetSize { get; set; }

        /// <summary>
        /// Gets or sets the ressource provider.
        /// </summary>
        /// <value>
        /// The ressources provider.
        /// </value>
        public RessourcesProviders RessourcesProvider { get; set; }
    }
}

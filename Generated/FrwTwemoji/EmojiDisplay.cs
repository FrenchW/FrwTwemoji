// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmojiDisplay.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014.
//   This software is licenced like https://github.com/twitter/twemoji :
//   Code licensed under the MIT License: http://opensource.org/licenses/MIT
//   Graphics licensed under CC-BY 4.0: https://creativecommons.org/licenses/by/4.0/ and created by Twitter
// </copyright>
// <summary>
//   The main control to display text containing emoji
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FrwTwemoji
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Security.Permissions;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using FrwTwemoji.Designers;

    /// <summary>
    /// The main control to display text containing emoji
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:EmojiDisplay \r\n"
                 + "Text=\"Today, Twitter is open sourcing their emoji to share with everyone  🎉 😜 👯 🍻 🎈 🎤 🎮 🚀 🌉 ✨\"\r\n"
                 + "AssetType=\"Png\" "
                 + "AssetSize=\"Render36Px\" " 
                 + "runat=server></{0}:EmojiDisplay>")]
    [Designer(typeof(EmojiDisplayDesigner))]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]

    public class EmojiDisplay : WebControl
    {

        private string internRenderedText = string.Empty;

        private string internFileExtension = "png";

        private string internFileSize = "Icons16x";

        /// <summary>
        /// Gets or sets the image resolution
        /// </summary>
        /// <value>
        /// The size of the asset.
        /// </value>
        public AssetSizes AssetSize
        {
            get
            {
                object obj =
                   this.ViewState["AssetSize"];
                if (obj == null)
                {
                    return AssetSizes.Render16Px;
                }

                return (AssetSizes)obj;
            }
            set
            {
                this.ViewState["AssetSize"] = value;
                this.DataChanged();
            }
        }
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

            /// <summary>
            /// Render in 128 pixels
            /// </summary>
            Render128Px,

            /// <summary>
            /// Render in 256 pixels
            /// </summary>
            Render256Px,

            /// <summary>
            /// Render in 512 pixels
            /// </summary>
            Render512Px,
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
        /// Emoji provider: local or a CDN
        /// </summary>
        public enum AssetTypes
        {
            /// <summary>
            /// Assets in png format
            /// </summary>
            Png,

            /// <summary>
            /// Assets in Svg format
            /// </summary>
            Svg
        }
        /// <summary>
        /// Gets or sets the image type
        /// </summary>
        /// <value>
        /// The type of the asset.
        /// </value>
        public AssetTypes AssetType
        {
            get
            {
                object obj =
                   this.ViewState["AssetType"];
                if (obj == null)
                {
                    return AssetTypes.Png;
                }

                return (AssetTypes)obj;
            }
            set
            {
                this.ViewState["AssetType"] = value;
                this.DataChanged();
            }
        }

        public string Text
        {
            get
            {
                object obj =
                   this.ViewState["Text"];
                if (obj == null)
                {
                    return string.Empty;
                }

                return (string)obj;
            }
            set
            {
                this.ViewState["Text"] = value.Trim();
                this.DataChanged();
            }
        }

        /// <summary>
        /// Gets or sets the ressource provider.
        /// </summary>
        /// <value>
        /// The ressources provider.
        /// </value>
        public RessourcesProviders RessourcesProvider
        {
            get
            {
                object obj =
                   this.ViewState["RessourcesProvider"];
                if (obj == null)
                {
                    return RessourcesProviders.Localhost;
                }

                return (RessourcesProviders)obj;
            }
            set
            {
                this.ViewState["RessourcesProvider"] = value;
                this.DataChanged();
            }
        }

        private bool CheckConfiguration(out string validationMessage)
        {
            List<string> errors = new List<string>();
            if (this.AssetType == AssetTypes.Png)
            {
                internFileExtension = "png";
                if (this.AssetSize == AssetSizes.Render128Px || this.AssetSize == AssetSizes.Render256Px)
                {
                    errors.Add(string.Format("There is no png ressource for {0} resolution", this.AssetSize.ToString()));
                }

                switch (this.AssetSize)
                {
                    case AssetSizes.Render16Px:
                        this.internFileSize = "Icons16x";
                        break;
                    case AssetSizes.Render36Px:
                        this.internFileSize = "Icons36x";
                        break;
                    case AssetSizes.Render72Px:
                        this.internFileSize = "Icons72x";
                        break;
                    default:
                        errors.Add(
                            string.Format("Asset type {0} has no resource at {0} resolution", this.AssetType.ToString()));
                        break;
                }
            }
            else if (this.AssetType == AssetTypes.Svg)
            {
                internFileExtension = "svg";

                switch (this.AssetSize)
                {
                    case AssetSizes.Render16Px:
                    case AssetSizes.Render36Px:
                    case AssetSizes.Render72Px:
                    case AssetSizes.Render128Px:
                    case AssetSizes.Render256Px:
                    default:
                        this.internFileSize = "IconsSvg";
                        break;
                }
            }
            else
            {
                errors.Add(
                    string.Format("Asset type {0} is not implemented in GetWebResourceName", this.AssetType.ToString()));
            }

            validationMessage = string.Empty;
            bool retval = true;

            if (errors.Count > 0)
            {
                retval = false;
                validationMessage = "<ul><li>" + string.Join("</li><li>", errors.ToArray()) + "</li></ul>";
            }

            return retval;
        }

        private void DataChanged()
        {
            string message;
            if (!this.CheckConfiguration(out message))
            {
                this.internRenderedText = message;
            }
        }

        protected internal string GetWebResourceName(string emoji)
        {

            return string.Format("FrwTwemoji.{0}.{1}.{2}", this.internFileSize, emoji, this.internFileExtension);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            var cs = this.Page.ClientScript;
            this.internRenderedText = this.WebParseEmoji();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (writer == null)
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            // ReSharper disable HeuristicUnreachableCode
            {
                // pas d'exception ici, la méthode étant implicitement appelée par le framework
                return;
            }

            // ReSharper restore HeuristicUnreachableCode
            const string ContainerTag = "span";

            writer.WriteBeginTag(ContainerTag);
            writer.WriteAttribute("id", this.ClientID);
            writer.WriteAttribute(
                "class",
                !string.IsNullOrEmpty(this.CssClass) ? this.CssClass : this.GetType().Name);

            writer.Write(HtmlTextWriter.TagRightChar);

            this.RenderContents(writer);

            writer.WriteEndTag(ContainerTag);

        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
           this.RenderContentsInternal(writer);
        }

        /// <summary>
        /// Parses the string and replaces emoji emoji.
        /// </summary>
        /// <param name="fromString">From string.</param>
        /// <returns>
        /// a string that contains the originale string with emoji replaced by image
        /// </returns>
        private string WebParseEmoji()
        {
            return Regex.Replace(this.Text, RegEx.EmojiSearchPattern, this.WebParseEmojiRegExMatchEvaluator);
        }

        private string WebParseEmojiRegExMatchEvaluator(Match match)
        {
            int codepoint = Helpers.ConvertUtf16ToCodePoint(match.Value);
            string emoji = string.Format("{0:x}", codepoint).ToUpperInvariant();

            // FrwTwemoji.Icons16x.1F170.png
            string resourceName = this.GetWebResourceName(emoji);
           
            //this.Page.ClientScript.RegisterClientScriptResource(this.GetType(), resourceName);
            string url = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), resourceName);
            return string.Format("<img src=\"{0}\"/>", url);
        }

        protected internal void RenderContentsInternal(HtmlTextWriter writer)
        {
            // check configuration
            string errorMEssage;
            if (!this.CheckConfiguration(out errorMEssage))
            {
                writer.Write("EmojiDisplay control is misconfigured : {0}", errorMEssage);
                return;
            }

            writer.Write(this.internRenderedText);
        }
    }
}

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
        public Helpers.AssetSizes AssetSize
        {
            get
            {
                object obj =
                   this.ViewState["AssetSize"];
                if (obj == null)
                {
                    return Helpers.AssetSizes.Render16Px;
                }

                return (Helpers.AssetSizes)obj;
            }
            set
            {
                this.ViewState["AssetSize"] = value;
                this.DataChanged();
            }
        }

        /// <summary>
        /// Gets or sets the image type
        /// </summary>
        /// <value>
        /// The type of the asset.
        /// </value>
        public Helpers.AssetTypes AssetType
        {
            get
            {
                object obj =
                   this.ViewState["AssetType"];
                if (obj == null)
                {
                    return Helpers.AssetTypes.Png;
                }

                return (Helpers.AssetTypes)obj;
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
        public Helpers.RessourcesProviders RessourcesProvider
        {
            get
            {
                object obj =
                   this.ViewState["RessourcesProvider"];
                if (obj == null)
                {
                    return Helpers.RessourcesProviders.Localhost;
                }

                return (Helpers.RessourcesProviders)obj;
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
            if (this.AssetType == Helpers.AssetTypes.Png)
            {
                internFileExtension = "png";
                if (this.AssetSize == Helpers.AssetSizes.Render128Px || this.AssetSize == Helpers.AssetSizes.Render256Px)
                {
                    errors.Add(string.Format("There is no png ressource for {0} resolution", this.AssetSize.ToString()));
                }

                switch (this.AssetSize)
                {
                    case Helpers.AssetSizes.Render16Px:
                        this.internFileSize = "Icons16x";
                        break;
                    case Helpers.AssetSizes.Render36Px:
                        this.internFileSize = "Icons36x";
                        break;
                    case Helpers.AssetSizes.Render72Px:
                        this.internFileSize = "Icons72x";
                        break;
                    default:
                        errors.Add(
                            string.Format("Asset type {0} has no resource at {0} resolution", this.AssetType.ToString()));
                        break;
                }
            }
            else if (this.AssetType == Helpers.AssetTypes.Svg)
            {
                internFileExtension = "svg";

                switch (this.AssetSize)
                {
                    case Helpers.AssetSizes.Render16Px:
                    case Helpers.AssetSizes.Render36Px:
                    case Helpers.AssetSizes.Render72Px:
                    case Helpers.AssetSizes.Render128Px:
                    case Helpers.AssetSizes.Render256Px:
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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.internRenderedText = new Parser(AssetSize,AssetType, RessourcesProvider).WebParseEmoji(this.Text);
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

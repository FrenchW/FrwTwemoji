// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmojiDisplay.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014-2016.
//   FrwTwemoji Project page : http://github.frenchw.net/FrwTwemoji/
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
        /// <summary>
        /// Backup for the rendered text
        /// </summary>
        private string internRenderedText = string.Empty;

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

        /// <summary>Gets or sets the text to render.</summary>
        /// <value>The text to render.</value>
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

        /// <summary>Internal renderer.</summary>
        /// <param name="writer">The writer.</param>
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

        /// <summary>
        /// The prerender handler <see cref="E:System.Web.UI.Control.PreRender" />.
        /// </summary>
        /// <param name="e">Object <see cref="T:System.EventArgs" /> that contains event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.internRenderedText = new Parser(this.AssetSize, this.AssetType, this.RessourcesProvider).WebParseEmoji(this.Text);
        }

        /// <summary>
        /// Renders the control in the writer.
        /// </summary>
        /// <param name="writer">Object <see cref="T:System.Web.UI.HtmlTextWriter" /> that receives the control content.</param>
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

        /// <summary>
        /// Renders the WebControl in the writer.
        /// </summary>
        /// <param name="writer"><see cref="T:System.Web.UI.HtmlTextWriter" /> to use for rendering.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            this.RenderContentsInternal(writer);
        }

        /// <summary>Checks the configuration.</summary>
        /// <param name="validationMessage">The validation message.</param>
        /// <returns>A <see cref="bool"/> returning true if the configuration is OK</returns>
        private bool CheckConfiguration(out string validationMessage)
        {
            List<string> errors = new List<string>();
            if (this.AssetType == Helpers.AssetTypes.Png)
            {
                if (this.AssetSize == Helpers.AssetSizes.Render128Px || this.AssetSize == Helpers.AssetSizes.Render256Px)
                {
                    errors.Add(string.Format("There is no png ressource for {0} resolution", this.AssetSize.ToString()));
                }

                switch (this.AssetSize)
                {
                    case Helpers.AssetSizes.Render16Px:
                        break;
                    case Helpers.AssetSizes.Render36Px:
                        break;
                    case Helpers.AssetSizes.Render72Px:
                        break;
                    default:
                        errors.Add(
                            string.Format("Asset type {0} has no resource at {0} resolution", this.AssetType.ToString()));
                        break;
                }
            }
            else if (this.AssetType == Helpers.AssetTypes.Svg)
            {
                switch (this.AssetSize)
                {
                        // ReSharper disable RedundantCaseLabel
                    case Helpers.AssetSizes.Render16Px:
                    case Helpers.AssetSizes.Render36Px:
                    case Helpers.AssetSizes.Render72Px:
                    case Helpers.AssetSizes.Render128Px:
                    case Helpers.AssetSizes.Render256Px:
                        // ReSharper disable RedundantEmptyDefaultSwitchBranch
                    default:
                        break;
                        // ReSharper restore RedundantEmptyDefaultSwitchBranch
                        // ReSharper restore RedundantCaseLabel
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

        /// <summary>handles action in case of change in the data.</summary>
        private void DataChanged()
        {
            string message;
            if (!this.CheckConfiguration(out message))
            {
                this.internRenderedText = message;
            }
        }
    }
}

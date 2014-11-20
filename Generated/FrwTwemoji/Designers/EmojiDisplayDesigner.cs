// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmojiDisplayDesigner.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014.
//   This software is licenced like https://github.com/twitter/twemoji :
//   Code licensed under the MIT License: http://opensource.org/licenses/MIT
//   Graphics licensed under CC-BY 4.0: https://creativecommons.org/licenses/by/4.0/ and created by Twitter
// </copyright>
// <summary>
//   Designer for the EmojiDisplay control
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FrwTwemoji.Designers
{
    using System;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.Design;

    /// <summary>
    /// Designer for the EmojiDisplay control
    /// </summary>
    public class EmojiDisplayDesigner : ControlDesigner
    {
        /// <summary>
        /// Récupère le balisage HTML utilisé pour représenter le contrôle au moment du design.
        /// </summary>
        /// <returns>
        /// Balisage HTML utilisé pour représenter le contrôle au moment du design.
        /// </returns>
        public override string GetDesignTimeHtml()
        {
            string retval;

            EmojiDisplay control = new EmojiDisplay
                                       {
                                           Text = "Today, Twitter is open sourcing their emoji to share with everyone 🎉 😜 👯 🍻 🎈 🎤 🎮 🚀 🌉 ✨",
                                           AssetSize = Helpers.AssetSizes.Render36Px,
                                           AssetType = Helpers.AssetTypes.Png,
                                       };

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new XhtmlTextWriter(sw))
                {
                    control.RenderContentsInternal(htw);
                    retval = htw.InnerWriter.ToString();
                }
            }

            return retval;
        }

        /// <summary>
        /// Récupère le balisage HTML qui fournit des informations sur l'exception spécifiée. 
        /// </summary>
        /// <returns>
        /// Balisage HTML au moment du design pour l'exception spécifiée.
        /// </returns>
        /// <param name="e">Exception survenue. </param>
        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            // pour savoir précisement ou le code plante dans la méthode Renderdu 
            string message = "<div>" + base.GetErrorDesignTimeHtml(e)
                + string.Format("<br /><p>Message: <blockquote>{0}<blockquote>", e.Message.Replace("\r\n", "<br />"))
                + string.Format("<br /><p>Stack : <blockquote>{0}<blockquote>", e.StackTrace.Replace("\r\n", "<br />"))
                + "</div>";
            return this.CreatePlaceHolderDesignTimeHtml(message);
        }
    }
}

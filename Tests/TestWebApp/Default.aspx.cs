// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Default.aspx.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014-2016.
//   FrwTwemoji Project page : http://github.frenchw.net/FrwTwemoji/
//   This software is licenced like https://github.com/twitter/twemoji :
//   Code licensed under the MIT License: http://opensource.org/licenses/MIT
//   Graphics licensed under CC-BY 4.0: https://creativecommons.org/licenses/by/4.0/ and created by Twitter
// </copyright>
// <summary>
//   Defines the _Default type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TestWebApp
{
    using FrwTwemoji;
    using System;

    public partial class PageDefault : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // direct rendering of the text
            this.EmojiDisplay2Local.Text = TestString.manger;
            this.EmojiDisplay2MaxCdn.Text = TestString.manger;
        }

        protected void BtnRender_Click(object sender, EventArgs e)
        {
            this.spnTestsLocal.InnerHtml = Parser.ParseEmoji(this.TxtEmoji.Text);
            this.spnTestsLocalMaxCdn.InnerHtml = Parser.ParseEmoji(this.TxtEmoji.Text, provider: Helpers.RessourcesProviders.MaxCdn);
            this.EmojiDisplay1Local.Text = this.TxtEmoji.Text;
            this.EmojiDisplay1MaxCdn.Text = this.TxtEmoji.Text;
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Default.aspx.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014-2020.
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
            this.spnTestsLocal.InnerHtml = Parser.ParseEmoji(TestString.TwitterAnnouncement);
            this.spnTestsLocalMaxCdn.InnerHtml = Parser.ParseEmoji(TestString.TwitterAnnouncement, provider: Helpers.RessourcesProviders.MaxCdn);


            this.EmojiDisplayDebugLocal.Text = TestString.F1TweetSequel;
            this.EmojiDisplayDebugMaxCdn.Text = TestString.F1TweetSequel;
        }

        protected void BtnRender_Click(object sender, EventArgs e)
        {
            this.spnTestsLocal.InnerHtml = Parser.ParseEmoji(this.TxtEmoji.Text.Replace(Environment.NewLine, "<br />"));
            this.spnTestsLocalMaxCdn.InnerHtml = Parser.ParseEmoji(this.TxtEmoji.Text.Replace(Environment.NewLine, "<br />"), provider: Helpers.RessourcesProviders.MaxCdn);
            this.EmojiDisplay1Local.Text = this.TxtEmoji.Text.Replace(Environment.NewLine, "<br />");
            this.EmojiDisplay1MaxCdn.Text = this.TxtEmoji.Text.Replace(Environment.NewLine, "<br />");
        }
    }
}
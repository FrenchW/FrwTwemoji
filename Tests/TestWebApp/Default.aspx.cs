﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Default.aspx.cs" company="FrenchW.net from @FrenchW">
//   Copyright FrenchW © 2014.
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
    using System;

    using FrwTwemoji;

    public partial class PageDefault : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // direct rendering of the text
            EmojiDisplay2.Text = TestString.manger;
        }

        protected void BtnRender_Click(object sender, EventArgs e)
        {
            spnTests.InnerHtml = Parser.ParseEmoji(TxtEmoji.Text);
            EmojiDisplay2.Text = TxtEmoji.Text;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestWebApp
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SpnTestString1.InnerText = FrwTwemoji.Parser.ParseEmoji(TestStrings.String1);
        }
    }
}
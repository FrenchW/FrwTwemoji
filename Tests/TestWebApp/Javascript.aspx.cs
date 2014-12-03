using System;
namespace TestWebApp
{
    using FrwTwemoji;

    /// <summary>
    /// Javascript Test PAge
    /// </summary>
    public partial class PageJavascript : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Javascript.AddJavascriptToPageClientScript(this, false);
            this.LitJavascriptUrl.Text = Javascript.GetJavascriptRessourceUrl(false);
        }
    }
}
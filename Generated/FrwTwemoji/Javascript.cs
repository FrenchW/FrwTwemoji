namespace FrwTwemoji
{
    using System.IO;
    using System.Web.UI;

    /// <summary>Handles twemoji javascript resources
    /// </summary>
    public static class Javascript
    {
        /// <summary>
        /// Adds the twemoji javascript to page ClientScript.
        /// </summary>
        /// <param name="page">The page to add the clientScrtipt to.</param>
        /// <param name="useMinified">if set to <c>true</c> the minified javascript is served.</param>
        public static void AddJavascriptToPageClientScript(Page page, bool useMinified = true)
        {
            page.ClientScript.RegisterClientScriptResource(typeof(Javascript), GetStringName(useMinified));
        }

        /// <summary>
        /// Gets the twemoji javascript download url (relative).
        /// </summary>
        /// <param name="useMinified">if set to <c>true</c> the minified javascript is served.</param>
        /// <returns>The url (root relative) of the resource served by webresources.axd</returns>
        public static string GetJavascriptRessourceUrl(bool useMinified = true)
        {
            return new Page().ClientScript.GetWebResourceUrl(typeof(Javascript), GetStringName(useMinified));
        }

        /// <summary>
        /// Gets the name of the javascript resource.
        /// </summary>
        /// <param name="useMinified">if set to <c>true</c> [use minified].</param>
        /// <returns></returns>
        private static string GetStringName(bool useMinified = true)
        {
           return useMinified ? "FrwTwemoji.Js.twemoji.min.js" : "FrwTwemoji.Js.twemoji.js";
        }
    }
}

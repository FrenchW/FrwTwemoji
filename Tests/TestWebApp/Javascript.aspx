<%@Page Title="Embed Javascript test page"  Language="C#" AutoEventWireup="true" CodeBehind="Javascript.aspx.cs" Inherits="TestWebApp.PageJavascript" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <p>This page contains some text with emoji. Twemoji javascript files are embed and served with one simple call:</p>
        <pre>
// Easy way, get simply the url and use it in your code :
LitJavascriptUrl.Value = Javascript.GetJavascriptRessourceUrl(false);
// wich outputs : <asp:Literal runat="server" ID="LitJavascriptUrl"></asp:Literal>

// Or the full way
Javascript.AddJavascriptToPageClientScript(true); 
// wich adds the ClientScript block to the page.
       </pre>
        <p>Then, you just have to use twemoji classic javascript call : </p>
        <pre>&lt;script&gt;twemoji.parse(document.getElementById('emojiTest'), {size: 72});&lt;/script&gt;</pre>
        <div>
            <div id="emojiTest">Test render : <span>J'ai mangé chinois 😱😍🍱🍣🍥🍙🍘🍚🍜🍱🍣🍥🍙🍘🍚🍜</span></div>
            <script>twemoji.parse(document.getElementById('emojiTest'), { size: 72 });</script>
        </div>
    </form>
</body>
</html>

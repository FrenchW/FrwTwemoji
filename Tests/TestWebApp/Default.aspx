<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestWebApp._Default" %>

<%@ Register Assembly="FrwTwemoji" Namespace="FrwTwemoji" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>FrwTwemoji Test Page</h1>
            <h2>Debug tests</h2>
            <p>Type some text and lick &quot;Render&quot;</p>
            <div>
                <asp:TextBox ID="TxtEmoji" runat="server" TextMode="MultiLine" Height="68px" Width="356px"></asp:TextBox><br/>
                <asp:Button ID="BtnRender" runat="server" Text="Render" OnClick="BtnRender_Click" />
            </div>
           <p>Rendering : <span runat="server" id="spnTests"></span></p> 
            <h2>Emoji Display</h2>
            <cc1:EmojiDisplay ID="EmojiDisplay1"
                runat="server"
                Text="Today,  Twitter is open sourcing their emoji to share with everyone 🎉 😜 👯 🍻 🎈 🎤 🎮 🚀 🌉 ✨" />

            <h2>French
            </h2>
            <p>
                <cc1:EmojiDisplay ID="EmojiDisplay2"
                    runat="server" AssetSize="Render72Px" AssetType="Png" />
            </p>
        </div>
    </form>
</body>
</html>

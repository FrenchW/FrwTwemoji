<%@ Page Title="FrwTwemoji Test page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestWebApp.PageDefault" %>

<%@ Register Assembly="FrwTwemoji" Namespace="FrwTwemoji" TagPrefix="cc1" %>
<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Helvetica, Arial, sans-serif, 'Apple Color Emoji', 'Segoe UI Emoji', 'Segoe UI Symbol';
            font-size: 15px;
        }

        h1 {
            font-size: 1.2em;
        }

        h2 {
            font-size: 1.1em;
        }

        table {
            width: 100%;
            border: 1Px solid black;
            border-collapse: collapse;
            empty-cells: show;
            border-spacing: 0;
        }

        th {
            border: 1px solid black;
            font-weight: bold;
        }

        td {
            border: 1px solid black;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>FrwTwemoji Test Page</h1>
            <h2>Debug tests</h2>
            <p>Type some text and click &quot;Render&quot;</p>
            <div>
                <asp:TextBox ID="TxtEmoji" runat="server" TextMode="MultiLine" Height="150px" Width="600px" Text="Emotions:😇😗😘🤑🤡🤠😡😱😢😈🤥😬🤢💀👻😹🤝👍🏾☝️☝🏻☝🏼☝🏽☝🏾☝🏿🤘🏼🤞🏿🗣👶🏽👨‍🚒👩‍🚒🤴👰🏽👱👱🏻👱🏼👱🏾👨‍🎤👩🏾‍🎓👨🏿‍🎤👩🏼‍🚀👨🏾‍🚀🚶🏽‍♀️👯👩‍❤️‍💋‍👩👡
Animals/nature:🐶🐹🦁🙊🐧🐔🦉🐛🦄🕷🦐🐬🦏🐘🐑🐁🐾🐉🐳🎍🥀🌼🌞🌈❄️💦✨⚡️🔥💥🌧🌩☃️🌬💨🌪🌫
Food:🍒🍑🥝🥑🍖🍤🥓🍟🥙🌯🥘🍙🍱🍡🍭🍿🍩🍻🍵🥂🍾🍹🥜🍇🍫🍬🍸🍽🍴🍰
Sports : ⚽️🏀⚾️🏈🏐🏒⛸🤺🤼‍♂️🤾🏻‍♀️⛹🏿‍♀️🤸🏽‍♀️🎣🏌🏽🎲🎻🎸🎳🥁🤼‍♀️⛹🏼‍♀️⛹🏼⛹🏻⛹🏻🏋🏿‍♀️🏇🚣🏽🥈🥉🤹🏽‍♂️🤹🏾‍♀️🏵
Travels:🚗🚌🚚🚔🚋🚃🚟🚅🛩✈️🚀🛳🚢🏰🎢🎡🏟🏯🏦🏛🏩🚡🕌🕋🗾🌆🌁🌋🏜
Objects:⌚️📱📹📟⏲🕰⏰🔦📡🎛💿⚙️⛏💈📿🔮💉🚿🛁🛎🛀🏾🎎📦📜📙🖌💗⛎❌🚷⚠️🔱🛃🔟"></asp:TextBox><br />
                <asp:Button ID="BtnRender" runat="server" Text="Render" OnClick="BtnRender_Click" />
            </div>
            <h2>Using code parsing functions (Parser.ParseEmoji(this.TxtEmoji.Text);)</h2>
            <table>
                <thead>
                    <tr>
                        <th>Using FrwTwemoji ressource file</th>
                        <th>Using MaxCdn images</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><span runat="server" id="spnTestsLocal"></span></td>
                        <td><span runat="server" id="spnTestsLocalMaxCdn"></span></td>
                    </tr>
                </tbody>
            </table>

            <h2>Using EmojiDisplay (png 36px)</h2>

            <table>
                <thead>
                    <tr>
                        <th>Using FrwTwemoji ressource file</th>
                        <th>Using MaxCdn images</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <cc1:EmojiDisplay ID="EmojiDisplay1Local"
                                runat="server"
                                RessourcesProvider="Localhost"
                                AssetType="Png"
                                AssetSize="Render36Px"
                                Text="Today,  Twitter is open sourcing their emoji to share with everyone 🎉 😜 👯 🍻 🎈 🎤 🎮 🚀 🌉 ✨" />
                        </td>
                        <td>
                            <cc1:EmojiDisplay ID="EmojiDisplay1MaxCdn"
                                runat="server"
                                RessourcesProvider="MaxCdn"
                                AssetType="Png"
                                AssetSize="Render36Px"
                                Text="Today,  Twitter is open sourcing their emoji to share with everyone 🎉 😜 👯 🍻 🎈 🎤 🎮 🚀 🌉 ✨" />
                        </td>
                    </tr>
                </tbody>
            </table>

            <h2>Using EmojiDisplay (svg 16px)
            </h2>
            <table>
                <thead>
                    <tr>
                        <th>Using FrwTwemoji ressource file</th>
                        <th>Using MaxCdn images</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <cc1:EmojiDisplay ID="EmojiDisplay2Local"
                                runat="server" AssetSize="Render16Px" AssetType="Svg" />
                        </td>
                        <td>
                            <cc1:EmojiDisplay ID="EmojiDisplay2MaxCdn"
                                RessourcesProvider="MaxCdn"
                                runat="server" AssetSize="Render16Px" AssetType="Svg" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <p>
            </p>
            <h2>And it supports skintone variants </h2>
            <table>
                <thead>
                    <tr>
                        <th>Using FrwTwemoji ressource file</th>
                        <th>Using MaxCdn images</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <cc1:EmojiDisplay ID="EmojiDisplay3Local"
                                runat="server"
                                RessourcesProvider="Localhost"
                                AssetType="svg"
                                AssetSize="Render72Px"
                                Text="And it does support skin tone : 🤙🤙🏻🤙🏼🤙🏽🤙🏾🤙🏿
                                Complex skin tone and gender mixture : 🤽🏿‍♀️🏄🏾‍♀️
                                Complex association : 👨‍👩‍👧‍👦" />
                        </td>
                        <td>
                            <cc1:EmojiDisplay ID="EmojiDisplay3MaxCdn"
                                runat="server"
                                RessourcesProvider="MaxCdn"
                                AssetType="svg"
                                AssetSize="Render72Px"
                                Text="And it does support skin tone : 🤙🤙🏻🤙🏼🤙🏽🤙🏾🤙🏿
                                Complex skin tone and gender mixture : 🤽🏿‍♀️🏄🏾‍♀️
                                Complex association : 👨‍👩‍👧‍👦" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <h2>Debug purpose
            </h2>
            <table>
                <thead>
                    <tr>
                        <th>Using FrwTwemoji ressource file</th>
                        <th>Using MaxCdn images</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <cc1:EmojiDisplay ID="EmojiDisplayDebugLocal"
                                runat="server" AssetSize="Render72Px" AssetType="Svg" />
                        </td>
                        <td>
                            <cc1:EmojiDisplay ID="EmojiDisplayDebugMaxCdn"
                                RessourcesProvider="MaxCdn"
                                runat="server" AssetSize="Render72Px" AssetType="Svg" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <p>
            </p>
            <h2>Other Pages</h2>
            <ul>
                <li><a href="Javascript.aspx">Javascript</a> test Page</li>
            </ul>
        </div>
    </form>
</body>
</html>

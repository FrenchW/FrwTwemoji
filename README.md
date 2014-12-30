FrwTwemoji
==========

The main purpose of FrwTwemoji project is to have TwEmoji  hosted in any dotnet or mono project.
Twemoji is the twitter initiative to open source emoji pictures and javascript parser : [twitter/twemoji](https://github.com/twitter/twemoji))
The goal is to include those pictures as resources (and WebResources) and also to have a .net parser to detect emoji caracters and display the pictures at their place.

 - [Downloads](#downloads) 
 - [Project Source](https://github.com/FrenchW/FrwTwemoji)
 - People : 
   - [FrenchW](http://githubfrenchw.net) ([Twitter @FrenchW](https://twitter.com/FrenchW))
 - Content :
   - [EmojiDisplay WebControl](#emojidisplay-webcontrol)
   - [Simple parser](#simple-parser)
   - [Twitter's original javascript](#twitter-original-javascript) as WebResource

##Downloads

You can download the latest FrwTwemoji assemblies [here](http://j.mp/1Bks3uN) for a direct use. 

Current version: 1.2.0.

##Downloads

You can download the latest FrwTwemoji assemblies [here](http://j.mp/1AlEIza) for a direct use

##EmojiDisplay WebControl

###Usage
Simply drop EmojiDisplay control on your web Page, set rendering options and add some text containing emojis :

```csharp
<h2>Emoji Display</h2>
<cc1:EmojiDisplay ID="EmojiDisplay1"
    runat="server"
    Text="Today, Twitter is open sourcing their emoji 
        to share with everyone  🎉 😜 👯 🍻 🎈 🎤 🎮 🚀 🌉 ✨" />
```
and it displays like this : 
![alt text](README_1.png)

##Simple parser

Anywhere, use the Parser function to get the string converted with string + images

###Usage

```csharp
MyHtmlSpanElement.InnerHtml = 
    FrwTwemoji.Parser.ParseEmoji(SomeStringContainingEmojiCaracters);
```

##Twitter Original javascript
The original twitter javascript twemoji is also included as a WebResource : the minified one (set the argument to true) or not (false)

```csharp
// Easy way, get simply the url and use it in your code :
LitJavascriptUrl.Value = Javascript.GetJavascriptRessourceUrl(false);
// wich outputs : [ Literal "LitJavascriptUrl" ]

// Or the full way
Javascript.AddJavascriptToPageClientScript(true); 
// wich adds the ClientScript block to the page.```


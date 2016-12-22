# FrwTwemoji

The main purpose of FrwTwemoji project is to have TwEmoji hosted in any dotnet or mono project.
Twemoji is the twitter initiative to open source emoji pictures and javascript parser : [twitter/twemoji](https://github.com/twitter/twemoji))
The goal is to include those pictures as resources (and WebResources) and also to have a .net parser to detect emoji caracters and display the pictures at their place.


* [Project Source](https://github.com/FrenchW/FrwTwemoji)
* People : 
   - [FrenchW](http://github.frenchw.net) (Twitter [@FrenchW](https://twitter.com/FrenchW))
* [Downloads](#downloads) 
* Content :
   - [EmojiDisplay WebControl](#WebControl)
   - [Simple parser](#simpleparser)
   - [Twitter's original javascript](#javascript) as WebResource

## Downloads<a id="downloads" name="downloads"></a>

You can download the latest FrwTwemoji assemblies [here](http://j.mp/1Bks3uN) for a direct use. 

Current version: 1.2.0.

<!-- Responsive -->
<ins class="adsbygoogle"
     style="display:block"
     data-ad-client="ca-pub-5683856818165673"
     data-ad-slot="6494445466"
     data-ad-format="auto"></ins>


## EmojiDisplay WebControl <a id="WebControl" name="WebControl"></a>

### Usage
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

## Simple parser <a id="simpleparser" name="simpleparser"></a>

Anywhere, use the Parser function to get the string converted with string + images

### Usage

```csharp
MyHtmlSpanElement.InnerHtml = 
    FrwTwemoji.Parser.ParseEmoji(SomeStringContainingEmojiCaracters);
```

## Twitter Original javascript <a id="javascript" name="javascript"></a>
The original twitter javascript twemoji is also included as a WebResource : the minified one (set the argument to true) or not (false)

```csharp
// Easy way, get simply the url and use it in your code :
LitJavascriptUrl.Value = Javascript.GetJavascriptRessourceUrl(false);
// wich outputs : [ Literal "LitJavascriptUrl" ]

// Or the full way
Javascript.AddJavascriptToPageClientScript(true); 
// wich adds the ClientScript block to the page. 
```



<!-- Responsive -->
<ins class="adsbygoogle"
     style="display:block"
     data-ad-client="ca-pub-5683856818165673"
     data-ad-slot="6494445466"
     data-ad-format="auto"></ins>


<script async src="//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>
<script>
(adsbygoogle = window.adsbygoogle || []).push({});
</script>
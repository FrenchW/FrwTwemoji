# FrwTwemoji project update process

## 1 - update to twemoji

- with git, open the folder twitter-twemoji
- make a git pull

## 2 - project update

 - load the project in Visual studio
 - Check current year in
   - Readme.md
   - Generator/templates.resx
   - Generator/AssemblyInfo.cs
   - TestWebApp/AssemblyInfo.cs
   - TestWinFormApp/AsemblyInfo.cs
 - Update the assembly version to match twemoji version . **This is important as it will make the program load the assets in the right folder !**
   - Generator/templates.resx
   - Generator/AssemblyInfo.cs
   - TestWebApp/AssemblyInfo.cs
   - TestWinFormApp/AsemblyInfo.cs
 - Set DEBUG and run project "Generator" (will update all files)
 - Check the updated files like EmojiSources.txt and StandardizedProcess.txt for interesting information
 - Verify if the assets are loaded in the right folder (matches twemoji version)
 - check in `twemoji.js` if (around line 228) if the regular expression has been updated . If so:
   -  copy the regular expression right after he first / and stop before the last /g
   -  paste in an advanced text editor
   -  set it to uppercase
   -  replace \U with \\\u (double)
   -  copy and paste in Templates.resx, parameter `Twemoji_RegEx`

## Tests

 - Set DEBUG and run project "Generator" (will regenerate all)
 - start TestWebapp and check

## Documentation
 - update [Changelog.md]() and [README.md]()

## Distribution

 - Set `Release` and run project "Generator" (will update all files)
 -  Build (F6) in release
 -  Zip the `BuildOutput` folder content

 - Update the pages
   - English [https://frenchw.net/en/frwtwemoji/](https://frenchw.net/en/frwtwemoji/)
   - French [https://frenchw.net/frwtwemoji/](https://frenchw.net/frwtwemoji/) 

##Project distribution


TwitchPlaysCookieClicker
========================

Cookie Clicker played by Twitch.tv chat.  Simply enter the name of the item you wish to buy and it 
will be bought.  This implementation utilizes WebSockets and UserScript injection into the Cookie 
Clicker webpage to allow us to call Cookie Clicker functions remotely.  The first time this is run
it will create a config file, edit the file and rerun the application.  Load the userscript in your
WebSocket supported browser (using chrome and developer mode you can load this via extensions) and
the application should be read to transmit irc chat to the websocket client.

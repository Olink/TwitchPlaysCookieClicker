// ==UserScript==
// @name        CookieClicker
// @namespace   http://orteil.dashnet.org/cookieclicker/
// @description http://orteil.dashnet.org/cookieclicker/
// @include     http://orteil.dashnet.org/cookieclicker/
// @version     1
// ==/UserScript==


window.addEventListener ("load", LocalMain, false);
var ws;
function SetupSocket()
{
   ws = new WebSocket("ws://localhost:9998/");
   ws.onopen = function()
   {
     console.log("Connected")
     
     setInterval(function(){
      console.log("Pinging");
      this.ws.send("PING");
     },1000 * 60 * 4);
   };
   ws.onmessage = function (evt) 
   { 
      var received_msg = evt.data.toLowerCase();
      
      if (received_msg === "click") {
        Game.ClickCookie();
        console.log("Cookie Clicked!");
      }
      
      for (var i in Game.Objects)//buildings
			{
				var me=Game.Objects[i];
        if (me.name.toLowerCase() === received_msg) {
          console.log(me.name + " was bought!");
          me.buy();
        }
      };
      
      for (var i in Game.Upgrades)//buildings
			{
				var me=Game.Upgrades[i];
        if (me.name.toLowerCase() === received_msg) {
          console.log(me.name + " was bought!");
          me.buy();
        }
      };
   };
   ws.onclose = function()
   { 
     console.log("Closed")
   };
}

function LocalMain ()
{
  var script = document.createElement('script');
  script.appendChild(document.createTextNode('('+ SetupSocket +')();'));
  (document.body).appendChild(script);
}

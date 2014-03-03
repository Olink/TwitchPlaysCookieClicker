using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Alchemy;
using Alchemy.Classes;

namespace CookieClicker
{
	class GameManager
	{
		public static Config Config;

		private string configPath = "config.json";
		private UserContext wsClient;
		
		public GameManager()
		{
			if (File.Exists(configPath))
			{
				Config = Config.Readfile(configPath);
			}
			else
			{
				Config = new Config();
				Config.WriteFile(configPath);
				Environment.Exit(0);
			}

			IrcConnector irc = new IrcConnector();
			irc.MessageReceived += OnMessage;

			WebSocketServer wsServer = new WebSocketServer(9998, IPAddress.Any)
			{
				OnReceive = OnReceive,
				OnSend = OnSend,
				OnConnect = OnConnected,
				OnConnected = OnConnect,
				OnDisconnect = OnDisconnect,
				TimeOut = TimeSpan.FromMinutes(5),
			};
			wsServer.Start();

			string line;
			while ((line = Console.ReadLine()) != "exit")
			{
				irc.SendMessage(line);
			}

			wsServer.Stop();
			irc.Disconnect();
		}

		private void OnDisconnect(UserContext context)
		{
			wsClient = null;
			//Console.WriteLine("Client disconnected");
		}

		private void OnConnect(UserContext context)
		{
			if (wsClient != null)
				wsClient.Send("dc");

			wsClient = context;
			//Console.WriteLine("Client connected");
		}

		private void OnConnected(UserContext context)
		{
			
		}

		private void OnSend(UserContext context)
		{
			//throw new NotImplementedException();
		}

		private void OnReceive(UserContext context)
		{
			//throw new NotImplementedException();
		}


		private void OnMessage(MessageEventArgs e)
		{
			if (wsClient == null)
				return;

			
			wsClient.Send(e.Message);
			Console.WriteLine(e.Message);
		}
	}
}

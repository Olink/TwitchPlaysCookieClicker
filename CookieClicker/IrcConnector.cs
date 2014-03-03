using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace CookieClicker
{
	class IrcConnector
	{
		private TcpClient tcpClient;
		private StreamReader tcpReader;
		private StreamWriter tcpWriter;

		public delegate void MessageEventD(MessageEventArgs e);
		public event MessageEventD MessageReceived;

		public IrcConnector()
		{
			tcpClient = new TcpClient(GameManager.Config.ircservername, 6667);
			tcpReader = new StreamReader(tcpClient.GetStream());
			tcpWriter = new StreamWriter(tcpClient.GetStream());

			ThreadPool.QueueUserWorkItem(BeginListen);

			SendCommand("PASS", GameManager.Config.oauthPassword);
			SendCommand("NICK", GameManager.Config.twitchUsername);
			SendCommand("JOIN", GameManager.Config.ircChannel);
		}

		public void SendMessage(string message)
		{
			SendCommand("PRIVMSG", String.Format("{0} :{1}", GameManager.Config.ircChannel, message));
		}

		private void SendCommand(string command, string message)
		{
			tcpWriter.WriteLine("{0} {1}", command, message);
			tcpWriter.Flush();
		}

		//:tpcookieclicker!tpcookieclicker@tpcookieclicker.tmi.twitch.tv PRIVMSG #tpcookieclicker :wassup
		public void BeginListen(object state)
		{
			string line;
			try
			{
				while ((line = tcpReader.ReadLine()) != null)
				{
					Console.WriteLine(line);
					if (line.StartsWith("PING"))
					{
						tcpWriter.WriteLine("PONG tmi.twitch.tv");
						tcpWriter.Flush();
					}
					else if (line.Contains("PRIVMSG"))
					{
						string[] parts = line.Split(':');
						string message = String.Join(":", parts, 2, parts.Length - 2);

						string name = (parts[1].Split('!'))[0];

						if (MessageReceived != null)
						{
							MessageReceived.Invoke(new MessageEventArgs
							{
								Message = message,
								Name = name
							});
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		public void Disconnect()
		{
			tcpClient.Close();
		}
	}

	public class MessageEventArgs
	{
		public string Message { get; set; }
		public string Name { get; set; }
	}
}

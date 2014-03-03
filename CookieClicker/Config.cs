using System.IO;
using Newtonsoft.Json;

namespace CookieClicker
{
	class Config
	{
		public string ircservername = "irc.twitch.tv";
		public string oauthPassword = "";
		public string twitchUsername = "";
		public string ircChannel = "";

		public static Config Readfile(string path)
		{
			using (var reader = new StreamReader(path))
			{
				return JsonConvert.DeserializeObject<Config>(reader.ReadToEnd());
			}
		}

		public void WriteFile(string path)
		{
			using (var writer = new StreamWriter(path))
			{
				writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
				writer.Flush();
			}
		}
	}
}

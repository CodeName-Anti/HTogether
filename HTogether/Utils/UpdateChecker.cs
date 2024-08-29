using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace HTogether.Utils;

public class UpdateChecker
{
	private static Dictionary<Tuple<string, string>, bool> updateCache = [];

	public static bool IsUpdateAvailable(string repoOwner, string repo, string currrentVersion)
	{
		Tuple<string, string> tuple = new(repoOwner, repo);

		if (updateCache.TryGetValue(tuple, out bool value))
		{
			return value;
		}

		using var client = new WebClient();
		// Some random user agent because with others it responds with 403
		client.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:93.0) Gecko/20100101 Firefox/93.0");
		string json = client.DownloadString($"https://api.github.com/repos/{repoOwner}/{repo}/releases");

		JArray jArr = JArray.Parse(json);

		string stringVersion = jArr[0].ToObject<JObject>().GetValue("tag_name").ToObject<string>();

		// Compare GitHub and Local Version
		Version git = new(stringVersion);
		Version current = Assembly.GetExecutingAssembly().GetName().Version;

		int result = current.CompareTo(git);
		bool updateAvailable = result < 0;

		updateCache[tuple] = updateAvailable;

		return updateAvailable;
	}
}
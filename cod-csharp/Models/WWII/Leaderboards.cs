using System.Collections.Generic;

namespace COD.Models.WWII
{
	public class Leaderboards
	{
		public class Values
		{
			public double wins { get; set; }
			public double kills { get; set; }
			public double kdRatio { get; set; }
			public double level { get; set; }
			public double losses { get; set; }
			public double prestige { get; set; }
			public double totalXp { get; set; }
			public double score { get; set; }
			public double timePlayed { get; set; }
			public double averageTime { get; set; }
			public double gamesPlayed { get; set; }
			public double scorePerMinute { get; set; }
			public double deaths { get; set; }
		}

		public class Entry
		{
			public double rank { get; set; }
			public string username { get; set; }
			public double updateTime { get; set; }
			public double rating { get; set; }
			public Values values { get; set; }
		}

		public class Data
		{
			public string title { get; set; }
			public string platform { get; set; }
			public string leaderboardType { get; set; }
			public string gameMode { get; set; }
			public int page { get; set; }
			public int resultsRequested { get; set; }
			public int totalPages { get; set; }
			public object sort { get; set; }
			public List<string> columns { get; set; }
			public List<Entry> entries { get; set; }
		}

		public string status { get; set; }
		public Data data { get; set; }
	}
}

namespace COD.Models.IW
{
	public class Profile
	{
		public class All
		{
			public double wins { get; set; }
			public double kills { get; set; }
			public double timePlayed { get; set; }
			public double lastUpdated { get; set; }
			public double kdRatio { get; set; }
			public double headshots { get; set; }
			public double deaths { get; set; }
			public double killstreaks { get; set; }
		}

		public class Dd
		{
			public double timePlayed { get; set; }
			public double lastUpdated { get; set; }
		}

		public class Sd
		{
			public double timePlayed { get; set; }
			public double lastUpdated { get; set; }
		}

		public class Ball
		{
			public double timePlayed { get; set; }
			public double lastUpdated { get; set; }
		}

		public class Gun
		{
			public double timePlayed { get; set; }
			public double lastUpdated { get; set; }
		}

		public class Dom
		{
			public double timePlayed { get; set; }
			public double lastUpdated { get; set; }
		}

		public class Ctf
		{
			public double timePlayed { get; set; }
			public double lastUpdated { get; set; }
		}

		public class Safe
		{
			public double timePlayed { get; set; }
			public double lastUpdated { get; set; }
		}

		public class Dm
		{
			public double timePlayed { get; set; }
			public double lastUpdated { get; set; }
		}

		public class Koth
		{
			public double timePlayed { get; set; }
			public double lastUpdated { get; set; }
		}

		public class Kill
		{
			public double timePlayed { get; set; }
			public double lastUpdated { get; set; }
		}

		public class Tdm
		{
			public double timePlayed { get; set; }
			public double lastUpdated { get; set; }
		}

		public class Mode
		{
			public Dd dd { get; set; }
			public Sd sd { get; set; }
			public Ball ball { get; set; }
			public Gun gun { get; set; }
			public Dom dom { get; set; }
			public Ctf ctf { get; set; }
			public Safe safe { get; set; }
			public Dm dm { get; set; }
			public Koth koth { get; set; }
			public Kill kill { get; set; }
			public Tdm tdm { get; set; }
		}

		public class Lifetime
		{
			public All all { get; set; }
			public Mode mode { get; set; }
		}

		public class Mp
		{
			public Lifetime lifetime { get; set; }
			public object weekly { get; set; }
			public double level { get; set; }
			public double maxLevel { get; set; }
			public double levelXpRemainder { get; set; }
			public double levelXpGained { get; set; }
			public double prestige { get; set; }
			public double prestigeId { get; set; }
			public double maxPrestige { get; set; }
		}

		public class All2
		{
			public double timePlayed { get; set; }
			public double lastUpdated { get; set; }
		}

		public class Mode2
		{
		}

		public class Lifetime2
		{
			public All2 all { get; set; }
			public Mode2 mode { get; set; }
		}

		public class Zombies
		{
			public Lifetime2 lifetime { get; set; }
			public object weekly { get; set; }
		}

		public class Engagement
		{
			public double plays { get; set; }
			public double dlc4 { get; set; }
			public double spend { get; set; }
			public double dlc3 { get; set; }
			public double dlc2 { get; set; }
			public double timePlayedCampaign { get; set; }
			public double dlc1 { get; set; }
			public double seasonPass { get; set; }
			public double engagementLevel { get; set; }
		}

		public class Data
		{
			public string title { get; set; }
			public string platform { get; set; }
			public string username { get; set; }
			public Mp mp { get; set; }
			public Zombies zombies { get; set; }
			public Engagement engagement { get; set; }
		}

		public string status { get; set; }
		public Data data { get; set; }

	}
}

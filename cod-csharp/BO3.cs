using System;
using COD.Models.BO3;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.ComponentModel;

namespace COD
{
	public static class BO3
	{

		public enum Gamemode
		{
			[Description("career")]
			Career,

			[Description("war")]
			TDM,

			[Description("dm")]
			FreeForAll,

			[Description("conf")]
			KillConfirmed,

			[Description("dom")]
			Domination,

			[Description("hp")]
			Hardpoint,

			[Description("sd")]
			SearchAndDestroy,

			[Description("demo")]
			Demolition,

			[Description("ctf")]
			CaptureTheFlag,

			[Description("ball")]
			Uplink,

			[Description("escort")]
			Safeguard,

			[Description("gun")]
			GunGame
		}

		public enum Type
		{
			[Description("core")]
			Core,

			[Description("hc")]
			Hardcore,

			[Description("arena")]
			Arena
		}

		private static bool IsValidHardcoreMode(Gamemode mode)
		{
			return mode == Gamemode.Career
				|| mode == Gamemode.TDM
				|| mode == Gamemode.FreeForAll
				|| mode == Gamemode.KillConfirmed
				|| mode == Gamemode.Domination
				|| mode == Gamemode.SearchAndDestroy
				|| mode == Gamemode.CaptureTheFlag;
		}

		private static bool IsValidArenaMode(Gamemode mode)
		{
			return mode == Gamemode.Career
				|| mode == Gamemode.Hardpoint
				|| mode == Gamemode.SearchAndDestroy
				|| mode == Gamemode.CaptureTheFlag
				|| mode == Gamemode.Uplink;
		}

		/// <summary>
		/// Get a user's profile and stats.
		/// </summary>
		/// <param name="platform">Platform the user is on</param>
		/// <param name="username">Username of the user</param>
		/// <returns>Profile data</returns>
		public static Profile.Data GetProfile(Platform platform, string username)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{Utilities.BO3_URL}platform/{platform.GetDescription()}/gamer/{username}/profile/").Result;

				if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"Bad response {response.StatusCode}");

				var responseData = response.Content.ReadAsStringAsync().Result;

				if (Utilities.ValidResponse(responseData))
					return JsonConvert.DeserializeObject<Profile>(responseData).data;

				dynamic data = JsonConvert.DeserializeObject(responseData);

				throw new CODException(data.data.message.ToString());
			}
		}

		/// <summary>
		/// Get leaderboard data based on page number.
		/// </summary>
		/// <param name="platform">Platform to get leaderboards for</param>
		/// <param name="time">Length of time to return leaderboard data for</param>
		/// <param name="type">Type of mode to search for</param>
		/// <param name="mode">Gamemode to get leaderboard data for</param>
		/// <param name="page">Page to fetch</param>
		/// <returns>Leaderboard data</returns>
		public static Leaderboards.Data GetLeaderboards(Platform platform, Time time, Type type, Gamemode mode, int page = 1)
		{
			using (var client = new HttpClient())
			{
				//Business logic
				if (type == Type.Arena && !IsValidArenaMode(mode)) throw new CODException($"Mode {mode.ToString()} is not a valid Arena mode.");
				if (type == Type.Hardcore && !IsValidHardcoreMode(mode)) throw new CODException($"Mode {mode.ToString()} is not a valid Hardcore mode.");

				var response = client.GetAsync($"{Utilities.BO3_LEADERBOARDS_URL}platform/{platform.GetDescription()}/time/{time.GetDescription()}/type/{type.GetDescription()}/mode/{mode.GetDescription()}/page/{page}/").Result;

				if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"Bad response {response.StatusCode}");

				var responseData = response.Content.ReadAsStringAsync().Result;

				if (Utilities.ValidResponse(responseData))
					return JsonConvert.DeserializeObject<Leaderboards>(responseData).data;

				dynamic data = JsonConvert.DeserializeObject(responseData);

				throw new CODException(data.data.message.ToString());

			}
		}

		/// <summary>
		/// Get leaderboard data relative to a username.
		/// </summary>
		/// <param name="platform">Platform to get leaderboards for</param>
		///	<param name="time">Length of time to return leaderboard data for</param>
		/// <param name="type">Type of mode to search for</param>
		/// <param name="mode">Gamemode to get leaderboard data for</param>
		/// <param name="username">User to find on leaderboards</param>
		/// <returns>Leaderboard data</returns>
		public static Leaderboards.Data GetLeaderboards(Platform platform, Time time, Type type, Gamemode mode, string username)
		{
			using (var client = new HttpClient())
			{
				//Business logic
				if (type == Type.Arena && !IsValidArenaMode(mode)) throw new CODException($"Mode {mode.ToString()} is not a valid Arena mode.");
				if (type == Type.Hardcore && !IsValidHardcoreMode(mode)) throw new CODException($"Mode {mode.ToString()} is not a valid Hardcore mode.");

				var response = client.GetAsync($"{Utilities.BO3_LEADERBOARDS_URL}platform/{platform.GetDescription()}/time/{time.GetDescription()}/type/{type.GetDescription()}/mode/{mode.GetDescription()}/gamer/{username}/").Result;

				if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"Bad response {response.StatusCode}");

				var responseData = response.Content.ReadAsStringAsync().Result;

				if (Utilities.ValidResponse(responseData))
					return JsonConvert.DeserializeObject<Leaderboards>(responseData).data;

				dynamic data = JsonConvert.DeserializeObject(responseData);

				throw new CODException(data.data.message.ToString());
			}
		}
	}
}

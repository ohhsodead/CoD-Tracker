using System;
using COD.Models.WWII;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.ComponentModel;

namespace COD
{
	public static class WWII
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

			[Description("ctf")]
			CaptureTheFlag,

			[Description("sd")]
			SearchAndDestroy,

			[Description("dom")]
			Domination,

			[Description("ball")]
			Gridiron,

			[Description("hp")]
			Hardpoint,

			[Description("1v1")]
			OnevOne,

			[Description("raid")]
			War
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
				var response = client.GetAsync($"{Utilities.WWII_URL}platform/{platform.GetDescription()}/gamer/{username}/profile/").Result;

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
		/// <param name="mode">Gamemode to get leaderboard data for</param>
		/// <param name="page">Page to fetch</param>
		/// <returns>Leaderboard data</returns>
		public static Leaderboards.Data GetLeaderboards(Platform platform, Time time, Gamemode mode, int page = 1)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{Utilities.WWII_LEADERBOARDS_URL}platform/{platform.GetDescription()}/time/{time.GetDescription()}/type/core/mode/{mode.GetDescription()}/page/{page}/").Result;

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
		/// <param name="mode">Gamemode to get leaderboard data for</param>
		/// <param name="username">User to find on leaderboards</param>
		/// <returns>Leaderboard data</returns>
		public static Leaderboards.Data GetLeaderboards(Platform platform, Time time, Gamemode mode, string username)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{Utilities.WWII_LEADERBOARDS_URL}platform/{platform.GetDescription()}/time/{time.GetDescription()}/type/core/mode/{mode.GetDescription()}/gamer/{username}/").Result;

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

using System;
using COD.Models.IW;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.ComponentModel;

namespace COD
{
	public static class IW
	{

		public enum Gamemode
		{
			[Description("career")]
			Career,

			[Description("war")]
			TDM,

			[Description("dm")]
			FreeForAll,

			[Description("dom")]
			Domination,

			[Description("sd")]
			SearchAndDestroy,

			[Description("hp")]
			Hardpoint,

			[Description("conf")]
			KillConfirmed,

			[Description("front")]
			Frontline,

			[Description("tdef")]
			Defender,

			[Description("grnd")]
			DropZone,

			[Description("gun")]
			GunGame,

			[Description("infect")]
			Infected,

			[Description("ball")]
			Uplink,

			[Description("ctf")]
			CaptureTheFlag,
		}

		public enum Sort
		{
			[Description("score")]
			Score,

			[Description("wins")]
			Wins,

			[Description("kills")]
			Kills,
		}

		public static Profile.Data GetProfile(Platform platform, string username)
		{
			using (var client = new HttpClient())
			{
				var response = client.GetAsync($"{Utilities.IW_URL}platform/{platform.GetDescription()}/gamer/{username}/profile/").Result;

				if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"Bad response {response.StatusCode}"); //TODO: Use proper exception

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
		/// <param name="order">Order of the results. Only applies to Gamemode.Career.</param>
		/// <returns>Leaderboard data</returns>
		public static Leaderboards.Data GetLeaderboards(Platform platform, Time time, Gamemode mode, int page = 1, Sort order = Sort.Kills)
		{
			using (var client = new HttpClient())
			{
				
				var url = $"{Utilities.IW_LEADERBOARDS_URL}platform/{platform.GetDescription()}/time/{time.GetDescription()}/type/core/mode/{mode.GetDescription()}/page/{page}/";

				//Since sorting only applies to career
				if (mode == Gamemode.Career) url += $"?sort={order.ToString().ToLower()}";

				var response = client.GetAsync(url).Result;

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
		/// <param name="order">Order of the results. Only applies to Gamemode.Career.</param>
		/// <returns>Leaderboard data</returns>
		public static Leaderboards.Data GetLeaderboards(Platform platform, Time time, Gamemode mode, string username, Sort order = Sort.Kills)
		{
			using (var client = new HttpClient())
			{

				var url = $"{Utilities.IW_LEADERBOARDS_URL}platform/{platform.GetDescription()}/time/{time.GetDescription()}/type/core/mode/{mode.GetDescription()}/gamer/{username}/";

				//Since sorting only applies to career
				if (mode == Gamemode.Career) url += $"?sort={order.ToString().ToLower()}";

				var response = client.GetAsync(url).Result;

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

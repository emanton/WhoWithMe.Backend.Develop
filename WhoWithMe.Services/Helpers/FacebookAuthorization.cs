using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WhoWithMe.Services.Helpers
{
	public class FacebookTokenValidationResult
	{
		[JsonProperty("data")]
		public FacebookTokenValidationData Data { get; set; }
	}

	public class FacebookTokenValidationData
	{
		[JsonProperty("app_id")]
		public string AppId { get; set; }
		[JsonProperty("type")]
		public string Type { get; set; }
		[JsonProperty("application")]
		public string Application { get; set; }
		[JsonProperty("is_valid")]
		public bool IsValid { get; set; }
		[JsonProperty("user_id")]
		public string UserId { get; set; }

		// TODO more fields
	}

	public class FacebookUserInfoResult
	{
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("first_name")]
		public string FirstName { get; set; }
		[JsonProperty("last_name")]
		public string LastName { get; set; }
		[JsonProperty("picture")]
		public FacebookPicture Picture { get; set; }
		[JsonProperty("email")]
		public string Email { get; set; }
		[JsonProperty("phone")]
		public string Phone { get; set; }
	}

	public class FacebookPicture
	{
		[JsonProperty("data")]
		public FacebookPictureData Data { get; set; }
	}

	public class FacebookPictureData
	{
		[JsonProperty("height")]
		public long Height { get; set; }
		[JsonProperty("is_silhouette")]
		public bool iIsSilhouette { get; set; }
		[JsonProperty("url")]
		public string Url { get; set; }
		[JsonProperty("width")]
		public long Width { get; set; }
	}

	public static class FacebookAuthorization
	{
		// phone
		private static string tokenValidationUrl = "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";
		private static string userInfoUrl = "https://graph.facebook.com/me?fields=first_name,last_name,picture,email&access_token={0}";
		private static string appId = "2735362606749096";
		private static string appSecret = "ab085fd41f10e4fabc13823e6b9f1d92";
		private static HttpClient client = new HttpClient();

		public static async Task<FacebookUserInfoResult> ValidateAccessTokenAsync(string accessToken)
		{
			string queryUrl = String.Format(tokenValidationUrl, accessToken, appId, appSecret);
			var stringJson = await MakeRequest(queryUrl);
			FacebookTokenValidationResult result = JsonConvert.DeserializeObject<FacebookTokenValidationResult>(stringJson);

			FacebookUserInfoResult userInfo = await GetUserInfoAsync(accessToken);
			if(userInfo.Id == result.Data.UserId && result.Data.IsValid)
			{
				return userInfo;
			}

			throw new Exception("Wrong access token");
		}

		public static async Task<FacebookUserInfoResult> GetUserInfoAsync(string accessToken)
		{
			string queryUrl = String.Format(userInfoUrl, accessToken);
			var stringJson = await MakeRequest(queryUrl);
			FacebookUserInfoResult result = JsonConvert.DeserializeObject<FacebookUserInfoResult>(stringJson);
			return result;
		}

		private static async Task<string> MakeRequest(string query)
		{
			
			HttpResponseMessage response = await client.GetAsync(query);
			try
			{
				response.EnsureSuccessStatusCode();
				return await response.Content.ReadAsStringAsync();
			}
			catch(Exception ex)
			{
				JObject res = JObject.Parse(await response.Content.ReadAsStringAsync());
				throw new Exception(res["error"]["message"].ToString());
			}
		}
	}
}

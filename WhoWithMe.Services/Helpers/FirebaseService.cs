using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WhoWithMe.Services.Helpers
{
	public class CustomNotification
	{
		public string UserToken { get; set; }
		public string NotificationTopic { get; set; }
		public string NotificationTitle { get; set; }
		public string NotificationBody { get; set; }
	}

	public static class FirebaseService
	{
		public static async Task<string> Test(CustomNotification notification)
		{
			var defaultApp = FirebaseApp.Create(new AppOptions()
			{
				Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FirebaseCred.json")),
			});
			Message message = new Message()
			{
				Data = new Dictionary<string, string>()
				{
					["TestDataField"] = "Test"
				},
				Notification = new Notification
				{
					Title = notification.NotificationTitle,
					Body = notification.NotificationBody
				},
				Token = notification.UserToken,
				Topic = notification.NotificationTopic
			};

			FirebaseMessaging messaging = FirebaseMessaging.DefaultInstance;
			string result = await messaging.SendAsync(message);
			return result;
			//Console.WriteLine(result); //projects/myapp/messages/2492588335721724324
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace GiveOrTake.FrontEnd.Shared.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private static readonly string DefaultValue = string.Empty;

		private const string SettingsKey = "settings_key";
		private const string AccessTokenKey = "access_token_settings_key";
		private const string FacebookAccessTokenKey = "facebook_access_token_settings_key";
		private const string ThisDeviceKey = "this_device_settings_key";

		#endregion


		public static string GeneralSettings
		{
			get { return AppSettings.GetValueOrDefault(SettingsKey, DefaultValue); }
			set { AppSettings.AddOrUpdateValue(SettingsKey, value); }
		}

		public static string AccessToken
		{
			get { return AppSettings.GetValueOrDefault(AccessTokenKey, DefaultValue); }
			set { AppSettings.AddOrUpdateValue(AccessTokenKey, value); }
		}

		public static string FacebookAccessToken
		{
			get { return AppSettings.GetValueOrDefault(FacebookAccessTokenKey, DefaultValue); }
			set { AppSettings.AddOrUpdateValue(FacebookAccessTokenKey, value); }
		}

		public static bool IsLoggedIn => !String.IsNullOrWhiteSpace(AccessToken);

		public static string DeviceId
		{
			get { return AppSettings.GetValueOrDefault(ThisDeviceKey, DefaultValue); }
			set { AppSettings.AddOrUpdateValue(ThisDeviceKey, value); }
		}
	}
}

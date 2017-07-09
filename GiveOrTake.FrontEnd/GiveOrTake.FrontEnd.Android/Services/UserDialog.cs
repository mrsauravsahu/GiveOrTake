using System;
using GiveOrTake.FrontEnd.Shared.Services;
using GiveOrTake.FrontEnd.Droid.Services;
using Acr.UserDialogs;

[assembly: Xamarin.Forms.Dependency(typeof(UserDialog))]
namespace GiveOrTake.FrontEnd.Droid.Services
{
	public class UserDialog : IUserDialog
	{
		public IUserDialogs Instance => UserDialogs.Instance;
		public void Show(string message, string title = null, TimeSpan? duration = default(TimeSpan?))
		{
			Instance.Toast(message, duration);
		}
	}
}
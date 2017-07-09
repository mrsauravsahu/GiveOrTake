using System;
using GiveOrTake.FrontEnd.Shared.Services;
using Acr.UserDialogs;
using GiveOrTake.FrontEnd.UWP.Services;

[assembly: Xamarin.Forms.Dependency(typeof(UserDialog))]
namespace GiveOrTake.FrontEnd.UWP.Services
{
	public class UserDialog : IUserDialog
	{
		public IUserDialogs Instance => UserDialogs.Instance;
		public void Show(string message, string title = null, TimeSpan? duration = default(TimeSpan?))
		{
			Instance.Alert(message, title, "Close");
		}
	}
}

using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace GiveOrTake.FrontEnd.Shared.ViewModels
{
	public class AboutViewModel : BaseViewModel
	{
		public AboutViewModel()
		{
			Title = "About";

			OpenLearnMoreLinkCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
			OpenGitHubLinkCommand = new Command(() => Device.OpenUri(new Uri("https://github.com/sauravMSFT/giveortake")));
		}

		/// <summary>
		/// Command to open browser to xamarin.com
		/// </summary>
		public ICommand OpenLearnMoreLinkCommand { get; }
		public ICommand OpenGitHubLinkCommand { get; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GiveOrTake.FrontEnd.Shared.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingView : ContentView
	{
		public LoadingView()
		{
			InitializeComponent();
		}

		public static readonly BindableProperty MessageProperty = BindableProperty.Create(nameof(Message), typeof(string), typeof(LoadingView));

		public string Message
		{
			get { return (string)GetValue(MessageProperty); }
			set { SetValue(MessageProperty, value); }
		}
	}
}
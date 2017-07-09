using System;
using System.Collections.Generic;
using System.Text;

namespace GiveOrTake.FrontEnd.Shared.Services
{
	public interface IUserDialog
	{
		void Show(string message, string title = null, TimeSpan? duration = null);
	}
}

using GiveOrTake.Database;
using GiveOrTake.FrontEnd.Shared.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GiveOrTake.FrontEnd.Shared.Services
{
	public class Server
	{
		internal static DataRetrieval DataRetrieval => DependencyService.Get<DataRetrieval>();
		internal static DataStore DataStore => DependencyService.Get<DataStore>();

		public async Task<List<Transaction>> GetTransactionsAsync()
		{
			try
			{
				var response = await NetworkCall.Instance.GetAsync(API.Device);
				response.EnsureSuccessStatusCode();

				var data = await response.Content.ReadAsStringAsync();

				return await Task.Run(() => JsonConvert.DeserializeObject<List<Transaction>>(data));
			}
			catch { throw; }
		}

		public async Task<bool> SendTransactionAsync(Transaction t)
		{
			try
			{
				var item = await DataStore.GetAssociatedItemAsync(t);

				var tran = new Transaction
				{
					TransactionId = t.TransactionId,
					OccurrenceDate = t.OccurrenceDate,
					ExpectedCompletionDate = t.ExpectedCompletionDate,
					CompletionDate = t.CompletionDate,
					Description = t.Description,
					DeviceId = t.DeviceId,
					ItemId = t.ItemId,
					Name = t.Name,
					TransactionType = t.TransactionType,
					UserId = t.UserId,
					Item = new Item
					{
						ItemId = item.ItemId,
						Name = item.Name
					}
				};

				await NetworkCall.Instance.PostAsync(API.Device, tran);
			}
			catch { throw; }
			return true;
		}

		public async Task SyncDataAsync()
		{
			var deviceId = new Guid(Settings.DeviceId);

			var data = await DataStore.GetDataToSynchronizeForThisDeviceIdAsync(deviceId);

			var response = await NetworkCall.Instance.PostAsync($"{API.Device}/{deviceId}", data);
		}
	}
}
